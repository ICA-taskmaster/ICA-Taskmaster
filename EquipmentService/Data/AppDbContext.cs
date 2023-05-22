using EquipmentService.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentService.Data;

public class AppDbContext : DbContext {
    
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) {
        
    } 

    public DbSet<Agent> agents { get; set; }
    public DbSet<Equipment> equipments { get; set; }

    protected void onModelCreating(ModelBuilder modelBuilder) {
        modelBuilder
            .Entity<Agent>()
            .HasMany(a => a.equipments)
            .WithOne(e => e.agent!)
            .HasForeignKey(e => e.agentId);

        modelBuilder
            .Entity<Equipment>()
            .HasOne(e => e.agent)
            .WithMany(a => a.equipments)
            .HasForeignKey(e => e.agentId);
    }
}