using EPRO.Core.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPRO.Core.Models.FilterModels
{
    public class DismissalFilterVM
    {
        [Display(Name = "Начин на подаване")]
        public int? EntryType { get; set; }
        [Display(Name = "Съд")]
        public int? CourtId { get; set; }
        [Display(Name = "Съдия")]
        public string JudgeName { get; set; }
        [Display(Name = "Вид отвод")]
        public int? DismissalTypeId { get; set; }
        [Display(Name = "Вид дело")]
        public int? CaseTypeId { get; set; }
        [Display(Name = "Дело номер")]
        public string CaseNumber { get; set; }
        [Display(Name = "Дело година")]
        public int? CaseYear { get; set; }
        [Display(Name = "Период от")]
        public DateTime? PeriodFrom { get; set; }
        [Display(Name = "Период до")]
        public DateTime? PeriodTo { get; set; }
        [Display(Name = "Основание")]
        public string DismissalReason { get; set; }

        public string reCaptchaToken { get; set; }
        public bool ActiveOnly { get; set; }

        public void UpdateNullables()
        {
            EntryType = EntryType.EmptyToNull().EmptyToNull(-2);
            CourtId = CourtId.EmptyToNull().EmptyToNull(-2);
            DismissalTypeId = DismissalTypeId.EmptyToNull().EmptyToNull(-2);
            CaseTypeId = CaseTypeId.EmptyToNull().EmptyToNull(-2);
            JudgeName = JudgeName.EmptyToNull();
            CaseNumber = CaseNumber.EmptyToNull();
            DismissalReason = DismissalReason.EmptyToNull();
        }
    }
}
