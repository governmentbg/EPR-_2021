using EPRO.Infrastructure.Data.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPRO.Infrastructure.ViewModels.Common
{
    public class DismissalPrintVM
    {
        public Dismissal Data { get; set; }
        public string CourtName { get; set; }
        public string CaseTypeName { get; set; }
        public string DismissalTypeName { get; set; }
        public string HearingTypeName { get; set; }
        public string ActTypeName { get; set; }
        public string CaseRoleName { get; set; }
        public string ActFileName { get; set; }
    }
}
