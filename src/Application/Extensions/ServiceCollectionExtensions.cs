using IdentityServer.Application.Services;
using IdentityServer.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<ILoginService, LoginService>();

        return services;
    }
}