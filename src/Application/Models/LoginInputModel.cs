using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Application.Models;

public class LoginInputModel
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    public bool RememberLogin { get; set; }
    
    public string? ReturnUrl { get; set; }
    
    public bool Accept { get; set; } = true;
}