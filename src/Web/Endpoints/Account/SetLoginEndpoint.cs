using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using IdentityServer.Application.Constants;
using IdentityServer.Application.Models;
using IdentityServer.Application.Models.Data;
using IdentityServer.Application.Services.Interfaces;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Endpoints.Account;

public class SetLoginEndpoint : EndpointBaseAsync.WithRequest<LoginInputModel>.WithActionResult
{
    private readonly UserManager<UserData> _userManager;
    private readonly SignInManager<UserData> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly ILoginService _loginService;

    public SetLoginEndpoint(
        UserManager<UserData> userManager,
        SignInManager<UserData> signInManager,
        IIdentityServerInteractionService interaction,
        IEventService events,
        ILoginService loginService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _events = events;
        _loginService = loginService;
    }

    [AllowAnonymous]
    [HttpPost("account/login")]
    public override async Task<ActionResult> HandleAsync(LoginInputModel request,
        CancellationToken cancellationToken = new())
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);

        // the user clicked the "cancel" button
        if (!request.Accept)
        {
            if (context == null) 
                // since we don't have a valid context, then we just go back to the home page
                return Ok(new RedirectViewModel { RedirectUrl = "/" });
            
            // if the user cancels, send a result back into IdentityServer as if they 
            // denied the consent (even if this client does not require consent).
            // this will send back an access denied OIDC error response to the client.
            await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                
            return Ok(new RedirectViewModel { RedirectUrl = request.ReturnUrl });
        }

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password,
                request.RememberLogin, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName,
                    clientId: context?.Client.ClientId));

                if (context != null)
                {
                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Ok(new RedirectViewModel { RedirectUrl = request.ReturnUrl });
                }

                // request for a local page
                if (Url.IsLocalUrl(request.ReturnUrl))
                {
                    return Ok(new RedirectViewModel { RedirectUrl = request.ReturnUrl });
                }

                if (string.IsNullOrEmpty(request.ReturnUrl))
                {
                    return Ok(new RedirectViewModel { RedirectUrl = "/" });
                }

                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            await _events.RaiseAsync(new UserLoginFailureEvent(request.Username, "invalid credentials",
                clientId: context?.Client.ClientId));
            ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
        }

        // something went wrong, show form with error
        var vm = await _loginService.BuildLoginViewModelAsync(request);
        return BadRequest(vm);
    }
    
    
}