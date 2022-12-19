namespace IdentityServer.Application.Models.Settings;

public class DatabaseSettings
{
    public string? ServerVersion { get; set; }

    /// <summary>
    /// Gets or sets the value of the max retry count
    /// </summary>
    public int MaxRetryCount { get; set; } = 5;

    /// <summary>
    /// Gets or sets the value of the max retry delay
    /// </summary>
    public int MaxRetryDelay { get; set; } = 15;
}