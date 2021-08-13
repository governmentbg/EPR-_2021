using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPRO.Infrastructure.Data.DataContracts
{
    /// <summary>
    /// Описание на решение по отвода
    /// </summary>
    public class DecisionModel
    {
        /// <summary>
        /// Вид заседание
        /// </summary>
        [Required]
        [JsonProperty("hearingType")]
        public string HearingType { get; set; }

        /// <summary>
        /// Дата на заседанието
        /// </summary>
        [Required]
        [JsonProperty("hearingDate")]
        public DateTime HearingDate { get; set; }

        /// <summary>
        /// Вид акт
        /// </summary>
        [Required]
        [JsonProperty("actType")]
        public string ActType { get; set; }

        /// <summary>
        /// Номер на акта
        /// </summary>
        [Required]
        [JsonProperty("actNumber")]
        public int ActNumber { get; set; }

        /// <summary>
        /// Дата на акта
        /// </summary>
        [Required]
        [JsonProperty("actDeclaredDate")]
        public DateTime ActDeclaredDate { get; set; }
    }
}
