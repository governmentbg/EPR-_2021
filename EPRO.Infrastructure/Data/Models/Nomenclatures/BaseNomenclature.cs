using EPRO.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPRO.Infrastructure.Data.Models.Nomenclatures
{
    /// <summary>
    /// Author: Stamo Petkov
    /// Created: 14.11.2016
    /// Description: Интерфейс за достъп до полетата 
    /// на общи номенклатури
    /// </summary>
    public class BaseNomenclature : IBaseNomenclature
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Номер по ред")]
        public int OrderNumber { get; set; }

        [Display(Name = "Код")]
        public string Code { get; set; }

        [Display(Name = "Етикет")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        public string Label { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }


        [Display(Name = "Активен")]
        public bool IsActive { get; set; }
    }
}
