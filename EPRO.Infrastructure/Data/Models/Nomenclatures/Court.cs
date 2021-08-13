using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models.Nomenclatures
{
    /// <summary>
    /// Съдилища
    /// </summary>
    [Table("nom_court")]
    [Display(Name = "Съдилища")]
    public class Court : BaseCommonNomenclature
    {
        public int? ApealRegionId { get; set; }

        [ForeignKey(nameof(ApealRegionId))]
        public virtual ApealRegion ApealRegion { get; set; }
    }
}
