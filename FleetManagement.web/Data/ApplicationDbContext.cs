using FleetManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<TokenBlacklist> TokenBlacklist { get; set; }

       protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    // =============================================================
    // Fix: MySQL key length limit (utf8mb4 = 4 bytes/char, 3072 byte max)
    // Identity default sizes are too big for composite PKs/FKs.
    // =============================================================

    // users table
    builder.Entity<ApplicationUser>(e =>
    {
        e.ToTable("users");
        e.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);              // 36 = UUID length
        e.Property(x => x.UserName).HasColumnName("user_name").HasMaxLength(100);
        e.Property(x => x.NormalizedUserName).HasColumnName("normalized_user_name").HasMaxLength(100);
        e.Property(x => x.Email).HasColumnName("email").HasMaxLength(150);
        e.Property(x => x.NormalizedEmail).HasColumnName("normalized_email").HasMaxLength(150);
        e.Property(x => x.PasswordHash).HasColumnName("password_hash");
        e.Property(x => x.SecurityStamp).HasColumnName("security_stamp").HasMaxLength(100);
        e.Property(x => x.ConcurrencyStamp).HasColumnName("concurrency_stamp").HasMaxLength(100);
        e.Property(x => x.PhoneNumber).HasColumnName("phone").HasMaxLength(20);
        e.Property(x => x.EmailConfirmed).HasColumnName("email_confirmed");
        e.Property(x => x.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
        e.Property(x => x.TwoFactorEnabled).HasColumnName("two_factor_enabled");
        e.Property(x => x.LockoutEnabled).HasColumnName("lockout_enabled");
        e.Property(x => x.LockoutEnd).HasColumnName("lockout_until");
        e.Property(x => x.AccessFailedCount).HasColumnName("failed_attempts");
        e.Property(x => x.Name).HasColumnName("name").HasMaxLength(100);
        e.Property(x => x.IsActive).HasColumnName("is_active");
        e.Property(x => x.LastLoginAt).HasColumnName("last_login_at");
        e.Property(x => x.LastLoginIp).HasColumnName("last_login_ip").HasMaxLength(45);
        e.Property(x => x.CreatedAt).HasColumnName("created_at");
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        e.Property(x => x.DeletedAt).HasColumnName("deleted_at");

        e.HasIndex(x => x.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
        e.HasIndex(x => x.NormalizedEmail).HasDatabaseName("EmailIndex");
    });

    // roles table
    builder.Entity<ApplicationRole>(e =>
    {
        e.ToTable("roles");
        e.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);              // 36 = UUID length
        e.Property(x => x.Name).HasColumnName("name").HasMaxLength(50);
        e.Property(x => x.NormalizedName).HasColumnName("normalized_name").HasMaxLength(50);
        e.Property(x => x.ConcurrencyStamp).HasColumnName("concurrency_stamp").HasMaxLength(100);
        e.Property(x => x.Description).HasColumnName("description").HasMaxLength(255);
        e.Property(x => x.CreatedAt).HasColumnName("created_at");

        e.HasIndex(x => x.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
    });

    // user_roles
    builder.Entity<IdentityUserRole<string>>(e =>
    {
        e.ToTable("user_roles");
        e.Property(x => x.UserId).HasColumnName("user_id").HasMaxLength(36);
        e.Property(x => x.RoleId).HasColumnName("role_id").HasMaxLength(36);
    });

    // user_claims
    builder.Entity<IdentityUserClaim<string>>(e =>
    {
        e.ToTable("user_claims");
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.UserId).HasColumnName("user_id").HasMaxLength(36);
        e.Property(x => x.ClaimType).HasColumnName("claim_type").HasMaxLength(255);
        e.Property(x => x.ClaimValue).HasColumnName("claim_value").HasMaxLength(255);
    });

    // role_claims
    builder.Entity<IdentityRoleClaim<string>>(e =>
    {
        e.ToTable("role_claims");
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.RoleId).HasColumnName("role_id").HasMaxLength(36);
        e.Property(x => x.ClaimType).HasColumnName("claim_type").HasMaxLength(255);
        e.Property(x => x.ClaimValue).HasColumnName("claim_value").HasMaxLength(255);
    });

    // user_logins
    builder.Entity<IdentityUserLogin<string>>(e =>
    {
        e.ToTable("user_logins");
        e.Property(x => x.LoginProvider).HasColumnName("login_provider").HasMaxLength(128);
        e.Property(x => x.ProviderKey).HasColumnName("provider_key").HasMaxLength(128);
        e.Property(x => x.ProviderDisplayName).HasColumnName("provider_display_name").HasMaxLength(255);
        e.Property(x => x.UserId).HasColumnName("user_id").HasMaxLength(36);
    });

    builder.Entity<IdentityUserToken<string>>(e =>
    {
        e.ToTable("user_tokens");
        e.Property(x => x.UserId).HasColumnName("user_id").HasMaxLength(36);
        e.Property(x => x.LoginProvider).HasColumnName("login_provider").HasMaxLength(128);
        e.Property(x => x.Name).HasColumnName("name").HasMaxLength(128);
        e.Property(x => x.Value).HasColumnName("value");
    });

    // Refresh tokens
    builder.Entity<RefreshToken>(e =>
    {
        e.ToTable("refresh_tokens");
        e.HasKey(x => x.Id);
        e.Property(x => x.UserId).HasMaxLength(36);
        e.HasIndex(x => x.TokenHash).IsUnique();
    });

    builder.Entity<TokenBlacklist>(e =>
    {
        e.ToTable("token_blacklist");
        e.HasKey(x => x.Jti);
        e.Property(x => x.Jti).HasMaxLength(36);
        e.Property(x => x.UserId).HasMaxLength(36);
    });
}
    }
}