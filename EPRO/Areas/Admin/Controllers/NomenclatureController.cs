using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using EPRO.Core.Constants;
using EPRO.Core.Contracts;
using EPRO.Core.Models.Nomenclatures;
using EPRO.Extensions;
using EPRO.Infrastructure.Constants;
using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Models.Nomenclatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPRO.Areas.Admin.Controllers
{
    /// <summary>
    /// Управление на номенклатури от общ вид
    /// </summary>
    public class NomenclatureController : BaseController
    {
        /// <summary>
        /// Работа с номенклатури
        /// </summary>
        private readonly INomenclatureService nomenclatureService;

        /// <summary>
        /// Текущ тип на номенклатура
        /// </summary>
        private Type nomenclatureType;

        public NomenclatureController(INomenclatureService _nomenclatureService)
        {
            nomenclatureService = _nomenclatureService;
        }

        /// <summary>
        /// Списък с елементи на номенклатурата
        /// </summary>
        /// <param name="nomenclatureName">Име на типа на номенклатурата</param>
        /// <returns></returns>
        public IActionResult Index(string nomenclatureName)
        {
            if (nomenclatureName == null)
            {
                return NotFound();
            }

            nomenclatureType = Type.GetType(String.Format(NomenclatureConstants.AssemblyQualifiedName, nomenclatureName), false);

            if (nomenclatureType == null)
            {
                TempData.Remove("NomenclatureName");

                return BadRequest(String.Format("Номенклатурата не е намерена ({0})", nomenclatureName));
            }

            TempData["NomenclatureName"] = nomenclatureName;
            TempData["Title"] = ((DisplayAttribute)nomenclatureType
                .GetCustomAttributes(typeof(DisplayAttribute), true)
                .SingleOrDefault())
                ?.Name ?? nomenclatureName;

            return View();
        }

        /// <summary>
        /// Метод за достъп на DataTables до данните от номенклатурата
        /// </summary>
        /// <param name="request">Заявка на DataTables</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult NomenclatureListData(IDataTablesRequest request, bool onlyActive = true)
        {
            string methodName = onlyActive ? "GetActiveList" : "GetList";
            var method = nomenclatureService.GetType().GetMethod(methodName);
            var generic = method.MakeGenericMethod(nomenclatureType);

            IQueryable<CommonNomenclatureListItem> data = (IQueryable<CommonNomenclatureListItem>)generic
                .Invoke(nomenclatureService, null);
            var response = request.GetResponse(data); //second parameter is for filtered set request.GetResponse(data. filteredContext)
            return new DataTablesJsonResult(response, true);
        }

        /// <summary>
        /// Добавяне на нов елемент
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            var test = nomenclatureType as ICommonNomenclature;
            if (test == null)
            {
                var modeltype = (IBaseNomenclature)Activator.CreateInstance(nomenclatureType);

                GetDropDownLists();

                return View("Edit", modeltype);
            }
            else
            {
                var model = (ICommonNomenclature)Activator.CreateInstance(nomenclatureType);

                GetDropDownLists();

                return View("Edit", model);
            }
        }

        /// <summary>
        /// Редактиране на елемент
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var method = nomenclatureService.GetType().GetMethod("GetItem");
            var generic = method.MakeGenericMethod(nomenclatureType);

            var model = Convert.ChangeType(generic.Invoke(nomenclatureService, new object[] { id }), nomenclatureType);
            GetDropDownLists();

            return View(model);
        }

        /// <summary>
        /// Запис на промените в елемент
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit()
        {
            var model = Activator.CreateInstance(nomenclatureType);

            if (!await TryUpdateModelAsync(model, nomenclatureType, String.Empty))
            {
                TempData[MessageConstant.ErrorMessage] = MessageConstant.Values.BindError;

                return RedirectToAction(nameof(Index));
            }

            var method = nomenclatureService.GetType().GetMethod("SaveItem");
            var generic = method.MakeGenericMethod(nomenclatureType);

            var nomInstance = (BaseCommonNomenclature)model;
            bool isUpdate = nomInstance.Id > 0;
            bool result = (bool)generic.Invoke(nomenclatureService, new object[] { model });

            if (result)
            {
                
                TempData[MessageConstant.SuccessMessage] = MessageConstant.Values.SaveOK;
            }
            else
            {
                TempData[MessageConstant.ErrorMessage] = MessageConstant.Values.SaveFailed;
            }

            GetDropDownLists();

            return View(model);
        }

        /// <summary>
        /// Промяна на подредбата на елементите
        /// </summary>
        /// <param name="orderArray"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ChangeOrder(ChangeOrderModel model)
        {
            var method = nomenclatureService.GetType().GetMethod("ChangeOrder");
            var generic = method.MakeGenericMethod(nomenclatureType);

            bool result = (bool)generic.Invoke(nomenclatureService, new object[] { model });

            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstant.Values.SaveFailed);
            }

            return Ok();
        }


        /// <summary>
        /// Действия, изпълнявани след изпълнението на всеки Action
        /// -- Управлява менюто, избира кое да е отворено
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            string menuItem = (string)TempData.Peek("NomenclatureName");

            if (menuItem != null)
            {
                ViewBag.MenuItemValue = menuItem;
            }

            base.OnActionExecuted(context);
        }

        /// <summary>
        /// Действия, изпълнявани преди изпълнението на всеки Action
        /// -- Избира типа на текущата номенклатура
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string nomenclatureName = (string)TempData.Peek("NomenclatureName");

            if (nomenclatureName != null)
            {
                nomenclatureType = Type.GetType(String.Format(NomenclatureConstants.AssemblyQualifiedName, nomenclatureName), false);

                if (nomenclatureType == null)
                {
                    filterContext.Result = BadRequest(String.Format("Номенклатурата не е намерена ({0})", nomenclatureName));
                    TempData.Remove("NomenclatureName");
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private void GetDropDownLists()
        {
            var fkProperties = nomenclatureType.GetProperties()
                .Where(p => p.CustomAttributes
                .Any(a => a.AttributeType == typeof(ForeignKeyAttribute)));

            foreach (var property in fkProperties)
            {
                var attrConstructorValue = property.CustomAttributes
                    .FirstOrDefault(a => a.AttributeType == typeof(ForeignKeyAttribute))
                    ?.ConstructorArguments
                    ?.FirstOrDefault();

                if (attrConstructorValue != null && attrConstructorValue.HasValue)
                {
                    string fkPropertyName = attrConstructorValue.Value.Value.ToString();

                    var method = nomenclatureService.GetType().GetMethod("GetDropDownList");
                    var generic = method.MakeGenericMethod(property.PropertyType);

                    ViewData[fkPropertyName + "_ddl"] = Convert.ChangeType(generic.Invoke(nomenclatureService, new object[] { true, false, false }), typeof(List<SelectListItem>));
                }
            }
        }

        /// <summary>
        /// Извлича списък на съдилища по даден апелативенн регион
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public IActionResult Get_CourtsByRegion(int regionId)
        {
            var model = nomenclatureService.GetCourtsByRegion(regionId).
                Select(x => new { id = x.Value, text = x.Text });
            return Json(model); 
            //return Json(new { results = model, more = false }); ;
        }
    }
}