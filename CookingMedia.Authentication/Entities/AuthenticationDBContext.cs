using Microsoft.EntityFrameworkCore;

namespace CookingMedia.Authentication.Entities;

public class AuthenticationDBContext : DbContext
{
    public AuthenticationDBContext()
    {
    }

    public AuthenticationDBContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "admin@cm.com",
                Password = "$argon2i$v=19$m=65536,t=3,p=1$ZjVVSktlS1cyYkRRNEdpNA$8gEdYXIzMcxk8CdwrribFoKbVVUrkpg6pDrj52lYSt8",
                Name = "Admin",
                Role = User.Roles.Admin
            },
            new User
            {
                Id = 1001,
                Email = "member1@cm.com",
                Password = "$argon2i$v=19$m=65536,t=3,p=1$NTQwOTR5WUs5U0EzTlFWeg$Y2pWpaI9J7g0tXP+YN0ueUvuqkDt0Tv65KCA/U0qE/Q",
                Name = "Member1",
                Role = User.Roles.Member
            }
        );
    }
}
