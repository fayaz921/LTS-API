using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Infrastructure.Persistence.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migration applied successfully.");

            await SeedSuperAdminAsync(scope.ServiceProvider, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying migrations.");
            throw;
        }
    }
    private static async Task SeedSuperAdminAsync(
        IServiceProvider services,
        ILogger logger)
    {
        var context = services.GetRequiredService<AppDbContext>();
        var passwordHasher = services.GetRequiredService<IPasswordHasher<User>>();

        // check if superadmin already exists
        var exists = await context.Users
            .AnyAsync(u => u.Role == UserRole.SuperAdmin);

        if (exists)
        {
            logger.LogInformation("SuperAdmin already exists. Skipping seed.");
            return;
        }

        // create default organization for superadmin
        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            OrganizationName = "LTS Administration",
            Slug = "lts-administration",
            Plan = SubscriptionPlan.Enterprise,
            IsActive = true,
            IsSubscriptionActive = true,
            MaxUsers = 100,
            MaxClients = 1000,
            SubscriptionStartDate = DateTime.UtcNow,
            SubscriptionEndDate = DateTime.UtcNow.AddYears(10),
            CreatedAt = DateTime.UtcNow,
        };

        // create superadmin user
        var superAdmin = new User
        {
            Id = Guid.NewGuid(),
            OrganizationId = organization.Id,
            Name = "Super Admin",
            Email = "mfayaz21703@gmail.com",
            Role = UserRole.SuperAdmin,
            IsActive = true,
            Otp = string.Empty,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "system",
        };

        superAdmin.PasswordHash = passwordHasher.HashPassword(superAdmin, "fayaz921");

        await context.Organizations.AddAsync(organization);
        await context.Users.AddAsync(superAdmin);
        await context.SaveChangesAsync();

        logger.LogInformation("SuperAdmin seeded successfully. Email: mfayaz21703@gmail.com Password: fayaz921");
    }
}