using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoList.Api.Infrastructure.Extensions;
using TodoList.Api.Models;
using TodoList.Api.Models.Configuration;

public class TodoListDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
    {

    }

    public DbSet<Task> Tasks { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<File> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AppUserConfiguration());
        builder.ApplyConfiguration(new AppRoleConfiguration());
        builder.ApplyConfiguration(new TaskConfiguration());
        builder.ApplyConfiguration(new StepConfiguration());
        builder.ApplyConfiguration(new FileConfiguration());

        builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

        builder.Seed();
    }
}