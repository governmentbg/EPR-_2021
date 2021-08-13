using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models.Nomenclatures
{
    /// <summary>
    /// Вид заседание
    /// </summary>
    [Table("nom_hearing_type")]
    [Display(Name = "Вид заседание")]
    public class HearingType : BaseCommonNomenclature
    {
        
    }
}
