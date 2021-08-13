using EPRO.Infrastructure.Data.Models.Nomenclatures;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models.Common
{
    /// <summary>
    /// Ключове за достъп през API услуга на ниво съд
    /// </summary>
    [Table("api_keys")]
    public class ApiKey
    {
        [Key]
        public int Id { get; set; }

        public int CourtId { get; set; }

        [Required]
        [StringLength(50)]
        public string AppKey { get; set; }

        [Required]
        [StringLength(100)]
        public string AppSecret { get; set; }

        [ForeignKey(nameof(CourtId))]
        public virtual Court Court { get; set; }
    }
}
