using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Application.Models.Data;

public class UserData : IdentityUser<Guid>
{
    public const string TableName = "Users";
}