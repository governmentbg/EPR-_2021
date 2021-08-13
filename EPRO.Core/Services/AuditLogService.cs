using EPRO.Core.Contracts;
using EPRO.Core.Extensions;
using EPRO.Core.Models;
using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Common;
using EPRO.Infrastructure.Data.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPRO.Core.Services
{
    public class AuditLogService : BaseService, IAuditLogService
    {
        public AuditLogService(
            IRepository _repo,
            ILogger<AuditLogService> _logger,
            IUserContext _userContext)
        {
            repo = _repo;
            logger = _logger;
            userContext = _userContext;
        }
        public async Task<bool> SaveAuditLog(int? courtId, string operation, string objectInfo, string clientIp, string requestUrl, string actionInfo = null)
        {
            try
            {
                var entity = new AuditLog()
                {
                    DateWrt = DateTime.Now,
                    UserId = userContext.UserId,
                    CourtId = courtId,
                    Operation = operation,
                    ObjectInfo = objectInfo,
                    ActionInfo = actionInfo,
                    ClientIP = clientIp,
                    RequestUrl = requestUrl
                };
                await repo.AddAsync(entity);
                await repo.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IQueryable<AuditLogVM> Select(AuditLogFilterVM filter)
        {
            filter.UpdateNullables();
            Expression<Func<AuditLog, bool>> whereCourt = x => true;
            if (filter.CourtId > 0)
            {
                whereCourt = x => x.CourtId == filter.CourtId;
            }
            if (userContext.CourtId > 0)
            {
                whereCourt = x => x.CourtId == userContext.CourtId;
            }
            return repo.AllReadonly<AuditLog>()
                            .Include(x => x.User)
                            .Include(x => x.Court)
                            .Where(whereCourt)
                            .Where(x => x.DateWrt >= (filter.DateFrom ?? x.DateWrt))
                            .Where(x => x.DateWrt <= (filter.DateTo.MakeEndDate() ?? x.DateWrt))
                            .Where(x => EF.Functions.ILike(x.User.FullName, filter.UserName.ToPaternSearch()))
                            .Where(x => EF.Functions.ILike(x.ObjectInfo, filter.Object.ToPaternSearch()))
                            .OrderByDescending(x => x.Id)
                            .Select(x => new AuditLogVM
                            {
                                CourtName = (x.Court != null) ? x.Court.Label : "Всички съдилища",
                                UserFullName = x.User.FullName,
                                DateWrt = x.DateWrt,
                                Operation = x.Operation,
                                Object = x.ObjectInfo
                            }).AsQueryable();
        }
    }
}

