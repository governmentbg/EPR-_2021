using EPRO.Core.Models;
using EPRO.Core.Models.FilterModels;
using EPRO.Infrastructure.Data.Models.Common;
using EPRO.Infrastructure.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPRO.Core.Contracts
{
    /// <summary>
    /// Услуга за управление на отводи
    /// </summary>
    public interface IDismissalService : IBaseService
    {
        /// <summary>
        /// Данни за отводи - оперативен регистър
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IQueryable<DismissalListVM> Select(DismissalFilterVM filter);

        /// <summary>
        /// Данни за отводи - справка за отводи
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IQueryable<DismissalListVM> SelectReport(ReportFilterVM filter);

        /// <summary>
        /// Запис на данни за отвод
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SaveResultVM> SaveDataAsync(Dismissal model);

        /// <summary>
        /// Връща падащо меню с години за подадени отводи
        /// </summary>
        /// <param name="forEdit"></param>
        /// <returns></returns>
        Task<List<SelectListItem>> Get_DDLYearsAsync(bool forEdit = false);

        /// <summary>
        /// Анулиране на данни за отвод
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SaveResultVM> ExpireAsync(DismissalManageVM model);

        /// <summary>
        /// Възстановяване на данни за отвод
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SaveResultVM> RecoverAsync(DismissalManageVM model);
    }
}
