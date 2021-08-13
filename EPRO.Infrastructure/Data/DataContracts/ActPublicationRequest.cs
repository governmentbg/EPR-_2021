using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPRO.Infrastructure.Data.DataContracts
{
    /// <summary>
    /// Заявка за актуализиране на файл към акт по отвод
    /// </summary>
    public class ActPublicationRequest
    {
        /// <summary>
        /// Идентификатор на отвод
        /// </summary>
        [JsonProperty("dismissalId")]
        [Required]
        public Guid DismissalId { get; set; }

        /// <summary>
        /// Име на файла, с разширението
        /// </summary>
        [JsonProperty("fileName")]
        [Required]
        public string FileName { get; set; }

        /// <summary>
        /// Съдържание на акта - Base64 encoded byte[]
        /// </summary>
        [JsonProperty("fileSource")]
        [Required]
        public string FileSource { get; set; }

        /// <summary>
        /// MimeType на файла 
        /// </summary>
        [JsonProperty("mimeType")]
        [Required]
        public string MimeType { get; set; }
    }
}
