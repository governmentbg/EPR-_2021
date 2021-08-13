using System;
using System.Collections.Generic;
using System.Text;

namespace EPRO.Infrastructure.ViewModels.Common
{
    public class DismissalListVM
    {
        public string Id { get; set; }
        public int EntryType { get; set; }
        public string JudgeName { get; set; }
        public string DismissalTypeName { get; set; }
        public string RegionName { get; set; }
        public string CourtName { get; set; }
        public string CaseNumber { get; set; }
        public int CaseYear { get; set; }
        public string CaseTypeName { get; set; }
        public string ReplaceJudgeName { get; set; }
        public DateTime? HearingDate { get; set; }
    }
}
