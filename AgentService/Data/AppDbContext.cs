using AgentService.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentService.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) {
        
    }

    public DbSet<Agent> agents { get; set; }
}