using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using EPRO.Core.Contracts;
using EPRO.Core.Models.FilterModels;
using EPRO.Extensions;
using EPRO.Infrastructure.Constants;
using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Models.Common;
using EPRO.Infrastructure.Data.Models.Nomenclatures;
using EPRO.Infrastructure.ViewModels.Common;
using EPRO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EPRO.Areas.Controllers
{
    /// <summary>
    /// Публичен контролер - свободен достъп
    /// </summary>
    [AllowAnonymous]
    [Area("public")]
    public class PublicController : Controller
    {
        private readonly ICdnService cdnService;
        private readonly IDismissalService dismissalService;
        private readonly INomenclatureService nomService;
        private readonly RecaptchaOptions recaptcha;
        public PublicController(
            ICdnService _cdnService,
            IDismissalService _dismissalService,
            IOptions<RecaptchaOptions> _recaptcha,
            INomenclatureService _nomService)
        {
            cdnService = _cdnService;
            dismissalService = _dismissalService;
            recaptcha = _recaptcha.Value;
            nomService = _nomService;
        }

        /// <summary>
        /// Начален екран за търсене на отводи
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            ViewBag.CourtId_ddl = nomService.GetDropDownList<Court>(false, true);
            ViewBag.DismissalTypeId_ddl = nomService.GetDropDownList<DismissalType>(false, true);
            ViewBag.CaseTypeId_ddl = nomService.GetDropDownList<CaseType>(false, true);
            ViewBag.CaseYear_ddl = await dismissalService.Get_DDLYearsAsync();
            var model = new DismissalFilterVM();
            return View(model);
        }

        [HttpPost]
        public IActionResult ListData(IDataTablesRequest request, DismissalFilterVM filter)
        {
            if (request.Draw == 1)
            {
                if (string.IsNullOrEmpty(filter.reCaptchaToken) || !ReCaptchaPassed(filter.reCaptchaToken))
                {
                    return new DataTablesJsonResult(request.GetResponse((new List<DismissalListVM>()).AsQueryable()), true);
                }
            }
            filter.ActiveOnly = true;
            var data = dismissalService.Select(filter);
            return new DataTablesJsonResult(request.GetResponse(data), true);
        }
        /// <summary>
        /// Обработка на антиспам защита - Google ReCaptcha
        /// </summary>
        /// <param name="gRecaptchaResponse"></param>
        /// <returns></returns>
        private bool ReCaptchaPassed(string gRecaptchaResponse)
        {
            HttpClient httpClient = new HttpClient();

            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={recaptcha.SecretKey}&response={gRecaptchaResponse}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
                return false;

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= 0.5m)
                return false;

            return true;
        }

        

        /// <summary>
        /// Преглед на отвод
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DismissalView(string id)
        {
            var model = await dismissalService.GetById<Dismissal>(id);

            if (model == null)
            {
                throw new NotFoundException();
            }

            ViewBag.CourtId_ddl = nomService.GetDropDownList<Court>(model.CourtId);
            ViewBag.DismissalTypeId_ddl = nomService.GetDropDownList<DismissalType>(model.DismissalTypeId);
            ViewBag.CaseTypeId_ddl = nomService.GetDropDownList<CaseType>(model.CaseTypeId);
            ViewBag.CaseRoleId_ddl = nomService.GetDropDownList<CaseRole>(model.CaseRoleId);
            ViewBag.HearingTypeId_ddl = nomService.GetDropDownList<HearingType>(model.HearingTypeId);
            ViewBag.ActTypeId_ddl = nomService.GetDropDownList<ActType>(model.ActTypeId);

            ViewBag.fileInfo = cdnService.Select(NomenclatureConstants.SourceType.Dismissal, id)
                                                .Where(x => x.DateExpired == null).FirstOrDefault();

            return View(model);
        }

        /// <summary>
        /// Изтегляне на отвод в pdf документ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Print(string id)
        {
            return RedirectToAction("Print", "Dismissal", new { area = "admin", id });
        }

        /// <summary>
        /// Изтегляне съдържание на акт към отвод
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FileResult> Download(string id)
        {
            var model = await cdnService.MongoCdn_Download(id);
            return File(Convert.FromBase64String(model.FileContentBase64), model.ContentType, model.FileName);
        }

        /// <summary>
        /// Управление на грешки при изпълнение на операции
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var feature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature.Error.Message;
            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = error,
                InnerException = feature.Error.InnerException?.Message
            };
            if (feature.Error is NotFoundException)
            {
                errorModel.Title = "Ненамерен ресурс";
            }
            return View(errorModel);
        }
    }
}
