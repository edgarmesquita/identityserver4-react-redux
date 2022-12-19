using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Application.Models.Data;

public class UserTokenData : IdentityUserToken<Guid>
{
    public const string TableName = "UserTokens";
}