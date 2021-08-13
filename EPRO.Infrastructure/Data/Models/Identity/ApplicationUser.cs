using EPRO.Infrastructure.Data.Models.Nomenclatures;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public string Uic { get; set; }
        public string FullName { get; set; }      

        public bool IsActive { get; set; }
        public bool MustChangePassword { get; set; }
        public int? CourtId { get; set; }
        public virtual Court Court { get; set; }
    }
}
