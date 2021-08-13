using EPRO.Core.Extensions;
using System.ComponentModel.DataAnnotations;

namespace EPRO.Core.Models.FilterModels
{
    public class OrganizationFilterVM
    {
        [Display(Name = "Наименование")]
        public string Label { get; set; }

        [Display(Name = "Вид")]
        public int? OrganizationTypeId { get; set; }

        public void UpdateNullables()
        {
            Label = Label.EmptyToNull();
            OrganizationTypeId = OrganizationTypeId.EmptyToNull();
        }
    }
}
