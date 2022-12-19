using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Application.Models.Data;

public class UserLoginData : IdentityUserLogin<Guid>
{
    public const string TableName = "UserLogins";
}