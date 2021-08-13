using EPRO.Infrastructure.Data.DataContracts;
using System.Threading.Tasks;

namespace EPRO.Core.Contracts
{
    /// <summary>
    /// Услуга за запис на данни за отводи, постъпили през API услуга
    /// </summary>
    public interface IApiService : IBaseService
    {
        /// <summary>
        /// Регистриране на отвод
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<DismissalRegistrationResponse> Insert(DismissalRegistrationRequest model);

        /// <summary>
        /// Актуализация на отвод
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<UpdateResponse> Update(DismissalRegistrationRequest model);

        /// <summary>
        /// Актуализация на нов съдия
        /// </summary>
        /// <param name="model"></param>
        /// <param name="courtCode"></param>
        /// <returns></returns>
        Task<UpdateResponse> ReplaceUpdate(ReplaceDismissalRequest model,string courtCode);

        /// <summary>
        /// Актуализация на файл на акт по отвод
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<UpdateResponse> DismissalActUpdate(ActPublicationRequest model);
    }
}
