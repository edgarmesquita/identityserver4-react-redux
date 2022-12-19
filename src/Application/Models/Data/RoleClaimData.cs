using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Application.Models.Data;

public class RoleClaimData : IdentityRoleClaim<Guid>
{
    public const string TableName = "RoleClaims";
}