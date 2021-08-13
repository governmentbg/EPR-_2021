using System;
using System.Collections.Generic;
using System.Text;

namespace EPRO.Infrastructure.ViewModels.Common
{
    public class CheckListVM
    {
        public bool Checked { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }
        public string Warrning { get; set; }
    }
}
