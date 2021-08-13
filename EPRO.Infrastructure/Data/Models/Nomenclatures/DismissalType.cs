using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models.Nomenclatures
{
    /// <summary>
    /// Вид отвод: Отвод или самоотвод
    /// </summary>
    [Table("nom_dismissal_type")]
    [Display(Name = "Вид отвод")]
    public class DismissalType : BaseCommonNomenclature
    {
        
    }
}
