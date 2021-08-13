using EPRO.Core.Contracts;
using EPRO.Core.Extensions;
using EPRO.Infrastructure.Constants;
using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Common;
using EPRO.Infrastructure.Data.Models;
using EPRO.Infrastructure.Data.Models.Common;
using EPRO.Infrastructure.Data.Models.Identity;
using EPRO.Infrastructure.ViewModels.Account;
using EPRO.Infrastructure.ViewModels.Common;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EPRO.Core.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(
            IRepository _repo,
            ILogger<AccountService> _logger,
            IUserContext _userContext)
        {
            repo = _repo;
            logger = _logger;
            userContext = _userContext;
        }

        public IQueryable<AccountVM> Select(string fullName)
        {
            Expression<Func<ApplicationUser, bool>> whereSearch = x => true;
            if (userContext.CourtId > 0)
            {
                whereSearch = x => x.CourtId == userContext.CourtId;
            }
            return repo.AllReadonly<ApplicationUser>()
                            .Include(x => x.Court)
                            .Where(x => EF.Functions.ILike(x.FullName, fullName.ToPaternSearch()))
                            .Where(whereSearch)
                            .Select(x => new AccountVM
                            {
                                Id = x.Id,
                                CourtName = (x.Court != null) ? x.Court.Label : "Всички съдилища",
                                Email = x.UserName,
                                FullName = x.FullName
                            }).AsQueryable();
        }

        public async Task<ApplicationUser> GetByUIC(string uic)
        {
            return await repo.All<ApplicationUser>()
                            .Where(x => x.Uic == uic)
                            .FirstOrDefaultAsync();
        }

        public async Task<SaveResultVM> CheckUser(AccountVM model)
        {
            if (!string.IsNullOrEmpty(model.Email))
            {
                model.Email = model.Email.ToLower();
            }
            if (await repo.AllReadonly<ApplicationUser>()
                        .Where(x => x.Email == model.Email && x.Id != model.Id)
                        .AnyAsync())
            {
                return new SaveResultVM(false, "Съществува потребител с такава електронна поща.");
            }

            if (await repo.AllReadonly<ApplicationUser>()
                        .Where(x => x.Uic == model.Uic && x.Id != model.Id)
                        .AnyAsync())
            {
                return new SaveResultVM(false, "Съществува потребител с такова ЕГН.");
            }

            return new SaveResultVM(true);
        }

        /// <summary>
        /// Този метод създава ключове за всички съдилища
        /// </summary>
        /// <returns></returns>
        public async Task GenerateApiKeys()
        {
            var courts = await repo.AllReadonly<Infrastructure.Data.Models.Nomenclatures.Court>()
                .ToListAsync();

            List<ApiKey> keys = new List<ApiKey>();

            foreach (var item in courts)
            {
                keys.Add(new ApiKey()
                {
                    AppKey = Guid.NewGuid().ToString().Replace("-", ""),
                    AppSecret = WebEncoders.Base64UrlEncode(GenerateRandomBytes(64)),
                    CourtId = item.Id
                });
            }

            await repo.AddRangeAsync(keys);
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Generate a cryptographically secure array of bytes with a fixed length
        /// </summary>
        /// <returns></returns>
        private byte[] GenerateRandomBytes(int numberOfBytes)
        {
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                byte[] byteArray = new byte[numberOfBytes];
                provider.GetBytes(byteArray);
                return byteArray;
            }
        }
    }
}
