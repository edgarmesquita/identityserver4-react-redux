using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using IdentityModel;
using IdentityServer.Application.Models;
using IdentityServer.Web.Constants;
using IdentityServer.Web.Models;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Endpoints;

public class AccountLogoutEndpoint: EndpointBaseAsync.WithRequest<LogoutInputModel>.WithActionResult
{
    private readonly SignInManager<UserData> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;

    public AccountLogoutEndpoint(
        SignInManager<UserData> signInManager,
        IIdentityServerInteractionService interaction,
        IEventService events)
    {
        _signInManager = signInManager;
        _interaction = interaction;
        _events = events;
    }
    
    [AllowAnonymous]
    [HttpPost("account/logout")]
    public override async Task<ActionResult> HandleAsync(LogoutInputModel request, CancellationToken cancellationToken = new())
    {
        // build a model so the logged out page knows what to display
        var vm = await BuildLoggedOutViewModelAsync(request.LogoutId);

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
        var url = Url.Content($"~/logout?logoutId={vm.LogoutId}");
            
        // this triggers a redirect to the external provider for sign-out
        return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
    }
    
    private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
    {
        // get context information (client name, post logout redirect URI and iframe for federated signOut)
        var logout = await _interaction.GetLogoutContextAsync(logoutId);

        var vm = new LoggedOutViewModel
        {
            AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
            SignOutIframeUrl = logout?.SignOutIFrameUrl,
            LogoutId = logoutId
        };

        if (User?.Identity?.IsAuthenticated != true) 
            return vm;
        
        var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
        if (idp is null or IdentityServer4.IdentityServerConstants.LocalIdentityProvider) 
            return vm;
        
        var providerSupportsSignOut = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
        if (!providerSupportsSignOut) 
            return vm;
        
        // if there's no current logout context, we need to create one
        // this captures necessary info from the current logged in user
        // before we signOut and redirect away to the external IdP for signOut
        vm.LogoutId ??= await _interaction.CreateLogoutContextAsync();

        vm.ExternalAuthenticationScheme = idp;

        return vm;
    }
}