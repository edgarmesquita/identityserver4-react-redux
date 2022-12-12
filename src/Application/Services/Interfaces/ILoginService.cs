using IdentityServer.Application.Models;

namespace IdentityServer.Application.Services.Interfaces;

public interface ILoginService
{
    Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model);
    Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl);
}