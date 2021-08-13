using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPRO.Infrastructure.ViewModels.Common
{
    public class NewsVM
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }

    public class NewsFilterVM
    {
        [Display(Name = "Търсене")]
        public string Search { get; set; }
    }
}
