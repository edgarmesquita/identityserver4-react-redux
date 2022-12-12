using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Endpoints;

public class GetLoginInfoEndpoint : EndpointBaseAsync.WithRequest<string>.WithActionResult
{
    public override Task<ActionResult> HandleAsync([FromQuery] string returnUrl, CancellationToken cancellationToken = new())
    {
        // build a model so we know what to show on the login page
        var vm = await BuildLoginViewModelAsync(returnUrl);

        if (vm.IsExternalLoginOnly)
        {
            // we only have one option for logging in and it's an external provider
            return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
        }

        return View(vm);
    }
}