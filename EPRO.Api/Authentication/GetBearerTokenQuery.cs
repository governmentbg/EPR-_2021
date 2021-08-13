using EPRO.Infrastructure.Data.Common;
using EPRO.Infrastructure.Data.Models.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EPRO.Api.Authentication
{
    /// <summary>
    /// Достъп до информация на потребителя по Bearer Token
    /// </summary>
    public class GetBearerTokenQuery : IGetBearerTokenQuery
    {
        private readonly IRepository repo;

        /// <summary>
        /// Инжектиране на зависимости
        /// </summary>
        /// <param name="_repo">Услуга за достъп до Redis</param>
        public GetBearerTokenQuery(IRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        /// Извлича информация за потребителя по token
        /// </summary>
        /// <param name="token">Идентификационен token</param>
        /// <returns></returns>
        public async Task<ApiKeyModel> GetDataByToken(string token)
        {
            return await repo.AllReadonly<ApiKey>()
                                .Include(x => x.Court)
                                .Where(x => x.AppKey == token)
                                .Select(x => new ApiKeyModel
                                {
                                    AppSecret = x.AppSecret,
                                    UserData = x.Court.Code
                                })
                                .FirstOrDefaultAsync();
        }
    }
}
