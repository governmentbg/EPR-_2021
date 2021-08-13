using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPRO.Infrastructure.Data.DataContracts
{
    /// <summary>
    /// Заявка за добавяне на нов съдия по отвод
    /// </summary>
    public class ReplaceDismissalRequest
    {
        /// <summary>
        /// Идентификатор на отвода
        /// </summary>
        [JsonProperty("dismissalId")]
        [Required]
        public Guid DismissalId { get; set; }

        /// <summary>
        /// Нов съдия
        /// </summary>
        [Required]
        public JudgeModel ReplaceJudge { get; set; }
    }
}
