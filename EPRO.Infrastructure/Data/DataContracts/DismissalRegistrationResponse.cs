using Newtonsoft.Json;
using System;

namespace EPRO.Infrastructure.Data.DataContracts
{
    /// <summary>
    /// Отговор на заявката за регистриране на отвод
    /// </summary>
    public class DismissalRegistrationResponse
    {
        /// <summary>
        /// Идентификатор на отвод, при успешен запис
        /// </summary>
        [JsonProperty("dismissalId")]
        public Guid? DismissalId { get; set; }

        /// <summary>
        /// Описание на грешка
        /// </summary>
        public ErrorModel Error { get; set; }
    }
}
