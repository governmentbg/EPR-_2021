using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPRO.Infrastructure.Data.DataContracts
{
    /// <summary>
    /// Описание на грешка при трансфер
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Код на грешка
        /// </summary>
        [JsonProperty("errorType")]
        public string ErrorType { get; set; }

        /// <summary>
        /// Причина за грешката
        /// </summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Описание на грешката
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Наименование на грешното поле от заявката
        /// </summary>
        [JsonProperty("faultyAttribute")]
        public string FaultyAttribute { get; set; }
    }
}
