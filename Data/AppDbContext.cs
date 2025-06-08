using Microsoft.EntityFrameworkCore;
using ParkingSystem.Models;

namespace ParkingSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}

    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<ParkingLot>  ParkingLots { get; set; } = null!;
    public DbSet<ParkingRecord> ParkingRecords { get; set; } = null!;
    
    public DbSet<User> Users { get; set; } = null!;
    
    //PLACA DO CARRO ÚNICA - DIRETO BD
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.LicensePlate)
            .IsUnique();
    }
}