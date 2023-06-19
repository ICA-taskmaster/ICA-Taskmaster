using AgentService.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentService.Data;

public static class PrepDb {
    public static void prepPopulation(IApplicationBuilder app, bool isProduction, ILogger logger) {
        using var serviceScope = app.ApplicationServices.CreateScope();
        seedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction, logger);
    }

    private static void seedData(AppDbContext context, bool isProduction, ILogger logger) {
        if (isProduction) {
            try {
                context.Database.Migrate();
                logger.LogInformation("Database migration completed");
            }
            catch (Exception ex) {
                logger.LogError(ex, "Could not run migrations: {ErrorMessage}", ex.Message);
            }
        }

        if (!context.agents.Any()) {
            logger.LogInformation("Seeding data...");
            
            context.agents.AddRange(
                new Agent { realName = "47", codeName = "Agent 47", burnerPhone = "06336046925", securityClearance = "Orange" },
                new Agent { realName = "Diana Penelope Burnwood", codeName = "Burnwood", burnerPhone = "06336375625", securityClearance = "Red" },
                new Agent { realName = "Alexander Fanin", codeName = "Fan", burnerPhone = "06336500395", securityClearance = "Purple" }
            );

            context.SaveChanges();
            logger.LogInformation("Data seeding completed");
        }
        else {
            logger.LogInformation("Data already exists in the database");
        }
    }
}