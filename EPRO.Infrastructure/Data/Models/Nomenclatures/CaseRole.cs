using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models.Nomenclatures
{
    /// <summary>
    /// Роля в делото: Съдия-докладчик, член състав
    /// </summary>
    [Table("nom_case_role")]
    [Display(Name = "Роля в делото")]
    public class CaseRole : BaseCommonNomenclature
    {
        
    }
}
