using EPRO.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPRO.Core.Models
{
    public class AuditLogVM
    {
        public string CourtName { get; set; }
        public string Operation { get; set; }
        public string UserFullName { get; set; }
        public string Object { get; set; }
        public DateTime DateWrt { get; set; }
        public string ClientIp { get; set; }
    }

    public class AuditLogFilterVM
    {
        [Display(Name = "Съд")]
        public int? CourtId { get; set; }

        [Display(Name = "Дата от")]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Дата до")]
        public DateTime? DateTo { get; set; }

        [Display(Name = "Потребител")]
        public string UserName { get; set; }

        [Display(Name = "Обект")]
        public string Object { get; set; }

        public void UpdateNullables()
        {
            CourtId = CourtId.EmptyToNull().EmptyToNull(-2);
            UserName = UserName.EmptyToNull();
            Object = Object.EmptyToNull();
        }
    }
}
