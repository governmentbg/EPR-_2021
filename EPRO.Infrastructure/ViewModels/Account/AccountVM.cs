using EPRO.Infrastructure.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPRO.Infrastructure.ViewModels.Account
{
    public class AccountVM
    {
        public string Id { get; set; }

        [Display(Name = "Съд")]
        public int? CourtId { get; set; }
        public string CourtName { get; set; }

        [Display(Name = "ЕГН")]
        [RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "Невалидна стойност")]
        [Required(ErrorMessage = "Въведете '{0}'.")]
        public string Uic { get; set; }

        [Display(Name = "Имена")]
        [Required(ErrorMessage = "Въведете '{0}'.")]
        public string FullName { get; set; }

        [Display(Name = "Електронна поща")]
        [Required(ErrorMessage = "Въведете '{0}'.")]
        [EmailAddress(ErrorMessage = "Невалидна електронна поща")]
        public string Email { get; set; }

        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }        

        [Display(Name = "Активен запис")]
        public bool IsActive { get; set; }

        [Display(Name = "Роли")]
        public IList<CheckListVM> Roles { get; set; }


        public AccountVM()
        {
            Roles = new List<CheckListVM>();
        }
    }
}
