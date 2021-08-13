using EPRO.Infrastructure.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPRO.Infrastructure.Data.Models.Nomenclatures
{
    public class BaseCommonNomenclature : ICommonNomenclature
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Номер по ред")]
        public int OrderNumber { get; set; }

        [Display(Name = "Код")]
        public string Code { get; set; }

        [Display(Name = "Име")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        public string Label { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Начална дата")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        public DateTime DateStart { get; set; }

        [Display(Name = "Крайна дата")]
        public DateTime? DateEnd { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; }
    }
}
