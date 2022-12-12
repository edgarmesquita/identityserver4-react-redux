using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Flurl;
using IdentityServer.Application.Models;
using IdentityServer.Application.Models.Data;
using IdentityServer.Application.Services.Interfaces;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Endpoints.Account.Base;

public abstract class LogoutEndpointBase<TRequest> : EndpointBaseAsync.WithRequest<TRequest>.WithActionResult
{
    private readonly SignInManager<UserData> _signInManager;
    private readonly IEventService _events;
    protected readonly ILogoutService LogoutService;

    protected LogoutEndpointBase(
        SignInManager<UserData> signInManager,
        IEventService events,
        ILogoutService logoutService)
    {
        _signInManager = signInManager;
        _events = events;
        LogoutService = logoutService;
    }
    
    protected async Task<ActionResult> LogoutAsync(LogoutInputModel request, CancellationToken cancellationToken = new())
    {
        // build a model so the logged out page knows what to display
        var vm = await LogoutService.BuildLoggedOutViewModelAsync(request.LogoutId);

        if (User?.Identity?.IsAuthenticated == true)
        {
            // delete local authentication cookie
            await _signInManager.SignOutAsync();

            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        }

        // check if we need to trigger sign-out at an upstream identity provider
        if (!vm.TriggerExternalSignout) 
            return Ok(vm);
        
        // build a return URL so the upstream provider will redirect back
        // to us after the user has logged out. this allows us to then
        // complete our single sign-out processing.
        var url = Url.Content($"~/logout".SetQueryParams(new { logoutId = vm.LogoutId}));
            
        // this triggers a redirect to the external provider for sign-out
        return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
    }
}