using EPRO.Core.Models.Nomenclatures;
using EPRO.Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPRO.Core.Contracts
{
    /// <summary>
    /// Общи номенклатури
    /// </summary>
    public interface INomenclatureService
    {
        /// <summary>
        /// Списък за показване в таблица, всички елементи
        /// </summary>
        /// <typeparam name="T">Тип на номенклатурата</typeparam>
        /// <returns></returns>
        IQueryable<CommonNomenclatureListItem> GetList<T>() where T : class, ICommonNomenclature;

        /// <summary>
        /// ЛСписък за показване в таблица, само активни елементи
        /// </summary>
        /// <typeparam name="T">Тип на номенклатурата</typeparam>
        /// <returns></returns>
        IQueryable<CommonNomenclatureListItem> GetActiveList<T>() where T : class, ICommonNomenclature;

        /// <summary>
        /// Конкретен елемент от номенклатура
        /// </summary>
        /// <typeparam name="T">Тип на номенклатурата</typeparam>
        /// <param name="id">Идентификатор на запис</param>
        /// <returns></returns>
        Task<T> GetItem<T>(int id) where T : class, ICommonNomenclature;

        /// <summary>
        /// Запис на елемент от номенклатура
        /// </summary>
        /// <typeparam name="T">Тип на номенклатурата</typeparam>
        /// <param name="entity">Елемент за запис</param>
        /// <returns></returns>
        Task<bool> SaveItem<T>(T entity) where T : class, ICommonNomenclature;

        /// <summary>
        /// Промяна на подредбата с бутони
        /// </summary>
        /// <typeparam name="T">Тип на номенклатурата</typeparam>
        /// <param name="model">Посока и идентификатор на запис</param>
        /// <returns></returns>
        bool ChangeOrder<T>(ChangeOrderModel model) where T : class, ICommonNomenclature;

        /// <summary>
        /// Генерира списък от елементи на номенклатура за комбо
        /// </summary>
        /// <typeparam name="T">Тип на номенклатурата</typeparam>
        /// <param name="addDefaultElement">Дали да добави елемент "Изберете"
        /// по подразбиране е изтина</param>
        /// <returns></returns>
        List<SelectListItem> GetDropDownList<T>(bool addDefaultElement = true, bool addAllElement = false, bool orderByNumber = false) where T : class, ICommonNomenclature;
        List<SelectListItem> GetDropDownList<T>(int id) where T : class, ICommonNomenclature;

        /// <summary>
        /// Списък на съдилища по апелативен район
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        List<SelectListItem> GetCourtsByRegion(int regionId);

        /// <summary>
        /// Връща етикет на обекта от обща номенклатура по подаден идентификатор
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetLabelById<T>(int id) where T : class, ICommonNomenclature;

        /// <summary>
        /// Валидация на формат номер на дело
        /// </summary>
        /// <param name="caseNumber"></param>
        /// <param name="courtId"></param>
        /// <param name="caseYear"></param>
        /// <returns></returns>
        bool ValidateCaseNumber(string caseNumber, int courtId, int caseYear = 0);
    }
}
