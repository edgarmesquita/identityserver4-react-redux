using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Application.Models.Data;
using IdentityServer.Application.Services.Interfaces;
using IdentityServer.Web.Endpoints.Account.Base;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Endpoints.Account;

public class GetLogoutInfoEndpoint : LogoutEndpointBase<string>
{
    public GetLogoutInfoEndpoint(
        SignInManager<UserData> signInManager,
        IEventService events, 
        ILogoutService logoutService) : base(signInManager, events, logoutService)
    {
    }
    
    [HttpGet("account/logout")]
    public override async Task<ActionResult> HandleAsync([FromQuery] string logoutId, CancellationToken cancellationToken = new CancellationToken())
    {
        // build a model so the logout page knows what to display
        var vm = await LogoutService.BuildLogoutViewModelAsync(logoutId);

        if (vm.ShowLogoutPrompt) 
            return Ok(vm);
        
        // if the request for logout was properly authenticated from IdentityServer, then
        // we don't need to show the prompt and can just log the user out directly.
        return await LogoutAsync(vm, cancellationToken);
    }
}