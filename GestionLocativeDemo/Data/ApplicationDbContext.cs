using GestionLocativeDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionLocativeDemo.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options) {

  public DbSet<Bien> Biens { get; set; }
  public DbSet<Locataire> Tenants { get; set; }
  public DbSet<RentDue> RentDues { get; set; }

  protected override void OnModelCreating(
    ModelBuilder builder) {

    base.OnModelCreating(builder);

    builder.Entity<Bien>()
    .ToTable("Properties");

    builder.Entity<Locataire>()
    .ToTable("Tenants");

    builder.Entity<RentDue>()
        .HasOne(rentDue => rentDue.Property)
        .WithMany()
        .HasForeignKey(rentDue => rentDue.PropertyId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<RentDue>()
        .HasOne(rentDue => rentDue.Locataire)
        .WithMany()
        .HasForeignKey(rentDue => rentDue.TenantId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}

