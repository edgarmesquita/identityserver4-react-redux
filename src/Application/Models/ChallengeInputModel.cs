using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Application.Models;

public class ChallengeInputModel
{
    [FromQuery]
    public string Scheme { get; set; } = string.Empty;
    
    [FromQuery]
    public string? ReturnUrl { get; set; }
}