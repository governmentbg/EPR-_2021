using System.ComponentModel.DataAnnotations;

namespace EPRO.Core.Models
{
    public class DismissalManageVM
    {
        public string DismissalId { get; set; }
        [Display(Name = "Причина")]
        [Required(ErrorMessage = "Въведете '{0}'.")]
        public string Reason { get; set; }
    }
}
