using IdentityServer.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Extensions;

public static class ActionResultExtensions
{
    public static ActionResult LoadingPage(this ControllerBase controller, string viewName, string redirectUri)
    {
        controller.HttpContext.Response.StatusCode = 200;
        controller.HttpContext.Response.Headers["Location"] = "";
            
        return controller.RedirectToAction(viewName, new RedirectViewModel { RedirectUrl = redirectUri });
    }
}