using EPRO.Components;
using EPRO.Core.Contracts;
using EPRO.Extensions;
using EPRO.Infrastructure.Constants;
using EPRO.Infrastructure.Data.Models.Common;
using EPRO.Infrastructure.ViewModels.Common;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EPRO.Infrastructure.Data.Models.Nomenclatures;
using EPRO.Core.Models.FilterModels;
using EPRO.Core.Models;
using EPRO.Infrastructure.Contracts;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;

namespace EPRO.Areas.Admin.Controllers
{
    /// <summary>
    /// Административен панел - Управление на отводи
    /// </summary>
    public class DismissalController : BaseController
    {
        private readonly IDismissalService dismissalService;
        private readonly INomenclatureService nomService;
        private readonly ICdnService cdnService;

        private bool CanChange
        {
            get
            {
                return userContext.IsUserInRole(AccountConstants.Roles.EDIT);
            }
        }

        public DismissalController(
            IDismissalService _dismissalService,
            INomenclatureService _nomService,
            ICdnService _cdnService)
        {
            dismissalService = _dismissalService;
            nomService = _nomService;
            cdnService = _cdnService;
        }

        /// <summary>
        /// Административен панел - Регистър на отводите
        /// </summary>
        /// <returns></returns>
        [MenuItem("dismissal")]
        public async Task<IActionResult> Index()
        {
            var model = new DismissalFilterVM();
            if (userContext.CourtId > 0)
            {
                ViewBag.CourtId_ddl = nomService.GetDropDownList<Court>(userContext.CourtId.Value);
                model.CourtId = userContext.CourtId;
            }
            else
            {
                ViewBag.CourtId_ddl = nomService.GetDropDownList<Court>(false, true);
            }
            ViewBag.DismissalTypeId_ddl = nomService.GetDropDownList<DismissalType>(false, true);
            ViewBag.CaseTypeId_ddl = nomService.GetDropDownList<CaseType>(false, true);
            ViewBag.CaseYear_ddl = await dismissalService.Get_DDLYearsAsync();
            ViewBag.canChange = this.CanChange;

            return View(model);
        }

        [HttpPost]
        public IActionResult ListData(IDataTablesRequest request, DismissalFilterVM filter)
        {
            var data = dismissalService.Select(filter);
            return new DataTablesJsonResult(request.GetResponse(data), true);
        }

        public IActionResult Add()
        {
            if (!CanChange)
            {
                return RedirectToAction(nameof(HomeController.Denied), nameof(HomeController));
            }
            var model = new Dismissal();
            if (userContext.CourtId > 0)
            {
                model.CourtId = userContext.CourtId.Value;
            }
            SetViewbag(model);
            return View(nameof(Edit), model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (!CanChange)
            {
                return RedirectToAction(nameof(HomeController.Denied), nameof(HomeController));
            }
            var model = await dismissalService.GetById<Dismissal>(id);
            if (userContext.CourtId > 0 && model.CourtId != userContext.CourtId)
            {
                return RedirectToAction(nameof(HomeController.Denied), nameof(HomeController));
            }
            if (model.EntryType != NomenclatureConstants.EntryType.User)
            {
                return RedirectToAction(nameof(View), new { id });
            }
            SetViewbag(model);
            return View(nameof(Edit), model);
        }

        public async Task<IActionResult> View(string id)
        {
            var model = await dismissalService.GetById<Dismissal>(id);
            if (userContext.CourtId > 0 && model.CourtId != userContext.CourtId)
            {
                return RedirectToAction(nameof(HomeController.Denied), nameof(HomeController));
            }
            SetViewbag(model);
            return View(nameof(View), model);
        }

        /// <summary>
        /// Изтегляне на отвод като pdf документ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Print(string id)
        {
            DismissalPrintVM model = new DismissalPrintVM()
            {
                Data = await dismissalService.GetById<Dismissal>(id)
            };

            model.CourtName = nomService.GetLabelById<Court>(model.Data.CourtId);
            model.CaseTypeName = nomService.GetLabelById<CaseType>(model.Data.CaseTypeId);
            model.DismissalTypeName = nomService.GetLabelById<DismissalType>(model.Data.DismissalTypeId);
            model.HearingTypeName = nomService.GetLabelById<HearingType>(model.Data.HearingTypeId);
            model.ActTypeName = nomService.GetLabelById<ActType>(model.Data.ActTypeId);
            model.CaseRoleName = nomService.GetLabelById<CaseRole>(model.Data.CaseRoleId);
            var file = cdnService.Select(NomenclatureConstants.SourceType.Dismissal, id).Where(x => x.DateExpired == null).FirstOrDefault();
            if (file != null)
            {
                model.ActFileName = file.Title;
            }
            var pdfBytes = await new ViewAsPdfByteWriter("Print", model, false).GetByte(this.ControllerContext);

            return File(pdfBytes, "application/pdf", "dismissalPreview.pdf");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Dismissal model)
        {
            if (!CanChange)
            {
                return RedirectToAction(nameof(Index));
            }
            if (userContext.CourtId > 0)
            {
                model.CourtId = userContext.CourtId.Value;
            }
            ValidateDismissal(model);
            if (!ModelState.IsValid)
            {
                SetViewbag(model);
                return View(nameof(Edit), model);
            }
            string currentId = model.Id;
            var result = await dismissalService.SaveDataAsync(model);
            if (result.Result)
            {
                if (string.IsNullOrEmpty(currentId))
                {
                    Audit_Operation = NomenclatureConstants.AuditOperations.Add;
                }
                else
                {
                    Audit_Operation = NomenclatureConstants.AuditOperations.Edit;
                }
                Audit_Object = result.AuditInfo;
                Audit_CourtId = model.CourtId;
                SetSuccessMessage();
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            }
            else
            {
                SetErrorMessage();
                SetViewbag(model);
                return View(model);
            }
        }

        private void SetViewbag(Dismissal model, bool forPrint = false)
        {
            if (userContext.CourtId > 0)
            {
                ViewBag.CourtId_ddl = nomService.GetDropDownList<Court>(userContext.CourtId.Value);
            }
            else
            {
                ViewBag.CourtId_ddl = nomService.GetDropDownList<Court>();
            }
            if (forPrint)
            {
                ViewBag.CourtId_ddl = nomService.GetDropDownList<Court>(model.CourtId);
            }
            ViewBag.DismissalTypeId_ddl = nomService.GetDropDownList<DismissalType>();
            ViewBag.CaseTypeId_ddl = nomService.GetDropDownList<CaseType>();
            ViewBag.CaseRoleId_ddl = nomService.GetDropDownList<CaseRole>();
            ViewBag.HearingTypeId_ddl = nomService.GetDropDownList<HearingType>();
            ViewBag.ActTypeId_ddl = nomService.GetDropDownList<ActType>();
            ViewBag.canChange = model.DateExpired == null;
            ViewBag.canRecover = userContext.IsUserInRole(AccountConstants.Roles.GLOBAL_ADMIN);
            if (forPrint && !string.IsNullOrEmpty(model.Id))
            {
                var file = cdnService.Select(NomenclatureConstants.SourceType.Dismissal, model.Id).Where(x => x.DateExpired == null).FirstOrDefault();
                if (file != null)
                {
                    ViewBag.actFile = file;
                }
            }
        }

        /// <summary>
        /// Валидиране данните за отвод
        /// </summary>
        /// <param name="model"></param>
        private void ValidateDismissal(Dismissal model)
        {
            if (string.IsNullOrEmpty(model.CaseNumber))
            {
                ModelState.AddModelError(nameof(Dismissal.CaseNumber), "Въведете 'Дело номер'.");
            }
            else
            {
                if (model.CaseNumber.Length != 14)
                {
                    ModelState.AddModelError(nameof(Dismissal.CaseNumber), "Невалиден номер на дело");
                }
                long _res = 0;
                if (!long.TryParse(model.CaseNumber, out _res))
                {
                    ModelState.AddModelError(nameof(Dismissal.CaseNumber), "Невалиден номер на дело");
                }
                if (!nomService.ValidateCaseNumber(model.CaseNumber, model.CourtId))
                {
                    ModelState.AddModelError(nameof(Dismissal.CaseNumber), "Невалиден номер на дело");
                }

                int _caseYear;
                if (int.TryParse(model.CaseNumber.Substring(0, 4), out _caseYear))
                {
                    if (_caseYear > DateTime.Now.Year || _caseYear < DateTime.Now.AddYears(-5).Year)
                    {
                        ModelState.AddModelError(nameof(Dismissal.CaseNumber), "Невалидна година на делото");
                    }
                }
            }
            if (model.HearingDate >= DateTime.Now.Date.AddDays(1))
            {
                ModelState.AddModelError(nameof(Dismissal.HearingDate), "Невалиден дата на провеждане");
            }
            if (model.DismissalTypeId == NomenclatureConstants.DismissalTypes.Otvod)
            {
                if ((model.DocumentNumber ?? 0) <= 0)
                {
                    ModelState.AddModelError(nameof(Dismissal.DocumentNumber), "Въведете 'Номер на документ'.");
                }
                if (model.DocumentDate == null)
                {
                    ModelState.AddModelError(nameof(Dismissal.DocumentDate), "Въведете 'Дата на документ'.");
                }
                if (string.IsNullOrEmpty(model.DocumentType))
                {
                    ModelState.AddModelError(nameof(Dismissal.DocumentType), "Въведете 'Вид входящ документ'.");
                }
                if (string.IsNullOrEmpty(model.SideFullName))
                {
                    ModelState.AddModelError(nameof(Dismissal.SideFullName), "Въведете 'Имена'.");
                }
                if (string.IsNullOrEmpty(model.SideInvolmentKind))
                {
                    ModelState.AddModelError(nameof(Dismissal.SideInvolmentKind), "Въведете 'Качество на страната'.");
                }
            }
            if (model.EntryType == NomenclatureConstants.EntryType.API)
            {
                if (string.IsNullOrEmpty(model.EditMotives))
                {
                    ModelState.AddModelError(nameof(Dismissal.EditMotives), "Въведете 'Причини за редактиране на данни за отвод'.");
                }
            }
        }

        /// <summary>
        /// Анулиране на отвод
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Expire(string id)
        {
            var model = new DismissalManageVM()
            {
                DismissalId = id
            };
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Expire(DismissalManageVM model)
        {
            var res = await dismissalService.ExpireAsync(model);
            if (res.Result)
            {
                var _del = await dismissalService.GetById<Dismissal>(model.DismissalId);
                var _dt = await dismissalService.GetById<DismissalType>(_del.DismissalTypeId);
                Audit_Operation = NomenclatureConstants.AuditOperations.Delete;
                Audit_Object = $"Изтриване на {_dt.Label}; Дело: {_del.CaseNumber}; Съдия: {_del.JudgeFullName}";
                Audit_CourtId = _del.CourtId;
            }
            return Json(res);
        }

        /// <summary>
        /// Възстановяване на отвод
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Recover(string id)
        {
            var model = new DismissalManageVM()
            {
                DismissalId = id
            };
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Recover(DismissalManageVM model)
        {
            var res = await dismissalService.RecoverAsync(model);
            if (res.Result)
            {
                var _del = await dismissalService.GetById<Dismissal>(model.DismissalId);
                var _dt = await dismissalService.GetById<DismissalType>(_del.DismissalTypeId);
                Audit_Operation = NomenclatureConstants.AuditOperations.Patch;
                Audit_Object = $"Възстановяване на {_dt.Label}; Дело: {_del.CaseNumber}; Съдия: {_del.JudgeFullName}";
                Audit_CourtId = _del.CourtId;
            }
            return Json(res);
        }

        /// <summary>
        /// Справка за постъпили отводи
        /// </summary>
        /// <returns></returns>
        [MenuItem("report")]
        public async Task<IActionResult> Report()
        {
            var model = new ReportFilterVM();
            ViewBag.ApealRegionId_ddl = nomService.GetDropDownList<ApealRegion>(false, true);
            //ViewBag.CourtIds_ddl = nomService.GetDropDownList<Court>(false, true);
            ViewBag.DismissalTypeId_ddl = nomService.GetDropDownList<DismissalType>(false, true);
            ViewBag.CaseTypeIds_ddl = nomService.GetDropDownList<CaseType>(false, true);
            ViewBag.CaseYear_ddl = await dismissalService.Get_DDLYearsAsync();
            ViewBag.HearingTypeId_ddl = nomService.GetDropDownList<HearingType>(false, true);
            ViewBag.CaseRoleId_ddl = nomService.GetDropDownList<CaseRole>(false, true);
            ViewBag.ActTypeId_ddl = nomService.GetDropDownList<ActType>(false, true);
            ViewBag.canChange = this.CanChange;
            ViewBag.viewAllCourts = userContext.CourtId == null || userContext.IsUserInRole(AccountConstants.Roles.GLOBAL_ADMIN);

            return View(model);
        }

        [HttpPost]
        public IActionResult Report_ListData(IDataTablesRequest request, ReportFilterVM filter)
        {
            if (request.Draw == 1)
            {
                Audit_Operation = NomenclatureConstants.AuditOperations.View;
                Audit_Object = "Справка за подадени отводи";
            }
            var data = dismissalService.SelectReport(filter);
            return new DataTablesJsonResult(request.GetResponse(data), true);
        }
    }
}
