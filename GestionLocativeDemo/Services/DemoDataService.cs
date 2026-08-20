using GestionLocativeDemo.Data;
using GestionLocativeDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionLocativeDemo.Services;

public sealed class DemoDataService {

  private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

  public DemoDataService(
      IDbContextFactory<ApplicationDbContext> dbFactory) {

    _dbFactory = dbFactory;
  }

  public async Task SeedLocatairesAsync() {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    if (await db.Locataires.AnyAsync())
      return;

    db.Locataires.AddRange(
        new Locataire {
          FirstName = "Marie",
          LastName = "Dupont",
          Address = "12 rue de la République, 35000 Rennes",
          Email = "marie.dupont@example.fr",
          Phone = "06 10 20 30 40"
        },

        new Locataire {
          FirstName = "Julien",
          LastName = "Leroy",
          Address = "8 avenue Victor-Hugo, 44000 Nantes",
          Email = "julien.leroy@example.fr",
          Phone = "06 20 30 40 50"
        },

        new Locataire {
          FirstName = "Sophie",
          LastName = "Bernard",
          Address = "24 rue du Port, 56000 Vannes",
          Email = "sophie.bernard@example.fr",
          Phone = "06 30 40 50 60"
        });

    await db.SaveChangesAsync();
  }

  public async Task SeedPropertiesAsync() {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    // Si des biens existent déjà, on ne touche à rien.
    if (await db.Biens.AnyAsync())
      return;

    db.Biens.AddRange(
        new Bien {
          Type = "Appartement T2",
          Surface = 42,
          Floor = 2,
          LotNumber = "12",
          Address = "12 rue de la République",
          PostalCode = "35000",
          City = "Rennes",
          FiscalIdentifier = "350238001234",
          Rent = 750m,
          Charges = 50m,
          IsRented = true
        },

        new Bien {
          Type = "Studio",
          Surface = 28,
          Floor = 1,
          LotNumber = "4",
          Address = "8 avenue Victor-Hugo",
          PostalCode = "44000",
          City = "Nantes",
          FiscalIdentifier = "440109005678",
          Rent = 590m,
          Charges = 40m,
          IsRented = true
        },

        new Bien {
          Type = "Appartement T3",
          Surface = 61,
          Floor = 3,
          LotNumber = "8",
          Address = "24 rue du Port",
          PostalCode = "56000",
          City = "Vannes",
          FiscalIdentifier = "560260009876",
          Rent = 950m,
          Charges = 70m,
          IsRented = true
        },

        new Bien {
          Type = "Maison T4",
          Surface = 92,
          City = "Lorient",
          Rent = 1100m,
          Charges = 50m,
          IsRented = true
        });

    await db.SaveChangesAsync();
  }

  public async Task<List<Bien>> GetPropertiesAsync() {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Biens
        .AsNoTracking()
        .OrderBy(property => property.Id)
        .ToListAsync();
  }

  public async Task<Bien?> GetPropertyAsync(int id) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Biens
        .AsNoTracking()
        .FirstOrDefaultAsync(property => property.Id == id);
  }

  public async Task AddPropertyAsync(Bien property) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    db.Biens.Add(property);

    await db.SaveChangesAsync();
  }


  public async Task<bool> UpdatePropertyAsync(Bien property) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    Bien? existingProperty =
        await db.Biens
            .FirstOrDefaultAsync(p => p.Id == property.Id);

    if (existingProperty == null)
      return false;

    existingProperty.Type = property.Type;
    existingProperty.Surface = property.Surface;
    existingProperty.Floor = property.Floor;
    existingProperty.LotNumber = property.LotNumber;
    existingProperty.Address = property.Address;
    existingProperty.PostalCode = property.PostalCode;
    existingProperty.City = property.City;
    existingProperty.FiscalIdentifier = property.FiscalIdentifier;
    existingProperty.Rent = property.Rent;
    existingProperty.Charges = property.Charges;
    existingProperty.IsRented = property.IsRented;

    await db.SaveChangesAsync();

    return true;
  }

  public async Task<List<Locataire>> GetLocatairesAsync() {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Locataires
        .AsNoTracking()
        .OrderBy(tenant => tenant.Id)
        .ToListAsync();
  }


  public async Task<Locataire?> GetTenantAsync(int id) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Locataires
        .AsNoTracking()
        .FirstOrDefaultAsync(tenant => tenant.Id == id);
  }


  public async Task AddTenantAsync(Locataire tenant) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    db.Locataires.Add(tenant);

    await db.SaveChangesAsync();
  }


  public async Task<bool> UpdateTenantAsync(Locataire tenant) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    Locataire? existingTenant =
        await db.Locataires
            .FirstOrDefaultAsync(t => t.Id == tenant.Id);

    if (existingTenant == null)
      return false;

    existingTenant.FirstName = tenant.FirstName;
    existingTenant.LastName = tenant.LastName;
    existingTenant.Address = tenant.Address;
    existingTenant.Email = tenant.Email;
    existingTenant.Phone = tenant.Phone;

    await db.SaveChangesAsync();

    return true;
  }

  public async Task SeedRentDuesAsync() {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    if (await db.RentDues.AnyAsync())
      return;


    Bien? propertyRennes =
        await db.Biens.FirstOrDefaultAsync(
            property =>
                property.FiscalIdentifier == "350238001234");

    Bien? propertyNantes =
        await db.Biens.FirstOrDefaultAsync(
            property =>
                property.FiscalIdentifier == "440109005678");

    Bien? propertyVannes =
        await db.Biens.FirstOrDefaultAsync(
            property =>
                property.FiscalIdentifier == "560260009876");


    Locataire? marie =
        await db.Locataires.FirstOrDefaultAsync(
            tenant =>
                tenant.Email == "marie.dupont@example.fr");

    Locataire? julien =
        await db.Locataires.FirstOrDefaultAsync(
            tenant =>
                tenant.Email == "julien.leroy@example.fr");

    Locataire? sophie =
        await db.Locataires.FirstOrDefaultAsync(
            tenant =>
                tenant.Email == "sophie.bernard@example.fr");


    if (propertyRennes == null ||
        propertyNantes == null ||
        propertyVannes == null ||
        marie == null ||
        julien == null ||
        sophie == null) {

      throw new InvalidOperationException(
          "Impossible d'initialiser les échéances : " +
          "un bien ou un locataire de démonstration est introuvable.");
    }


    db.RentDues.AddRange(

        new RentDue {
          PropertyId = propertyRennes.Id,
          TenantId = marie.Id,
          DueDate = new DateTime(2026,9,5),
          Rent = 750m,
          Charges = 50m,
          Status = RentDueStatus.Upcoming
        },

        new RentDue {
          PropertyId = propertyNantes.Id,
          TenantId = julien.Id,
          DueDate = new DateTime(2026,9,5),
          Rent = 590m,
          Charges = 40m,
          Status = RentDueStatus.Upcoming
        },

        new RentDue {
          PropertyId = propertyVannes.Id,
          TenantId = sophie.Id,
          DueDate = new DateTime(2026,8,5),
          Rent = 730m,
          Charges = 70m,
          Status = RentDueStatus.Late
        });

    await db.SaveChangesAsync();
  }

  public async Task<List<RentDue>> GetRentDuesAsync() {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.RentDues
        .AsNoTracking()
        .Include(rentDue => rentDue.Property)
        .Include(rentDue => rentDue.Locataire)
        .OrderBy(rentDue => rentDue.DueDate)
        .ToListAsync();
  }
}