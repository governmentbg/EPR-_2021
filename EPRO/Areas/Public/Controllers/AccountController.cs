using EPRO.Core.Contracts;
using EPRO.Infrastructure.Constants;
using EPRO.Infrastructure.Data.Models.Identity;
using EPRO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace EPRO.Areas.Controllers
{
    /// <summary>
    /// Управление по потребители - публичен достъп
    /// </summary>
    [Area("public")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ILogger logger;
        private readonly IConfiguration config;
        private readonly IAccountService accountService;
        private readonly IStringLocalizer localizer;


        public AccountController(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<ApplicationRole> _roleManager,
            ILogger<AccountController> _logger,
            IAccountService _accountService,
            IStringLocalizer<AccountController> _localizer,
            IConfiguration _config)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            logger = _logger;
            accountService = _accountService;
            localizer = _localizer;
            config = _config;
        }

        /// <summary>
        /// Вход в системата
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null, string error = null)
        {
            var model = new LoginVM
            {
                ReturnUrl = returnUrl
            };

            model.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.errorMessage = error;
            }
            if (config.GetValue<bool>("Authentication:PasswordLoginEnabled"))
            {
                model.LoginWithPassword = true;
            }

            return View(model);
        }

        /// <summary>
        /// Изход от системата
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            return Content("ok");
        }

        /// <summary>
        /// Обработване на отказан достъп до функционалност
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl)
        {
            TempData[Core.Constants.MessageConstant.ErrorMessage] = Core.Constants.MessageConstant.Values.Unauthorized;
            return LocalRedirect("/");
        }

        /// <summary>
        /// Вход чрез външен доставчик на ауторизация - вход с КЕП
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                logger.LogError($"Error from external provider: {remoteError}");

                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                logger.LogError("Error loading external login information.");

                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            ApplicationUser user = null;
            if (info.LoginProvider == "IdStampIT")
            {
                user = await accountService.GetByUIC(info.ProviderKey);

                if (user == null || !user.IsActive)
                {
                    return RedirectToAction("Login", new { ReturnUrl = returnUrl, error = "Невалиден потребител" });
                }
            }
            if (user == null)
            {
                user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            }
            if (user != null)
            {
                var claims = await userManager.GetClaimsAsync(user);
                var certNoClaim = claims.FirstOrDefault(c => c.Type == CustomClaimType.IdStampit.CertificateNumber);
                var currentCertNoClaim = info.Principal.Claims.FirstOrDefault(c => c.Type == CustomClaimType.IdStampit.CertificateNumber);

                if (currentCertNoClaim != null)
                {
                    if (certNoClaim != null)
                    {
                        await userManager.ReplaceClaimAsync(user, certNoClaim, currentCertNoClaim);
                    }
                    else
                    {
                        await userManager.AddClaimAsync(user, currentCertNoClaim);
                    }
                }

                await signInManager.SignInAsync(user, isPersistent: false);

                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("AccessDenied");
        }

        /// <summary>
        /// Обработка на грешка при избор на КЕП
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginCertError(string error)
        {
            logger.LogError(error);

            return RedirectToAction(nameof(Login), new { error = "Моля изберете валиден сертификат." });
        }


    }
}