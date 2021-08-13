using EPRO.Components;
using EPRO.Core.Constants;
using EPRO.Core.Contracts;
using EPRO.Core.Models;
using EPRO.Extensions;
using EPRO.Infrastructure.Constants;
using EPRO.Infrastructure.Data.Models.Identity;
using EPRO.Infrastructure.ViewModels.Account;
using EPRO.Infrastructure.ViewModels.Common;
using EPRO.Models;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using EPRO.Infrastructure.Data.Models.Nomenclatures;
using System;
using EPRO.Core.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace EPRO.Areas.Admin.Controllers
{
    /// <summary>
    /// Управление на потребители
    /// </summary>
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ILogger logger;
        private readonly IConfiguration config;
        private readonly IAccountService accountService;
        private readonly IAuditLogService auditService;
        private readonly IStringLocalizer localizer;
        private readonly INomenclatureService nomService;


        public AccountController(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<ApplicationRole> _roleManager,
            ILogger<AccountController> _logger,
            IAccountService _accountService,
            IStringLocalizer<AccountController> _localizer,
            IAuditLogService _auditService,
            INomenclatureService _nomService,
            IConfiguration _config)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            logger = _logger;
            accountService = _accountService;
            auditService = _auditService;
            nomService = _nomService;
            localizer = _localizer;
            config = _config;
        }

        [MenuItem("users")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ListData(IDataTablesRequest request)
        {
            var data = accountService.Select(request.Search?.Value);
            return new DataTablesJsonResult(request.GetResponse(data), true);
        }

        /// <summary>
        /// Регистриране на потребител
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            AccountVM model = new AccountVM()
            {
                CourtId = userContext.CourtId,
                IsActive = true
            };
            SetViewBag();
            return View(nameof(Edit), model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AccountVM model)
        {
            if (userContext.CourtId > 0)
            {
                model.CourtId = userContext.CourtId;
            }
            if (!model.Uic.IsEGN())
            {
                ModelState.AddModelError(nameof(AccountVM.Uic), "Невалидно ЕГН");
            }

            if (!ModelState.IsValid)
            {
                SetViewBag();
                return View(nameof(Edit), model);
            }

            var checkResult = await accountService.CheckUser(model);
            if (!checkResult.Result)
            {
                SetErrorMessage(checkResult.ErrorMessage);
                SetViewBag();
                return View(nameof(Edit), model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Uic = model.Uic,
                FullName = model.FullName,
                CourtId = model.CourtId,
                IsActive = model.IsActive
            };

            IdentityResult res = await userManager.CreateAsync(user);
            if (res.Succeeded)
            {
                Audit_Operation = NomenclatureConstants.AuditOperations.Add;
                Audit_Object = $"Потребител: {user.FullName}";
                SetSuccessMessage(MessageConstant.Values.SaveOK);
                return RedirectToAction(nameof(Edit), new { id = user.Id });
            }
            else
            {
                SetErrorMessage(MessageConstant.Values.SaveFailed);
                SetViewBag();
                return View(nameof(Edit), model);
            }
        }

        /// <summary>
        /// Редакция на потребител
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException();
            }
            var model = new AccountVM()
            {
                Id = user.Id,
                CourtId = user.CourtId,
                Uic = user.Uic,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive
            };
            Func<string, bool> whereRoles = x => true;
            if (!userContext.IsUserInRole(AccountConstants.Roles.GLOBAL_ADMIN))
            {
                whereRoles = x => x != AccountConstants.Roles.GLOBAL_ADMIN;
            }
            model.Roles = roleManager.Roles
                .ToList()
                .Select(x => x.Name)
                .Where(x => whereRoles(x))
                .Select(x => new CheckListVM
                {
                    Value = x,
                    Label = localizer[x].ToString(),
                    Checked = false
                }).OrderBy(x => x.Label).ToList();

            var userRoles = await userManager.GetRolesAsync(user);
            if (userRoles.Any(x => x == AccountConstants.Roles.GLOBAL_ADMIN)
                && !userContext.IsUserInRole(AccountConstants.Roles.GLOBAL_ADMIN))
            {
                SetErrorMessage("Нямате право да коригиране глобални администратори");
                return RedirectToAction(nameof(Index));
            }
            foreach (var role in model.Roles)
            {
                if (userRoles.Any(x => x == role.Value))
                {
                    role.Checked = true;
                }
            }
            SetViewBag();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountVM model)
        {
            if (userContext.CourtId.HasValue)
            {
                model.CourtId = userContext.CourtId.Value;
            }
            if (!model.Uic.IsEGN())
            {
                ModelState.AddModelError(nameof(AccountVM.Uic), "Невалидно ЕГН");
            }
            if (!ModelState.IsValid)
            {
                SetViewBag();
                return View(model);
            }

            var checkResult = await accountService.CheckUser(model);
            if (!checkResult.Result)
            {
                SetErrorMessage(checkResult.ErrorMessage);
                SetViewBag();
                return View(model);
            }

            //Редактиране на потребител
            var user = await userManager.FindByIdAsync(model.Id);
            bool savedIsLastive = user.IsActive;

            //Задаване на роли/групи
            var userRoles = await userManager.GetRolesAsync(user);
            var res = await userManager.RemoveFromRolesAsync(user, userRoles);
            if (res.Succeeded)
            {
                res = await userManager.AddToRolesAsync(user, model.Roles.Where(x => x.Checked).Select(x => x.Value));

                user.Uic = model.Uic;
                user.CourtId = model.CourtId;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.FullName = model.FullName;
                user.IsActive = model.IsActive;
                await userManager.UpdateAsync(user);
                Audit_Operation = NomenclatureConstants.AuditOperations.Edit;
                Audit_Object = $"Потребител: {user.FullName}";
                if (savedIsLastive == true && !model.IsActive)
                {
                    Audit_Object += " - деактивиран.";
                }
                if (!savedIsLastive == true && model.IsActive)
                {
                    Audit_Object += " - активиран.";
                }
                SetSuccessMessage(MessageConstant.Values.SaveOK);
            }
            else
            {
                SetErrorMessage(MessageConstant.Values.SaveFailed);
            }

            return RedirectToAction(nameof(Edit), new { id = model.Id });
        }

        private void SetViewBag()
        {
            ViewBag.CourtId_ddl = nomService.GetDropDownList<Court>(false).Prepend(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem("За всички съдилища", null)).ToList();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        /// <summary>
        /// Журнал на промените
        /// </summary>
        /// <returns></returns>
        public IActionResult AuditLog()
        {
            ViewBag.MenuItemValue = "auditLog";
            ViewBag.CourtId_ddl = nomService.GetDropDownList<Court>();
            ViewBag.viewAllCourts = userContext.CourtId == null || userContext.IsUserInRole(AccountConstants.Roles.GLOBAL_ADMIN);
            return View();
        }

        [HttpPost]
        public IActionResult AuditLog_ListData(IDataTablesRequest request, AuditLogFilterVM filter)
        {
            var data = auditService.Select(filter);
            return new DataTablesJsonResult(request.GetResponse(data), true);
        }
    }
}