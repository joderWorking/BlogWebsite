using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data.EFs;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
        // Constructor logic here
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var readerRoleId = "9d7de7c5-39cd-40a3-be92-0cda9ce7cecc";
        var writerRoleId = "8050f363-d781-4b59-a1b7-99a7212d8f74";

        //Create reader and writer roles
        var roles = new List<IdentityRole>
        {
            new()
            {
                Id = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper(),
                ConcurrencyStamp = readerRoleId
            },
            new()
            {
                Id = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper(),
                ConcurrencyStamp = writerRoleId
            }
        };
        // Seed roles into the database
        builder.Entity<IdentityRole>().HasData(roles);

        //  Create admin user
        var adminUserId = "90791e2e-fa24-4b45-b309-2b1823a7f695";
        var admin = new IdentityUser
        {
            Id = adminUserId,
            UserName = "admin@codepulse.com",
            Email = "admin@codepulse.com",
            NormalizedEmail = "admin@codepulse.com".ToUpper(),
            NormalizedUserName = "admin@codepulse.com".ToUpper()
        };
        admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

        builder.Entity<IdentityUser>().HasData(admin);

        // Give roles to admin 
        var adminRoles = new List<IdentityUserRole<string>>
        {
            new()
            {
                UserId = adminUserId,
                RoleId = readerRoleId
            },
            new()
            {
                UserId = adminUserId,
                RoleId = writerRoleId
            }
        };
        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }
}