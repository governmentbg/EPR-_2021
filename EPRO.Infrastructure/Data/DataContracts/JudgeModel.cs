using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EPRO.Infrastructure.Data.DataContracts
{
    /// <summary>
    /// Описание на съдия
    /// </summary>
    public class JudgeModel
    {
        /// <summary>
        /// Имена на съдията
        /// </summary>
        [Required]
        [JsonProperty("judgeName")]
        public string JudgeName { get; set; }

        /// <summary>
        /// Флаг дали съдията е председател на състава
        /// </summary>
        [JsonProperty("isChairman")]
        public bool IsChairman { get; set; }
    }
}
