using GestionLocativeDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionLocativeDemo.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options) {

  public DbSet<Bien> Biens { get; set; }
  public DbSet<Locataire> Locataires { get; set; }
  public DbSet<Echeance> Echeances { get; set; }

  protected override void OnModelCreating(
    ModelBuilder builder) {

    base.OnModelCreating(builder);

    builder.Entity<Bien>()
    .ToTable("Properties");

    builder.Entity<Bien>()
    .Property(bien => bien.TypeBien)
    .HasColumnName("Type");

    builder.Entity<Bien>()
        .Property(bien => bien.Etage)
        .HasColumnName("Floor");

    builder.Entity<Bien>()
        .Property(bien => bien.NumeroLot)
        .HasColumnName("LotNumber");

    builder.Entity<Bien>()
        .Property(bien => bien.Adresse)
        .HasColumnName("Address");

    builder.Entity<Bien>()
        .Property(bien => bien.CodePostal)
        .HasColumnName("PostalCode");

    builder.Entity<Bien>()
        .Property(bien => bien.Ville)
        .HasColumnName("City");

    builder.Entity<Bien>()
        .Property(bien => bien.IdentifiantFiscal)
        .HasColumnName("FiscalIdentifier");

    builder.Entity<Bien>()
        .Property(bien => bien.Loyer)
        .HasColumnName("Rent");

    builder.Entity<Bien>()
        .Property(bien => bien.EstLoue)
        .HasColumnName("IsRented");

    builder.Entity<Locataire>()
    .ToTable("Tenants");

    builder.Entity<Echeance>()
        .ToTable("RentDues");

    builder.Entity<Echeance>()
        .Property(echeance => echeance.BienId)
        .HasColumnName("PropertyId");

    builder.Entity<Echeance>()
        .Property(echeance => echeance.LocataireId)
        .HasColumnName("TenantId");

    builder.Entity<Echeance>()
        .Property(echeance => echeance.DateEcheance)
        .HasColumnName("DueDate");

    builder.Entity<Echeance>()
        .Property(echeance => echeance.Loyer)
        .HasColumnName("Rent");

    builder.Entity<Echeance>()
        .Property(echeance => echeance.Statut)
        .HasColumnName("Status");

    builder.Entity<Echeance>()
        .HasOne(echeance => echeance.Bien)
        .WithMany()
        .HasForeignKey(echeance => echeance.BienId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<Echeance>()
        .HasOne(echeance => echeance.Locataire)
        .WithMany()
        .HasForeignKey(echeance => echeance.LocataireId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}

