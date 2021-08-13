using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EPRO.Infrastructure.Data.Models.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}
