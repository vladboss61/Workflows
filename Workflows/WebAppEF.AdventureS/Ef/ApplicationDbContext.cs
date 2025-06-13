using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebAppEF.AdventureS.Ef;


public class User
{
    public int Id { get; set; }

    public string UserGuid { get; set; }

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? RoleId { get; set; }

    public Role Role { get; set; }
}

public class Role
{
    public int Id { get; set; }

    public string RoleGuid { get; set; }

    public RoleType RoleType { get; set; }

    public ICollection<User> Users { get; set; }
}

public class RoleTypes
{
    public int Id { get; set; }

    public string RoleName { get; set; }

    public RoleType RoleType { get; set; }

    public string RoleDescription { get; set; }
}

public enum RoleType
{
    Admin, Developer, Manager, Tester
}


public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<RoleTypes> RoleTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User", "mad");
        modelBuilder.Entity<Role>().ToTable("Role", "mad");
        modelBuilder.Entity<RoleTypes>().ToTable("RoleTypes", "mad");

        modelBuilder.Entity<User>().HasKey(x => x.Id);

        modelBuilder.Entity<Role>().HasKey(x => x.Id);
        modelBuilder.Entity<RoleTypes>().HasKey(x => x.Id);

        modelBuilder.Entity<User>()
            .HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId);

        modelBuilder.Entity<User>()
            .Property(x => x.Name)
            .HasMaxLength(250)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(x => x.CreatedAt)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(x => x.RoleId)
            .IsRequired(false);

        modelBuilder.Entity<RoleTypes>()
            .Property(x => x.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<RoleTypes>().Property(x => x.RoleName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<RoleTypes>().Property(x => x.RoleDescription)
            .HasMaxLength(1500)
            .IsRequired(false);

        modelBuilder.Entity<RoleTypes>().HasData([
            new RoleTypes
            {
                 Id = 1,
                 RoleType = RoleType.Admin,
                 RoleName = "Admin",
                 RoleDescription = "Administrator with full access to the system."
            },
             new RoleTypes
            {
                 Id = 2,
                 RoleType = RoleType.Developer,
                 RoleName = "Developer",
                 RoleDescription = "Developer with access to development tools and resources."
            },
             new RoleTypes
                {
                    Id = 3,
                    RoleType = RoleType.Manager,
                    RoleName = "Manager",
                    RoleDescription = "Manager with access to management tools and resources."
                },
            new RoleTypes
                {
                    Id = 4,
                    RoleType = RoleType.Tester,
                    RoleName = "Tester",
                    RoleDescription = "Tester with acces to some internal resources."
                }
            ]);
    }
}
