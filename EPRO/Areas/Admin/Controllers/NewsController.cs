using EPRO.Components;
using EPRO.Core.Contracts;
using EPRO.Extensions;
using EPRO.Infrastructure.Constants;
using EPRO.Infrastructure.Data.Models.Common;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPRO.Areas.Admin.Controllers
{
    
    //public class NewsController : BaseController
    //{
    //    private readonly INewsService newsService;

    //    public NewsController(INewsService _newsService)
    //    {
    //        newsService = _newsService;
    //    }

    //    [MenuItem("news")]
    //    public IActionResult Index()
    //    {
    //        return View();
    //    }

    //    [HttpPost]
    //    public IActionResult ListData(IDataTablesRequest request)
    //    {
    //        var data = newsService.Select(false, request.Search?.Value);
    //        return new DataTablesJsonResult(request.GetResponse(data), true);
    //    }

    //    public IActionResult Add()
    //    {
    //        var model = new News();
    //        return View(nameof(Edit), model);
    //    }

    //    public async Task<IActionResult> Edit(int id)
    //    {
    //        var model = await newsService.GetById<News>(id);
    //        return View(nameof(Edit), model);
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> Edit(News model)
    //    {
    //        int currentId = model.Id;
    //        var result = await newsService.SaveData(model);
    //        if (result.Result)
    //        {
    //            if (currentId == 0)
    //            {
    //                Audit_Operation = NomenclatureConstants.AuditOperations.Add;
    //            }
    //            else
    //            {
    //                Audit_Operation = NomenclatureConstants.AuditOperations.Edit;
    //            }
    //            Audit_Object = $"Новина: {model.Label}";
    //            SetSuccessMessage();
    //            return RedirectToAction(nameof(Edit), new { id = model.Id });
    //        }
    //        else
    //        {
    //            SetErrorMessage();
    //            return View(model);
    //        }
    //    }
    //}
}
