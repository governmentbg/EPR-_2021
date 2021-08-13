using EPRO.Infrastructure.Constants;
using EPRO.Infrastructure.Data.Common;
using EPRO.Infrastructure.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPRO.Extensions
{
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        private readonly IRepository repo;
        public ApplicationClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<IdentityOptions> options,
            IRepository _repo) : base(userManager, roleManager, options)
        {
            repo = _repo;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(CustomClaimType.Court, (user.CourtId > 0) ? user.CourtId.ToString() : "null"));
            ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(CustomClaimType.FullName, user.FullName));
            return principal;
        }



    }
}
