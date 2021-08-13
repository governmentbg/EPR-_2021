using EPRO.Core.Contracts;
using EPRO.Core.Extensions;
using EPRO.Core.Models;
using EPRO.Core.Models.FilterModels;
using EPRO.Infrastructure.Constants;
using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Common;
using EPRO.Infrastructure.Data.Models.Common;
using EPRO.Infrastructure.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPRO.Core.Services
{
    public class DismissalService : BaseService, IDismissalService
    {
        public DismissalService(
            IRepository _repo,
            ILogger<DismissalService> _logger,
            IUserContext _userContext)
        {
            repo = _repo;
            logger = _logger;
            userContext = _userContext;
        }

        public async Task<SaveResultVM> ExpireAsync(DismissalManageVM model)
        {
            try
            {
                var saved = await repo.GetByIdAsync<Dismissal>(model.DismissalId);
                if (saved == null)
                {
                    return new SaveResultVM(false, "Няма такъв отвод.");
                }

                if (saved.DateExpired != null)
                {
                    return new SaveResultVM(false, "Отвода вече е анулиран.");
                }

                saved.DateExpired = DateTime.Now;
                saved.UserExpiredId = userContext.UserId;
                saved.DescriptionExpired = model.Reason;
                repo.Update(saved);
                repo.SaveChanges();
                return new SaveResultVM(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Expire");
                return new SaveResultVM(false);
            }
        }

        public async Task<List<SelectListItem>> Get_DDLYearsAsync(bool forEdit = false)
        {
            if (forEdit)
            {
                var startYear = 2010;
                return Enumerable.Range(startYear, DateTime.Now.Year - startYear + 1).ToList()
                             .OrderByDescending(x => x)
                             .Select(x => new SelectListItem(x.ToString(), x.ToString()))
                             .ToList()
                             .Prepend(new SelectListItem() { Text = "Изберете", Value = null })
                             .ToList();
            }
            else
            {
                return (await repo.AllReadonly<Dismissal>()
                             .Select(x => x.CaseYear)
                             .Distinct()
                             .OrderByDescending(x => x)
                             .ToListAsync())
                             .Select(x => new SelectListItem(x.ToString(), x.ToString()))
                             .ToList()
                             .Prepend(new SelectListItem() { Text = "Всички", Value = null })
                             .ToList();
            }
        }

        public async Task<SaveResultVM> RecoverAsync(DismissalManageVM model)
        {
            try
            {
                var saved = await repo.GetByIdAsync<Dismissal>(model.DismissalId);
                if (saved == null)
                {
                    return new SaveResultVM(false, "Няма такъв отвод.");
                }

                if (saved.DateExpired == null)
                {
                    return new SaveResultVM(false, "Отвода не е анулиран.");
                }

                saved.DateExpired = null;
                saved.UserExpiredId = null;
                saved.DescriptionExpired = model.Reason;
                repo.Update(saved);
                repo.SaveChanges();
                return new SaveResultVM(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Recover");
                return new SaveResultVM(false);
            }
        }

        public async Task<SaveResultVM> SaveDataAsync(Dismissal model)
        {
            try
            {
                model.UserId = userContext.UserId;
                model.DateWrt = DateTime.Now;

                model.CaseYear = int.Parse(model.CaseNumber.Substring(0, 4));

                if (!string.IsNullOrEmpty(model.Id))
                {
                    var saved = await repo.GetByIdAsync<Dismissal>(model.Id);
                    repo.Detach(saved);

                    model.CourtId = saved.CourtId;
                    model.EntryType = saved.EntryType;

                    repo.Update(model);
                }
                else
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.EntryType = NomenclatureConstants.EntryType.User;
                    await repo.AddAsync(model);
                }
                await repo.SaveChangesAsync();

                var auditInfo = await repo.AllReadonly<Dismissal>()
                       .Include(x => x.Court)
                       .Include(x => x.CaseType)
                       .Include(x => x.DismissalType)
                       .Where(x => x.Id == model.Id)
                       .Select(x => $"{x.DismissalType.Label} на {x.JudgeFullName} по дело {x.CaseType.Label} {x.CaseNumber}/{x.CaseYear} към {x.Court.Label}")
                       .FirstOrDefaultAsync();


                return new SaveResultVM()
                {
                    Result = true,
                    AuditInfo = auditInfo
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SaveData");
                return new SaveResultVM(false);
            }
        }


        public IQueryable<DismissalListVM> Select(DismissalFilterVM filter)
        {
            filter.UpdateNullables();
            if (userContext.CourtId > 0)
            {
                filter.CourtId = userContext.CourtId;
            }
            Expression<Func<Dismissal, bool>> wherePeriod = x => true;
            if (filter.PeriodFrom.HasValue || filter.PeriodTo.HasValue)
            {
                wherePeriod = x => x.HearingDate >= (filter.PeriodFrom ?? DateTime.MinValue) &&
                x.HearingDate <= (filter.PeriodTo.MakeEndDate() ?? DateTime.MaxValue);
            }

            Expression<Func<Dismissal, bool>> whereActive = x => true;
            if (filter.ActiveOnly)
            {
                whereActive = x => x.DateExpired == null;
            }
            return repo.AllReadonly<Dismissal>()
                        .Include(x => x.Court)
                        .Include(x => x.CaseType)
                        .Where(x => x.EntryType == (filter.EntryType ?? x.EntryType))
                        .Where(x => x.CourtId == (filter.CourtId ?? x.CourtId))
                        .Where(x => x.CaseTypeId == (filter.CaseTypeId ?? x.CaseTypeId))
                        .Where(x => x.DismissalTypeId == (filter.DismissalTypeId ?? x.DismissalTypeId))
                        .Where(x => EF.Functions.ILike(x.JudgeFullName, filter.JudgeName.ToPaternSearch()))
                        .Where(x => EF.Functions.ILike(x.CaseNumber, filter.CaseNumber.ToCasePaternSearch()))
                        .Where(x => x.CaseYear == (filter.CaseYear ?? x.CaseYear))
                        .Where(x => EF.Functions.ILike((x.DismissalReason ?? ""), filter.DismissalReason.ToPaternSearch()))
                        .Where(wherePeriod)
                        .Where(whereActive)
                        .Select(x => new DismissalListVM
                        {
                            Id = x.Id,
                            EntryType = x.EntryType,
                            CourtName = x.Court.Label,
                            CaseTypeName = x.CaseType.Label,
                            DismissalTypeName = x.DismissalType.Label,
                            JudgeName = x.JudgeFullName,
                            ReplaceJudgeName = x.ReplaceJudgeFullName,
                            CaseNumber = x.CaseNumber,
                            CaseYear = x.CaseYear,
                            HearingDate = x.HearingDate
                        })
                        .OrderByDescending(x => x.HearingDate)
                        .AsQueryable();
        }

        public IQueryable<DismissalListVM> SelectReport(ReportFilterVM filter)
        {
            filter.UpdateNullables();
            Expression<Func<Dismissal, bool>> wherePeriod = x => true;
            if (filter.PeriodFrom.HasValue || filter.PeriodTo.HasValue)
            {
                wherePeriod = x => x.HearingDate >= (filter.PeriodFrom ?? DateTime.MinValue) &&
                x.HearingDate <= (filter.PeriodTo.MakeEndDate() ?? DateTime.MaxValue);
            }
            Expression<Func<Dismissal, bool>> whereCourts = x => true;
            if (!string.IsNullOrEmpty(filter.CourtIds))
            {
                whereCourts = x => filter.CourtIds.SplitToInts().Contains(x.CourtId);
            }
            else
            {
                if (filter.ApealRegionId > 0)
                {
                    whereCourts = x => x.Court.ApealRegionId == filter.ApealRegionId;
                }
            }
            if (userContext.CourtId > 0)
            {
                whereCourts = x => x.CourtId == userContext.CourtId;
            }
            Expression<Func<Dismissal, bool>> whereCaseTypes = x => true;
            if (!string.IsNullOrEmpty(filter.CaseTypeIds))
            {
                whereCaseTypes = x => filter.CaseTypeIds.SplitToInts().Contains(x.CaseTypeId);
            }
            return repo.AllReadonly<Dismissal>()
                        .Include(x => x.Court)
                        .ThenInclude(x => x.ApealRegion)
                        .Include(x => x.CaseType)
                        .Where(x => x.EntryType == (filter.EntryType ?? x.EntryType))
                        .Where(whereCourts)
                        .Where(whereCaseTypes)
                        .Where(x => x.DismissalTypeId == (filter.DismissalTypeId ?? x.DismissalTypeId))
                        .Where(x => EF.Functions.ILike(x.JudgeFullName, filter.JudgeName.ToPaternSearch()))
                        .Where(x => EF.Functions.ILike(x.CaseNumber, filter.CaseNumber.ToCasePaternSearch()))
                        .Where(x => x.CaseYear == (filter.CaseYear ?? x.CaseYear))
                        .Where(x => EF.Functions.ILike((x.DismissalReason ?? ""), filter.DismissalReason.ToPaternSearch()))
                        .Where(x => x.CaseRoleId == (filter.CaseRoleId ?? x.CaseRoleId))
                        .Where(x => x.HearingTypeId == (filter.HearingTypeId ?? x.HearingTypeId))
                        .Where(x => x.ActTypeId == (filter.ActTypeId ?? x.ActTypeId))
                        .Where(wherePeriod)
                        .Where(x => x.DateExpired == null)
                        .Select(x => new DismissalListVM
                        {
                            Id = x.Id,
                            EntryType = x.EntryType,
                            CourtName = x.Court.Label,
                            RegionName = x.Court.ApealRegion.Label,
                            CaseTypeName = x.CaseType.Label,
                            DismissalTypeName = x.DismissalType.Label,
                            JudgeName = x.JudgeFullName,
                            ReplaceJudgeName = x.ReplaceJudgeFullName,
                            CaseNumber = x.CaseNumber,
                            CaseYear = x.CaseYear,
                            HearingDate = x.HearingDate
                        })
                        .OrderByDescending(x => x.HearingDate)
                        .AsQueryable();
        }
    }
}
