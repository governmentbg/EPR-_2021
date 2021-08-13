using Newtonsoft.Json;

namespace EPRO.Infrastructure.Data.DataContracts
{
    /// <summary>
    /// Резултат от актуализиране на данни
    /// </summary>
    public class UpdateResponse
    {
        /// <summary>
        /// Флаг за успешен запис
        /// </summary>
        [JsonProperty("updateSuccessful")]
        public bool UpdateSuccessful { get; set; }

        /// <summary>
        /// Описание на грешка, при неуспешен запис
        /// </summary>
        public ErrorModel Error { get; set; }
    }
}
