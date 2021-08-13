using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models.Nomenclatures
{
    /// <summary>
    /// Вид съдебен акт по ПАС
    /// </summary>
    [Table("nom_act_type")]
    [Display(Name = "Вид съдебен акт")]
    public class ActType : BaseCommonNomenclature
    {
        
    }
}
