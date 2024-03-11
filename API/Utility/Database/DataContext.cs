using API.Utility.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Utility.Database;

public sealed class DataContext : DbContext
{
    public DataContext()
    {
        Database.Migrate();
    }

    public DbSet<Authorization> Authorization { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Price> Prices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Program.Settings.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Authorization>().ToTable("Authorization");
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Employee>().ToTable("Employees");
        modelBuilder.Entity<Appointment>().ToTable("Appointments");
        modelBuilder.Entity<Order>().ToTable("Orders");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<Price>().ToTable("Prices");
    }
}