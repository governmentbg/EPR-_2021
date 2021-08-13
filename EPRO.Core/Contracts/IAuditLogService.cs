using EPRO.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EPRO.Core.Contracts
{
    /// <summary>
    /// Услуга за управление на журнал на промените
    /// </summary>
    public interface IAuditLogService
    {
        /// <summary>
        /// Регистриране на ново събитие в журнала
        /// </summary>
        /// <param name="courtId">id на съд</param>
        /// <param name="operation">Вид операция</param>
        /// <param name="objectInfo">Данни за операцията</param>
        /// <param name="clientIp">IP адрес на заявителя на данни</param>
        /// <param name="requestUrl">Адрес на търсения ресурс</param>
        /// <param name="actionInfo">Допълнителна информация</param>
        /// <returns></returns>
        Task<bool> SaveAuditLog(int? courtId, string operation, string objectInfo, string clientIp, string requestUrl, string actionInfo = null);

        /// <summary>
        /// Извлича данни от журнала по подадения филтър
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IQueryable<AuditLogVM> Select(AuditLogFilterVM filter);
    }
}
