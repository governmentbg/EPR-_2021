using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPRO.Core.Models
{
    public class DismissalSearchViewModel
    {
      public int Id { get; set; }

        [Display(Name = "Съд")]
        public int CourtId { get; set; }

        [Display(Name = "Съдия")]
        public string JudgeName { get; set; }

        [Display(Name = "Вид отвод")]
        public int DismissalType { get; set; }

        [Display(Name = "Вид дело")]
        public int CaseType { get; set; }

        [Display(Name = "Дело номер")]
        public int CaseNumber { get; set; }

        [Display(Name = "Дело година")]
        public int CaseYear { get; set; }

        [Display(Name = "Период от")]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Период до")]
        public DateTime DateTo { get; set; }

        [Display(Name = "Основание")]
        public string Reason { get; set; }
    }
}
