using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Flurl;
using IdentityServer.Application.Models;
using IdentityServer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Endpoints.Account;

public class GetLoginInfoEndpoint : EndpointBaseAsync.WithRequest<string>.WithActionResult
{
    private readonly ILoginService _loginService;

    public GetLoginInfoEndpoint(ILoginService loginService)
    {
        _loginService = loginService;
    }
    
    public override async Task<ActionResult> HandleAsync([FromQuery] string returnUrl, CancellationToken cancellationToken = new())
    {
        // build a model so we know what to show on the login page
        var vm = await _loginService.BuildLoginViewModelAsync(returnUrl);

        if (vm.IsExternalLoginOnly)
        {
            // we only have one option for logging in and it's an external provider
            return Ok(new RedirectViewModel
            {
                RedirectUrl = Url.Content("~/external/challenge"
                    .SetQueryParams(new { scheme = vm.ExternalLoginScheme, returnUrl }))!
            });
        }

        return Ok(vm);
    }
}