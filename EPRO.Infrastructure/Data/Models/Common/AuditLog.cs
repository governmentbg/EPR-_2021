using EPRO.Infrastructure.Data.Models.Identity;
using EPRO.Infrastructure.Data.Models.Nomenclatures;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models.Common
{
    /// <summary>
    /// Журнал на промените
    /// </summary>
    [Table("audit_log")]
    public class AuditLog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("date_wrt")]
        public DateTime DateWrt { get; set; }

        [Column("court_id")]
        public int? CourtId { get; set; }

        [Column("operation")]
        public string Operation { get; set; }

        [Column("object_info")]
        public string ObjectInfo { get; set; }

        [Column("action_info")]
        public string ActionInfo { get; set; }

        [Column("request_url")]
        public string RequestUrl { get; set; }

        [Column("client_ip")]
        public string ClientIP { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey(nameof(CourtId))]
        public virtual Court Court { get; set; }
    }
}
