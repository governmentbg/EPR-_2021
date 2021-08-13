using EPRO.Core.Contracts;
using EPRO.Infrastructure.Data.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPRO.Api.Controllers
{

    /// <summary>
    /// Съобщения обменяни между ЕПРО и софтуерите използвани от съдилищата за управление на дела
    /// </summary>
    public class DismissalController : BaseController
    {

        private readonly IApiService dismissalService;

        private string CourtCode
        {
            get
            {
                if (this.User == null)
                {
                    return null;
                }
                return this.User.Claims
                        .Where(x => x.Type == ClaimTypes.UserData)
                        .Select(x => x.Value)
                        .FirstOrDefault();
            }
        }

        public DismissalController(IApiService _dismissalService)
        {
            dismissalService = _dismissalService;
        }

        /// <summary>
        /// Заявка за регистриране на отвод/самоотвод
        /// </summary>
        /// <returns></returns>
        /// <response code="200">R002 Отговор за регистриране на отвод/самоотвод</response>
        /// <response code="400">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        /// <response code="500">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        [HttpPost]
        [Route("DismissalInsert")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DismissalRegistrationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
        public async Task<IActionResult> DismissalInsert(DismissalRegistrationRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DismissalRegistrationResponse result = await dismissalService.Insert(model);

                    if (result.Error != null)
                    {
                        return BadRequest(result);
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                else
                {

                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                var res = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                return res;
            }

        }


        /// <summary>
        /// Заявка за корекция на данни за отвод/самоотвод
        /// </summary>
        /// <returns></returns>
        /// <response code="200">R005 Отговор за регистриране на отвод/самоотвод</response>
        /// <response code="400">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        /// <response code="500">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        [HttpPost]
        [Route("DismissalUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DismissalRegistrationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
        public async Task<IActionResult> DismissalUpdate(DismissalRegistrationRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    UpdateResponse result = await dismissalService.Update(model);

                    if (result.Error != null)
                    {
                        return BadRequest(result.Error);
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                var res = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                return res;
            }

        }

        /// <summary>
        /// Заявка за регистриране на данни за нов съдия при отвод/самоотвод 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">R005 Отговор за регистриране на отвод/самоотвод</response>
        /// <response code="400">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        /// <response code="500">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        [HttpPost]
        [Route("ReplaceUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DismissalRegistrationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
        public async Task<IActionResult> ReplaceUpdate(ReplaceDismissalRequest model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    UpdateResponse result = await dismissalService.ReplaceUpdate(model, CourtCode);

                    if (result.Error != null)
                    {
                        return BadRequest(result.Error);
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                var res = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                return res;
            }

        }



        /// <summary>
        /// Заявка за изпращане на обезличен файл на съдебен акт 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">R005 Отговор за регистриране на отвод/самоотвод</response>
        /// <response code="400">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        /// <response code="500">CL099 Съобщение за грешка в отговор на заявка от всеки тип</response>
        [HttpPost]
        [Route("ActUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DismissalRegistrationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
        public async Task<IActionResult> ActUpdate(ActPublicationRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UpdateResponse result = await dismissalService.DismissalActUpdate(model);

                    if (result.Error != null)
                    {
                        return BadRequest(result.Error);
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                var res = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                return res;
            }

        }

    }
}
