using System;
using System.Collections.Generic;
using System.Text;

namespace EPRO.Infrastructure.ViewModels.Common
{
    public class SaveResultVM
    {
        public bool Result { get; set; }
        public string ErrorMessage { get; set; }
        public string AuditInfo { get; set; }

        public SaveResultVM()
        {

        }

        public SaveResultVM(bool result, string errorMessage = null)
        {
            this.Result = result;
            this.ErrorMessage = errorMessage;
        }
    }
}
