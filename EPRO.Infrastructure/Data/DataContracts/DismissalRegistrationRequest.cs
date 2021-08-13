using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPRO.Infrastructure.Data.DataContracts
{
    /// <summary>
    /// Заявка за регистриране/актуализиране на отвод
    /// </summary>
    public class DismissalRegistrationRequest
    {
        /// <summary>
        /// Идентификатор на отвод - задължително при актуализация
        /// </summary>
        //[Required] on update
        [JsonProperty("dismissalId")]
        public Guid? DismissalId { get; set; }

        /// <summary>
        /// Код на съд
        /// </summary>
        [Required]
        [JsonProperty("court")]
        public string Court { get; set; }

        /// <summary>
        /// Код на точен вид дело
        /// </summary>
        [Required]
        [JsonProperty("caseType")]
        public string CaseType { get; set; }

        /// <summary>
        /// Номер на дело, 14 цифри
        /// </summary>
        [Required]
        [JsonProperty("caseNumber")]
        public string CaseNumber { get; set; }

        /// <summary>
        /// Година на делото
        /// </summary>
        [Required]
        [JsonProperty("caseYear")]
        public int CaseYear { get; set; }

        /// <summary>
        /// Код на вид отвод
        /// </summary>
        [Required]
        [JsonProperty("dismissalType")]
        public string DismissalType { get; set; }

        /// <summary>
        /// Уважено искане за отвод
        /// </summary>
        [JsonProperty("objectionUpheld")]
        public bool ObjectionUpheld { get; set; }

        /// <summary>
        /// Причина за отвода
        /// </summary>
        [Required]
        [JsonProperty("dismissalReason")]
        public string DismissalReason { get; set; }

        /// <summary>
        /// Код на роля на съдията в делото
        /// </summary>
        [Required]
        [JsonProperty("caseRole")]
        public string CaseRole { get; set; }

        /// <summary>
        /// Съдия
        /// </summary>
        [Required]
        public JudgeModel Judge { get; set; }

        /// <summary>
        /// Искане за отвод
        /// </summary>
        public ObjectionModel Objection { get; set; }

        /// <summary>
        /// Решение за отвода
        /// </summary>
        [Required]
        public DecisionModel Decision { get; set; }
    }
}
