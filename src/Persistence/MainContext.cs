using IdentityServer.Application.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Persistence;

public class MainContext: IdentityDbContext<UserData, RoleData, Guid, UserClaimData, UserRoleData, UserLoginData, RoleClaimData, UserTokenData>
{
    public MainContext(DbContextOptions<MainContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserData>(b =>
        {
            b.Property(r => r.Id).HasColumnName("UserId");
            b.ToTable(UserData.TableName);
        });
        builder.Entity<UserClaimData>(b =>
        {
            b.Property(r => r.Id).HasColumnName("UserClaimId");
            b.ToTable(UserClaimData.TableName);
        });
        builder.Entity<UserLoginData>(b =>
        {
            b.ToTable(UserLoginData.TableName);
        });
        builder.Entity<UserRoleData>(b =>
        {
            b.ToTable(UserRoleData.TableName);
        });
        builder.Entity<UserTokenData>(b =>
        {
            b.ToTable(UserTokenData.TableName);
        });
        
        builder.Entity<RoleData>(b =>
        {
            b.Property(r => r.Id).HasColumnName("RoleId");
            b.ToTable(RoleData.TableName);
        });
        builder.Entity<RoleClaimData>(b =>
        {
            b.Property(r => r.Id).HasColumnName("RoleClaimId");
            b.ToTable(RoleClaimData.TableName);
        });
    }
}