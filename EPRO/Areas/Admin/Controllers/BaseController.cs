using IO.LogOperation.Models;
using EPRO.Components;
using EPRO.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using EPRO.Core.Constants;
using EPRO.Core.Contracts;

namespace EPRO.Areas.Admin.Controllers
{
    /// <summary>
    /// Базов контролер
    /// </summary>
    [Authorize]
    [Area("admin")]
    public class BaseController : Controller
    {
        private IUserContext _userContext;

        protected IUserContext userContext
        {
            get
            {
                if (_userContext == null)
                {
                    _userContext = (IUserContext)HttpContext
                         .RequestServices
                         .GetService(typeof(IUserContext));
                }

                return _userContext;
            }
        }


        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            /*
             *      Управление на активния елемент на менюто
             *      ViewBag.MenuItemValue съдържа ключовата дума, отговорна за отварянето на менюто
             *      Ако не намери атрибут на action-а MenuItem("{keyword}"), се използва името на action-а
             *      Ако action-а е от вида List_Edit се подава list (отрязва до последния символ долна подчертавка)
             */
            ControllerActionDescriptor controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor != null)
            {
                ActionName = controllerActionDescriptor.ActionName;
                ControllerName = controllerActionDescriptor.ControllerName;
                object currentMenuItem = null;
                var menuAttrib = controllerActionDescriptor
                                    .MethodInfo
                                    .CustomAttributes
                                    .FirstOrDefault(a => a.AttributeType == typeof(MenuItemAttribute));
                if (menuAttrib != null)
                {
                    currentMenuItem = menuAttrib.ConstructorArguments[0].Value;
                }
                if (currentMenuItem == null)
                {
                    var actionName = controllerActionDescriptor.ActionName;
                    if (actionName.Contains('_'))
                    {
                        currentMenuItem = actionName.Substring(0, actionName.LastIndexOf('_')).ToLower();
                    }
                    else
                    {
                        currentMenuItem = actionName.ToLower();
                    }
                }
                ViewBag.MenuItemValue = currentMenuItem;
                ViewBag.ActionName = this.ActionName;
            }
            // ---------Управление на активния елемент на менюто, край

            Audit_CourtId = userContext.CourtId;
        }

        /// <summary>
        /// Текущ контекст на промените за даден обект
        /// </summary>
        ActionExecutedContext lastContext;
        string lastClientIP;
        protected int? Audit_CourtId;
        protected string Audit_Operation;
        protected string Audit_Object;
        protected string Audit_Action;
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            lastContext = context;
            lastClientIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            //if (Request.Headers.TryGetValue("X-Forwarded-For", out var currentIp))
            //{
            //    lastClientIP = currentIp;
            //}

        }

        protected override void Dispose(bool disposing)
        {
            if (!string.IsNullOrEmpty(Audit_Operation))
            {

                var requestUrl = lastContext.HttpContext.Request.Path;
                var auditService = (IAuditLogService)HttpContext.RequestServices.GetService(typeof(IAuditLogService));
                var auditSave = auditService.SaveAuditLog(Audit_CourtId, Audit_Operation, Audit_Object, lastClientIP, requestUrl, Audit_Action).Result;
            }
            base.Dispose(disposing);
        }

        protected void SetSuccessMessage(string message = MessageConstant.Values.SaveOK)
        {
            TempData[MessageConstant.SuccessMessage] = message;
        }

        protected void SetErrorMessage(string message = MessageConstant.Values.SaveFailed)
        {
            TempData[MessageConstant.ErrorMessage] = message;
        }
    }
}