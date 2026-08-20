using GestionLocativeDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionLocativeDemo.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options) {

  public DbSet<Property> Properties { get; set; }
  public DbSet<Tenant> Tenants { get; set; }
  public DbSet<RentDue> RentDues { get; set; }

  protected override void OnModelCreating(
    ModelBuilder builder) {

    base.OnModelCreating(builder);

    builder.Entity<RentDue>()
        .HasOne(rentDue => rentDue.Property)
        .WithMany()
        .HasForeignKey(rentDue => rentDue.PropertyId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<RentDue>()
        .HasOne(rentDue => rentDue.Tenant)
        .WithMany()
        .HasForeignKey(rentDue => rentDue.TenantId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}

