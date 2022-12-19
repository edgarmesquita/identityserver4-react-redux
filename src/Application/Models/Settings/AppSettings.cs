namespace IdentityServer.Application.Models.Settings;

public class AppSettings
{
    public DatabaseSettings Database { get; set; } = new();
}