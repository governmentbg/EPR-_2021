using System;

namespace EPRO.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public string InnerException { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorViewModel()
        {
            Title = "Грешка";
        }
    }
}
