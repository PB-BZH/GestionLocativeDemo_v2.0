using GestionLocativeDemo.Models;

namespace GestionLocativeDemo.Services;

public sealed class DemoDataService {

  private readonly List<Property> _properties = [
      new Property {
          Id = 1,
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

      new Property {
          Id = 2,
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

      new Property {
          Id = 3,
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

      new Property {
        Id = 4,
        Type = "Maison T4",
        Surface = 92,
        City = "Lorient",
        Rent = 1100m,
        Charges = 50m,
        IsRented = true
    }
  ];


  private readonly List<Tenant> _tenants = [
      new Tenant {
          Id = 1,
          FirstName = "Marie",
          LastName = "Dupont",
          Address = "12 rue de la République, 35000 Rennes",
          Email = "marie.dupont@example.fr",
          Phone = "06 10 20 30 40"
      },

      new Tenant {
          Id = 2,
          FirstName = "Julien",
          LastName = "Leroy",
          Address = "8 avenue Victor-Hugo, 44000 Nantes",
          Email = "julien.leroy@example.fr",
          Phone = "06 20 30 40 50"
      },

      new Tenant {
          Id = 3,
          FirstName = "Sophie",
          LastName = "Bernard",
          Address = "24 rue du Port, 56000 Vannes",
          Email = "sophie.bernard@example.fr",
          Phone = "06 30 40 50 60"
      }
  ];


  private readonly List<RentDue> _rentDues = [
      new RentDue {
          Id = 1,
          PropertyId = 1,
          TenantId = 1,
          DueDate = new DateTime(2026, 9, 5),
          Rent = 750m,
          Charges = 50m,
          Status = RentDueStatus.Upcoming
      },

      new RentDue {
          Id = 2,
          PropertyId = 2,
          TenantId = 2,
          DueDate = new DateTime(2026, 9, 5),
          Rent = 590m,
          Charges = 40m,
          Status = RentDueStatus.Upcoming
      },

      new RentDue {
          Id = 3,
          PropertyId = 3,
          TenantId = 3,
          DueDate = new DateTime(2026, 8, 5),
          Rent = 730m,
          Charges = 70m,
          Status = RentDueStatus.Late
      }
  ];


  public IReadOnlyList<Property> Properties =>
      _properties;

  public IReadOnlyList<Tenant> Tenants =>
      _tenants;

  public IReadOnlyList<RentDue> RentDues =>
      _rentDues;


  public Property? GetProperty(int id) =>
      _properties.FirstOrDefault(
          property => property.Id == id);


  public Tenant? GetTenant(int id) =>
      _tenants.FirstOrDefault(
          tenant => tenant.Id == id);

  public void AddProperty(Property property) {

    int nextId =
        _properties.Count == 0
            ? 1
            : _properties.Max(p => p.Id) + 1;

    property.Id = nextId;

    _properties.Add(property);
  }


  public bool UpdateProperty(Property property) {

    Property? existingProperty =
        _properties.FirstOrDefault(
            p => p.Id == property.Id);

    if (existingProperty == null)
      return false;

    existingProperty.Type =
        property.Type;

    existingProperty.Surface =
        property.Surface;

    existingProperty.Floor =
        property.Floor;

    existingProperty.LotNumber =
        property.LotNumber;

    existingProperty.Address =
        property.Address;

    existingProperty.PostalCode =
        property.PostalCode;

    existingProperty.City =
        property.City;

    existingProperty.FiscalIdentifier =
        property.FiscalIdentifier;

    existingProperty.Rent =
        property.Rent;

    existingProperty.Charges =
        property.Charges;

    existingProperty.IsRented =
        property.IsRented;

    return true;
  }

  public void AddTenant(Tenant tenant) {

    int nextId =
        _tenants.Count == 0
            ? 1
            : _tenants.Max(t => t.Id) + 1;

    tenant.Id = nextId;

    _tenants.Add(tenant);
  }


  public bool UpdateTenant(Tenant tenant) {

    Tenant? existingTenant =
        _tenants.FirstOrDefault(
            t => t.Id == tenant.Id);

    if (existingTenant == null)
      return false;

    existingTenant.FirstName =
        tenant.FirstName;

    existingTenant.LastName =
        tenant.LastName;

    existingTenant.Address =
        tenant.Address;

    existingTenant.Email =
        tenant.Email;

    existingTenant.Phone =
        tenant.Phone;

    return true;
  }
}