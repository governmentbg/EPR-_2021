using EPRO.Infrastructure.Data.Models.Identity;
using EPRO.Infrastructure.ViewModels.Account;
using EPRO.Infrastructure.ViewModels.Common;
using System.Linq;
using System.Threading.Tasks;

namespace EPRO.Core.Contracts
{
    /// <summary>
    /// Услуга за управление на потребители
    /// </summary>
    public interface IAccountService : IBaseService
    {
        /// <summary>
        /// Извличане на потребител по ЕГН
        /// </summary>
        /// <param name="uic"></param>
        /// <returns></returns>
        Task<ApplicationUser> GetByUIC(string uic);

        /// <summary>
        /// Списък на регистрирани потребители
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        IQueryable<AccountVM> Select(string fullName);

        /// <summary>
        /// Валидиране на данни при регистриране на потребител
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SaveResultVM> CheckUser(AccountVM model);

        /// <summary>
        /// Тестова услуга за създаване на кодове за достъп
        /// </summary>
        /// <returns></returns>
        Task GenerateApiKeys();
    }
}
