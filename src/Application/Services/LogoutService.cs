using IdentityModel;
using IdentityServer.Application.Constants;
using IdentityServer.Application.Models;
using IdentityServer.Application.Services.Interfaces;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application.Services;

public class LogoutService : ILogoutService
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogoutService(
        IIdentityServerInteractionService interaction, 
        IHttpContextAccessor httpContextAccessor)
    {
        _interaction = interaction;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
    {
        var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

        if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated != true)
        {
            // if the user is not authenticated, then just show logged out page
            vm.ShowLogoutPrompt = false;
            return vm;
        }

        var context = await _interaction.GetLogoutContextAsync(logoutId);
        if (context?.ShowSignoutPrompt != false)
            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        
        // it's safe to automatically sign-out
        vm.ShowLogoutPrompt = false;
        return vm;
    }
    
    public async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
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

        if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated != true) 
            return vm;
        
        var idp = _httpContextAccessor.HttpContext?.User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
        if (idp is null or IdentityServer4.IdentityServerConstants.LocalIdentityProvider) 
            return vm;
        
        var providerSupportsSignOut = await _httpContextAccessor.HttpContext.GetSchemeSupportsSignOutAsync(idp);
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