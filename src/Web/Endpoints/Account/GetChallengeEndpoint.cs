using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using IdentityServer.Application.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Endpoints.Account;

public class GetChallengeEndpoint: EndpointBaseAsync.WithRequest<ChallengeInputModel>.WithActionResult
{
    private readonly IIdentityServerInteractionService _interaction;

    public GetChallengeEndpoint(IIdentityServerInteractionService interaction)
    {
        _interaction = interaction;
    }
    
    [HttpGet("external/challenge", Name = nameof(GetChallengeEndpoint))]
    public override Task<ActionResult> HandleAsync(ChallengeInputModel request, CancellationToken cancellationToken = new())
    {
        if (string.IsNullOrEmpty(request.ReturnUrl)) request.ReturnUrl = "~/";

        // validate returnUrl - either it is a valid OIDC URL or back to a local page
        if (Url.IsLocalUrl(request.ReturnUrl) == false && _interaction.IsValidReturnUrl(request.ReturnUrl) == false)
        {
            // user might have clicked on a malicious link - should be logged
            throw new Exception("invalid return URL");
        }
            
        // start challenge and roundtrip the return URL and scheme 
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.RouteUrl(nameof(GetCallbackEndpoint)), 
            Items =
            {
                { "returnUrl", request.ReturnUrl }, 
                { "scheme", request.Scheme },
            }
        };

        return Task.FromResult( (ActionResult)Challenge(props, request.Scheme) );
    }
}