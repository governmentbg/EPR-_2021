using EPRO.Core.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPRO.Core.Models.FilterModels
{
    public class ReportFilterVM
    {
        [Display(Name = "Начин на подаване")]
        public int? EntryType { get; set; }
        [Display(Name = "Апелативен район")]
        public int? ApealRegionId { get; set; }
        [Display(Name = "Съд")]
        public string CourtIds { get; set; }
        [Display(Name = "Съдия")]
        public string JudgeName { get; set; }
        [Display(Name = "Вид отвод")]
        public int? DismissalTypeId { get; set; }
        [Display(Name = "Вид дело")]
        public string CaseTypeIds { get; set; }
        [Display(Name = "Дело номер")]
        public string CaseNumber { get; set; }
        [Display(Name = "Дело година")]
        public int? CaseYear { get; set; }
        [Display(Name = "Заседание от")]
        public DateTime? PeriodFrom { get; set; }
        [Display(Name = "Заседание до")]
        public DateTime? PeriodTo { get; set; }
        [Display(Name = "Основание")]
        public string DismissalReason { get; set; }

        [Display(Name = "Вид заседание")]
        public int? HearingTypeId { get; set; }
        [Display(Name = "Вид акт")]
        public int? ActTypeId { get; set; }
        [Display(Name = "Роля в заседанието")]
        public int? CaseRoleId { get; set; }

        public void UpdateNullables()
        {
            EntryType = EntryType.EmptyToNull().EmptyToNull(-2);
            CourtIds = CourtIds.EmptyToNull().EmptyToNull("-2");
            DismissalTypeId = DismissalTypeId.EmptyToNull().EmptyToNull(-2);
            CaseTypeIds = CaseTypeIds.EmptyToNull().EmptyToNull("-2");
            JudgeName = JudgeName.EmptyToNull();
            CaseNumber = CaseNumber.EmptyToNull();
            DismissalReason = DismissalReason.EmptyToNull();
            HearingTypeId = HearingTypeId.EmptyToNull().EmptyToNull(-2);
            ActTypeId = ActTypeId.EmptyToNull().EmptyToNull(-2);
            CaseRoleId = CaseRoleId.EmptyToNull().EmptyToNull(-2);
        }
    }
}
