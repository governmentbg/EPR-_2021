using EPRO.Components;
using EPRO.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EPRO.Areas.Admin.Controllers
{

    /// <summary>
    /// Начален екран 
    /// </summary>
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> _logger)
        {
            this.logger = _logger;
        }

        /// <summary>
        /// Начален екран в административен панел
        /// </summary>
        /// <returns></returns>
        [MenuItem("home")]
        public IActionResult Index()
        {
            return View();
        }

        [MenuItem("home")]
        public IActionResult Denied()
        {
            return View();
        }        
    }
}
