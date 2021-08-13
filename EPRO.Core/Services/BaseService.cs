using EPRO.Core.Contracts;
using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPRO.Core.Services
{
    public class BaseService : IBaseService
    {
        protected IRepository repo;
        protected IUserContext userContext;
        protected ILogger<BaseService> logger;

        public async Task<T> GetById<T>(object id) where T : class
        {
            return await repo.GetByIdAsync<T>(id);
        }

        protected Expression<Func<T, bool>> FilterActive<T>(DateTime? dtNow = null) where T : IDatePeriod
        {
            dtNow = dtNow ?? DateTime.Now;
            return x => x.DateStart <= dtNow && (x.DateEnd ?? DateTime.MaxValue) >= dtNow;
        }

        protected void SetUserDateWRT(IUserDateWRT model)
        {
            model.UserId = userContext.UserId;
            model.DateWrt = DateTime.Now;
        }

        protected DateTime dtNow = DateTime.Now;
    }
}
