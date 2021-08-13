using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Models.Nomenclatures;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRO.Infrastructure.Data.Models.Common
{
    /// <summary>
    /// Отводи
    /// </summary>
    [Table("dismissal")]
    public class Dismissal : UserDateWRT
    {
        [Column("id")]
        [Key]
        public string Id { get; set; }

        [Column("court_id")]
        [Display(Name = "Съд")]
        [Required(ErrorMessage = "Изберете '{0}.'")]
        [Range(1, 9999, ErrorMessage = "Изберете '{0}.'")]
        public int CourtId { get; set; }

        [Column("case_type_id")]
        [Display(Name = "Вид дело")]
        [Required(ErrorMessage = "Изберете '{0}.'")]
        [Range(1, 9999, ErrorMessage = "Изберете '{0}.'")]
        public int CaseTypeId { get; set; }

        /// <summary>
        /// Вид запис: 1 от API, 2 въведен през административен модул
        /// </summary>
        [Column("entry_type")]
        public int EntryType { get; set; }

        [Column("case_number")]
        [Display(Name = "Дело номер")]
        [Required(ErrorMessage = "Въведете '{0}.'")]
        public string CaseNumber { get; set; }

        [Column("case_year")]
        [Display(Name = "Дело година")]
        //[Required(ErrorMessage = "Изберете '{0}.'")]
        //[Range(1, 9999, ErrorMessage = "Изберете '{0}.'")]
        public int CaseYear { get; set; }

        [Column("judge_full_name")]
        [Display(Name = "Имена")]
        [Required(ErrorMessage = "Въведете '{0}.'")]
        public string JudgeFullName { get; set; }

        [Column("judge_is_chairman")]
        [Display(Name = "Съдията е председател на състава")]
        public bool JudgeIsChairman { get; set; }

        [Column("case_role_id")]
        [Display(Name = "Роля на съдия")]
        [Required(ErrorMessage = "Изберете '{0}.'")]
        [Range(1, 9999, ErrorMessage = "Изберете '{0}.'")]
        public int CaseRoleId { get; set; }

        [Column("dismissal_type_id")]
        [Display(Name = "Вид отвод")]
        [Required(ErrorMessage = "Изберете '{0}.'")]
        [Range(1, 9999, ErrorMessage = "Изберете '{0}.'")]
        public int DismissalTypeId { get; set; }

        [Column("objection_upheld")]
        [Display(Name = "Уважено искане за отвод")]
        public bool ObjectionUpheld { get; set; }

        [Column("dismissal_reason")]
        [Display(Name = "Мотиви за уважение или отхвърляне на искане за отвод / Мотиви за самоотвод")]
        public string DismissalReason { get; set; }

        [Column("document_type")]
        [Display(Name = "Вид входящ документ")]
        public string DocumentType { get; set; }

        [Column("document_document")]
        [Display(Name = "Номер на документ")]
        public int? DocumentNumber { get; set; }

        [Column("document_date")]
        [Display(Name = "Дата на документ")]
        public DateTime? DocumentDate { get; set; }

        [Column("side_full_name")]
        [Display(Name = "Имена")]
        public string SideFullName { get; set; }

        [Column("side_involvment_kind")]
        [Display(Name = "Качество на страната")]
        public string SideInvolmentKind { get; set; }

        [Column("hearing_type")]
        [Display(Name = "Вид заседание")]
        [Required(ErrorMessage = "Изберете '{0}.'")]
        [Range(1, 9999, ErrorMessage = "Изберете '{0}.'")]
        public int HearingTypeId { get; set; }

        [Column("hearing_date")]
        [Display(Name = "Дата на провеждане")]
        [Required(ErrorMessage = "Въведете '{0}.'")]
        public DateTime? HearingDate { get; set; }

        [Column("act_type_id")]
        [Display(Name = "Вид съдебен акт")]
        [Required(ErrorMessage = "Изберете '{0}.'")]
        [Range(1, 9999, ErrorMessage = "Изберете '{0}.'")]
        public int ActTypeId { get; set; }

        [Column("act_document")]
        [Display(Name = "Акт номер")]
        [Range(1, 9999, ErrorMessage = "Изберете '{0}.'")]
        [Required(ErrorMessage = "Въведете '{0}.'")]
        public int ActNumber { get; set; }

        [Column("act_declared_date")]
        [Display(Name = "Акт дата")]
        [Required(ErrorMessage = "Въведете '{0}.'")]
        public DateTime? ActDeclaredDate { get; set; }

        [Column("replacejudge_full_name")]
        [Display(Name = "Имена")]
        public string ReplaceJudgeFullName { get; set; }

        [Column("replacejudge_is_chairman")]
        [Display(Name = "Съдията е председател на състава")]
        public bool ReplaceJudgeIsChairman { get; set; }
        
        
        [Column("edit_motives")]
        [Display(Name = "Причини за редактиране на данни за отвод")]
        public string EditMotives { get; set; }

        [ForeignKey(nameof(CourtId))]
        public virtual Court Court { get; set; }

        [ForeignKey(nameof(CaseTypeId))]
        public virtual CaseType CaseType { get; set; }

        [ForeignKey(nameof(DismissalTypeId))]
        public virtual DismissalType DismissalType { get; set; }

        [ForeignKey(nameof(CaseRoleId))]
        public virtual CaseRole CaseRole { get; set; }

        [ForeignKey(nameof(ActTypeId))]
        public virtual ActType ActType { get; set; }

        [ForeignKey(nameof(HearingTypeId))]
        public virtual HearingType HearingType { get; set; }
    }
}

