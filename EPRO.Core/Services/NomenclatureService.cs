using EPRO.Core.Contracts;
using EPRO.Core.Extensions;
using EPRO.Core.Models.Nomenclatures;
using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Common;
using EPRO.Infrastructure.Data.Models.Nomenclatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPRO.Core.Services
{
    public class NomenclatureService : INomenclatureService
    {
        private readonly ILogger logger;

        private readonly IRepository repo;

        public NomenclatureService(
            ILogger<NomenclatureService> _logger,
            IRepository _repo)
        {
            logger = _logger;
            repo = _repo;
        }



        public bool ChangeOrder<T>(ChangeOrderModel model) where T : class, ICommonNomenclature
        {
            bool result = false;

            try
            {
                var nomList = repo.All<T>()
                    .ToList();

                int maxOrderNumber = nomList
                    .Max(x => x.OrderNumber);
                int minOrderNumber = nomList
                    .Min(x => x.OrderNumber);
                var currentElement = nomList
                    .Where(x => x.Id == model.Id)
                    .FirstOrDefault();

                if (currentElement != null)
                {
                    if (model.Direction == "up" && currentElement.OrderNumber > minOrderNumber)
                    {
                        var previousElement = nomList
                            .Where(x => x.OrderNumber == currentElement.OrderNumber - 1)
                            .FirstOrDefault();

                        if (previousElement != null)
                        {
                            previousElement.OrderNumber = currentElement.OrderNumber;
                        }

                        currentElement.OrderNumber -= 1;
                    }

                    if (model.Direction == "down" && currentElement.OrderNumber < maxOrderNumber)
                    {
                        var nextElement = nomList
                            .Where(x => x.OrderNumber == currentElement.OrderNumber + 1)
                            .FirstOrDefault();

                        if (nextElement != null)
                        {
                            nextElement.OrderNumber = currentElement.OrderNumber;
                        }

                        currentElement.OrderNumber += 1;
                    }

                    repo.SaveChanges();
                }

                result = true;
            }
            catch (Exception ex)
            {
                logger.LogError("ChangeOrder", ex);
            }

            return result;
        }

        public List<SelectListItem> GetDropDownList<T>(bool addDefaultElement, bool addAllElement, bool orderByNumber) where T : class, ICommonNomenclature
        {
            var result = repo.AllReadonly<T>()
                        .ToSelectList(addDefaultElement, addAllElement, orderByNumber);

            return result;
        }

        public List<SelectListItem> GetDropDownList<T>(int id) where T : class, ICommonNomenclature
        {
            var result = repo.AllReadonly<T>()
                                .Where(x => x.Id == id)
                       .ToSelectList(false, false, false);

            return result;
        }

        public async Task<T> GetItem<T>(int id) where T : class, ICommonNomenclature
        {
            var item = await repo.GetByIdAsync<T>(id);

            return item;
        }

        public IQueryable<CommonNomenclatureListItem> GetList<T>() where T : class, ICommonNomenclature
        {
            return repo.AllReadonly<T>()
                .Select(x => new CommonNomenclatureListItem()
                {
                    Id = x.Id,
                    Code = x.Code,
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    Label = x.Label,
                    OrderNumber = x.OrderNumber
                }).OrderBy(x => x.OrderNumber);
        }

        public IQueryable<CommonNomenclatureListItem> GetActiveList<T>() where T : class, ICommonNomenclature
        {
            DateTime now = DateTime.Today;
            return repo.AllReadonly<T>()
                .Where(n => n.IsActive && n.DateStart <= now)
                .Where(n => n.DateEnd == null || n.DateEnd >= now)
                .Select(x => new CommonNomenclatureListItem()
                {
                    Id = x.Id,
                    Code = x.Code,
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    Label = x.Label,
                    OrderNumber = x.OrderNumber
                }).OrderBy(x => x.OrderNumber);
        }

        public async Task<bool> SaveItem<T>(T entity) where T : class, ICommonNomenclature
        {
            bool result = false;

            try
            {
                if (entity.Id > 0)
                {
                    repo.Update(entity);
                }
                else
                {
                    int maxOrderNumber = repo.AllReadonly<T>()
                        .Select(x => x.OrderNumber)
                        .OrderByDescending(x => x)
                        .FirstOrDefault();

                    entity.OrderNumber = maxOrderNumber + 1;
                    await repo.AddAsync(entity);
                }

                repo.SaveChanges();

                result = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Грешка при запис на номенклатура ({ typeof(T).ToString() })");
            }

            return result;
        }

        public List<SelectListItem> GetCourtsByRegion(int regionId)
        {
            return repo.AllReadonly<Court>()
                         .Where(x => x.ApealRegionId == regionId || regionId <= 0)
                         .OrderBy(x => x.Label)
                         .ToSelectList();
        }

        public string GetLabelById<T>(int id) where T : class, ICommonNomenclature
        {
            return repo.AllReadonly<T>()
                            .Where(x => x.Id == id)
                            .Select(x => x.Label)
                            .FirstOrDefault();
        }

        public bool ValidateCaseNumber(string caseNumber, int courtId, int caseYear = 0)
        {
            if (string.IsNullOrEmpty(caseNumber))
            {
                return false;
            }

            if (caseNumber.Length != 14)
            {
                return false;
            }

            if (!caseNumber.All(Char.IsDigit))
            {
                return false;
            }

            if (caseYear > 0)
            {
                int numYear = int.Parse(caseNumber.Substring(0, 4));
                if (numYear != caseYear)
                {
                    return false;
                }
            }

            string courtCode = caseNumber.Substring(4, 3);
            string currentCoutCode = repo.AllReadonly<Court>()
                                            .Where(x => x.Id == courtId)
                                            .Select(x => x.Code)
                                            .FirstOrDefault();

            if (currentCoutCode != courtCode)
            {
                return false;
            }

            return true;
        }
    }
}
