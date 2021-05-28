using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace QuestStoreNAT.web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found!";
                    _logger.LogWarning($"404 error occured. Path = {statusCodeResult.OriginalPath} " +
                        $"and QueryString = {statusCodeResult.OriginalQueryString}");
                    return View("NotFound");
                case 500:
                    ViewBag.ErrorMessage = "Sorry, there was an server error. Try later or...";
                    _logger.LogWarning($"500 error occured. Path = {statusCodeResult.OriginalPath} " +
                        $"and QueryString = {statusCodeResult.OriginalQueryString}");
                    return View("Error");
            }
            return View("Error");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            _logger.LogError($"The path {exceptionDetails.Path} threw an exception {exceptionDetails.Error}");

            //ViewBag.ExceptionPath = exceptionDetails.Path;
            //ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            //ViewBag.StackTrace = exceptionDetails.Error.StackTrace;

            return View("Error");
        }
    }
}
