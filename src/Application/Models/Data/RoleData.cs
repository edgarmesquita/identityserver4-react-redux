using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Application.Models.Data;

public class RoleData : IdentityRole<Guid>
{
    public const string TableName = "Roles";
}