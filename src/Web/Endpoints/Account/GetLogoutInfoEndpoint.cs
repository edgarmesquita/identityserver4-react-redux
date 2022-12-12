using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using IdentityServer.Application.Models.Data;
using IdentityServer.Application.Services.Interfaces;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Endpoints.Account;

public class GetLogoutInfoEndpoint : EndpointBaseAsync.WithRequest<string>.WithActionResult
{
    private readonly SignInManager<UserData> _signInManager;
    private readonly IEventService _events;
    private readonly ILogoutService _logoutService;

    public GetLogoutInfoEndpoint(
        SignInManager<UserData> signInManager,
        IEventService events, 
        ILogoutService logoutService)
    {
        _signInManager = signInManager;
        _events = events;
        _logoutService = logoutService;
    }
    
    public override async Task<ActionResult> HandleAsync([FromQuery] string logoutId, CancellationToken cancellationToken = new CancellationToken())
    {
        // build a model so the logout page knows what to display
        var vm = await _logoutService.BuildLogoutViewModelAsync(logoutId);

        if (vm.ShowLogoutPrompt) 
            return Ok(vm);
        
        // if the request for logout was properly authenticated from IdentityServer, then
        // we don't need to show the prompt and can just log the user out directly.
        var endpoint = new SetLogoutEndpoint(_signInManager, _events, _logoutService);
        return await endpoint.HandleAsync(vm, cancellationToken);
    }
}