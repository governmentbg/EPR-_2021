using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models.Nomenclatures
{
    /// <summary>
    /// Вид дело по ПАС
    /// </summary>
    [Table("nom_case_type")]
    [Display(Name = "Вид дело")]
    public class CaseType : BaseCommonNomenclature
    {
        
    }
}
