using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Application.Models.Data;

public class UserClaimData : IdentityUserClaim<Guid>
{
    public const string TableName = "UserClaims";
}