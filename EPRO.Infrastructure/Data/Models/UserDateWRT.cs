using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Models.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models
{
    public class UserDateWRT : IExpiredInfo, IUserDateWRT
    {
        [Column("user_id")]
        public string UserId { get; set; }

        [Column("date_wrt")]
        [Display(Name = "Последна актуализация")]
        public DateTime DateWrt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Column("user_expired_id")]
        public string UserExpiredId { get; set; }
        [Column("date_expired")]
        public DateTime? DateExpired { get; set; }
        [Column("description_expired")]
        public string DescriptionExpired { get; set; }

        [ForeignKey(nameof(UserExpiredId))]
        public virtual ApplicationUser UserExpired { get; set; }
    }
}
