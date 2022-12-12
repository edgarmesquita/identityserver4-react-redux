using IdentityServer.Application.Models;

namespace IdentityServer.Application.Services.Interfaces;

public interface ILogoutService
{
    Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId);
    Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId);
}