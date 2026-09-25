using EduTek.Application.Services;
using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EduTek.API.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<EduTek.API.Program>
{
    private readonly string _databaseName = $"EduTekTests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            RemoveDbContext(services);

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }

    private static void RemoveDbContext(IServiceCollection services)
    {
        var descriptors = services
            .Where(d =>
                d.ServiceType == typeof(AppDbContext) ||
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                (d.ServiceType.IsGenericType &&
                 d.ServiceType.Name.Contains("DbContextOptions", StringComparison.Ordinal)))
            .ToList();

        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }

    public void Seed()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasherService>();

        db.Database.EnsureCreated();

        if (db.Roles.Any())
        {
            return;
        }

        db.Roles.AddRange(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "Teacher" },
            new Role { Id = 3, Name = "Student" });

        db.Users.AddRange(
            new User
            {
                Username = "admin",
                Email = "admin@edutek.test",
                PasswordHash = hasher.HashPassword("Admin@123"),
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Username = "teacher",
                Email = "teacher@edutek.test",
                PasswordHash = hasher.HashPassword("Teacher@123"),
                RoleId = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Username = "student",
                Email = "student@edutek.test",
                PasswordHash = hasher.HashPassword("Student@123"),
                RoleId = 3,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

        db.SaveChanges();
    }
}
