using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Application.Models.Data;

public class UserRoleData : IdentityUserRole<Guid>
{
    public const string TableName = "UserRoles";
}