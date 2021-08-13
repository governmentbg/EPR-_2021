using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.ViewModels.Cdn;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

namespace EPRO.Areas.Admin.Controllers
{
    /// <summary>
    /// Управление на файлово съдържание
    /// </summary>
    public class FilesController : BaseController
    {
        private readonly ICdnService cdnService;
        private readonly ILogger<FilesController> logger;
        public FilesController(
            ICdnService _cdnService,
            ILogger<FilesController> _logger)
        {
            this.cdnService = _cdnService;
            this.logger = _logger;
        }

        /// <summary>
        /// Изтегляне списък на файлове към даден обект (отвод)
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="sourceID"></param>
        /// <returns></returns>
        public IActionResult GetFileList(int sourceType, string sourceID)
        {
            List<CdnItemVM> model = cdnService.Select(sourceType, sourceID)
                                    .Where(x => x.DateExpired == null).ToList();
            return Json(model);
        }

        /// <summary>
        /// Добавяне на нов файл
        /// </summary>
        /// <param name="sourceType">Тип на обекта</param>
        /// <param name="sourceId">Идентификатор на обекта</param>
        /// <param name="container">Контайнер за прикачване</param>
        /// <param name="defaultTitle">Заглавие на контрола</param>
        /// <returns></returns>
        public PartialViewResult UploadFile(int sourceType, string sourceId, string container, string defaultTitle)
        {
            CdnUploadRequest model = new CdnUploadRequest()
            {
                SourceType = sourceType,
                SourceId = sourceId,
                FileContainer = container,
                Title = defaultTitle,
                MaxFileSize = 30
            };
            return PartialView(model);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile(ICollection<IFormFile> files, CdnUploadRequest model)
        {
            if (files != null && files.Count() > 0)
            {
                string result = "failed";
                if (model.MaxFileSize > 0)
                {
                    long maxSize = (long)model.MaxFileSize * 1024 * 1024;
                    if (files.Any(x => x.Length > maxSize))
                    {
                        return Content("max_size");
                    }
                }
                foreach (var file in files)
                {
                    var fileExt = Path.GetExtension(file.FileName).Replace(".", "").ToLower();
                    string[] acceptFileExts = { "doc", "docx", "rtf", "pdf", "html" };
                    if (!acceptFileExts.Contains(fileExt))
                    {
                        return Content("file_ext");
                    }
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        model.FileContentBase64 = Convert.ToBase64String(ms.ToArray());
                    }
                    model.ContentType = file.ContentType;
                    model.FileName = Path.GetFileName(file.FileName);
                    var isOk = await cdnService.MongoCdn_AppendUpdate(model);
                    if (isOk)
                    {
                        result = "ok";
                    }
                    else
                    {
                        result = "failed";
                        break;
                    }
                }
                return Content(result);
            }
            else
            {
                return Content("failed");
            }
        }

        /// <summary>
        /// Изтегляне на документ по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FileResult> Download(string id)
        {
            var model = await cdnService.MongoCdn_Download(id);
            return File(Convert.FromBase64String(model.FileContentBase64), model.ContentType, model.FileName);
        }

        /// <summary>
        /// Преглед на документ - само за pdf и изображения
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FileResult> Preview(string id)
        {
            var model = await cdnService.MongoCdn_Download(id);

            var contentDispositionHeader = new ContentDisposition
            {
                Inline = true,
                FileName = HttpUtility.UrlPathEncode(model.FileName).Replace(",", "%2C")
            };

            Response.Headers.Add("Content-Disposition", contentDispositionHeader.ToString());

            byte[] fileBytes = Convert.FromBase64String(model.FileContentBase64);


            return File(fileBytes, model.ContentType);
        }
    }
}
