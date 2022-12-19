using IdentityServer.Application.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IdentityServer.Persistence.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseMainPersistence(this DbContextOptionsBuilder opt, IConfiguration config, DatabaseSettings settings)
    {
        var serverVersion = new MySqlServerVersion(new Version(settings.ServerVersion ?? "8.0.30"));
        opt.UseMySql(config.GetConnectionString("DefaultConnection")!, serverVersion, x =>
        {
            x.UseNetTopologySuite();
            x.EnableRetryOnFailure(
                settings.MaxRetryCount,
                TimeSpan.FromSeconds(settings.MaxRetryDelay),
                null
            );
        });
        return opt;
    }
}