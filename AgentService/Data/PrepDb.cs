using AgentService.Models;

namespace AgentService.Data;

public static class PrepDb {
    public static void prepPopulation(IApplicationBuilder app) {
        using( var serviceScope = app.ApplicationServices.CreateScope()) {
            seedData(serviceScope.ServiceProvider.GetService<AppDbContext>() ?? throw new InvalidOperationException());
        }
    }

    private static void seedData(AppDbContext context) {
        if (!context.agents.Any())
        {
            Console.WriteLine("Seeding data...");
            context.agents.AddRange(
                new Agent { realName = "47", codeName = "Agent 47", burnerPhone = "06336046925", securityClearance = "Orange" },
                new Agent { realName = "Diana Penelope Burnwood", codeName = "Burnwood", burnerPhone = "06336375625", securityClearance = "Red" },
                new Agent { realName = "Alexander Fanin", codeName = "Fan", burnerPhone = "06336500395", securityClearance = "Purple" }
                );
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("We already have data");
        }
    }
}