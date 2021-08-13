using Newtonsoft.Json;
using System;

namespace EPRO.Infrastructure.Data.DataContracts
{
    /// <summary>
    /// Искане за отвод
    /// </summary>
    public class ObjectionModel
    {
        /// <summary>
        /// Вид документ, с който е заявено искането за отвод
        /// </summary>
        [JsonProperty("documentType")]
        public string DocumentType { get; set; }

        /// <summary>
        /// Номер на документа 
        /// </summary>
        [JsonProperty("documentNumber")]
        public int DocumentNumber { get; set; }

        /// <summary>
        /// Дата на документа
        /// </summary>
        [JsonProperty("documentDate")]
        public DateTime DocumentDate { get; set; }

        /// <summary>
        /// Имена на лицето, подало искането за отвод
        /// </summary>
        [JsonProperty("sideName")]
        public string SideName { get; set; }

        /// <summary>
        /// Вид на лицето в документа
        /// </summary>
        [JsonProperty("sideInvolmentKind")]
        public string SideInvolmentKind { get; set; }
    }
}
