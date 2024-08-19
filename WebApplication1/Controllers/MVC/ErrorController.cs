using ForumProject.Services;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController(IUserContextService userContextService) : base(userContextService)
        {
        }
        
        public IActionResult Index(int? statusCode = null, string message = null)
        {
            if (statusCode.HasValue)
            {
                ViewBag.StatusCode = statusCode.Value;
            }

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }

            return View();
        }
    }
}
