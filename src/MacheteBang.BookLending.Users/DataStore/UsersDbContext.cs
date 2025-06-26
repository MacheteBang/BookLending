using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MacheteBang.BookLending.Users.DataStore;

internal sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Associate Identity with custom table names (and custom column names)
        builder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.HasKey(k => k.Id).HasName("UserId");
            b.Property(p => p.Id).HasColumnName("UserId");
        });

        builder.Entity<Role>(b =>
        {
            b.ToTable("Roles");
            b.HasKey(k => k.Id).HasName("RoleId");
            b.Property(p => p.Id).HasColumnName("RoleId");
        });

        builder.Entity<IdentityUserRole<Guid>>(b =>
        {
            b.ToTable("UserRoles");
            b.HasKey(k => new { k.UserId, k.RoleId });
        });

        builder.Entity<IdentityRoleClaim<Guid>>(b =>
        {
            b.ToTable("RoleClaims");
            b.HasKey(k => k.Id);
            b.Property(p => p.Id).HasColumnName("RoleClaimId");
        });

        builder.Entity<IdentityUserClaim<Guid>>(b =>
        {
            b.ToTable("UserClaims");
            b.HasKey(k => k.Id).HasName("UserClaimId");
            b.Property(p => p.Id).HasColumnName("UserClaimId");
        });

        builder.Entity<IdentityUserLogin<Guid>>(b =>
        {
            b.ToTable("UserLogins");
            b.HasKey(k => new { k.UserId, k.ProviderKey });
        });

        builder.Entity<IdentityUserToken<Guid>>(b =>
        {
            b.ToTable("UserTokens");
            b.HasKey(k => new { k.UserId, k.LoginProvider, k.Name });
        });
    }

}
