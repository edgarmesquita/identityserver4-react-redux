using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Application.Models;
using IdentityServer.Application.Models.Data;
using IdentityServer.Application.Services.Interfaces;
using IdentityServer.Web.Endpoints.Account.Base;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Endpoints.Account;

public class SetLogoutEndpoint: LogoutEndpointBase<LogoutInputModel>
{
    public SetLogoutEndpoint(
        SignInManager<UserData> signInManager,
        IEventService events, 
        ILogoutService logoutService) : base(signInManager, events, logoutService)
    {
    }
    
    [AllowAnonymous]
    [HttpPost("account/logout")]
    public override Task<ActionResult> HandleAsync(LogoutInputModel request, CancellationToken cancellationToken = new())
    {
        return LogoutAsync(request, cancellationToken);
    }
}