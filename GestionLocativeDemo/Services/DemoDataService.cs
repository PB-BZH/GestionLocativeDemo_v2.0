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
          Prenom = "Marie",
          Nom = "Dupont",
          Adresse = "12 rue de la République, 35000 Rennes",
          Email = "marie.dupont@example.fr",
          Telephone = "06 10 20 30 40"
        },

        new Locataire {
          Prenom = "Julien",
          Nom = "Leroy",
          Adresse = "8 avenue Victor-Hugo, 44000 Nantes",
          Email = "julien.leroy@example.fr",
          Telephone = "06 20 30 40 50"
        },

        new Locataire {
          Prenom = "Sophie",
          Nom = "Bernard",
          Adresse = "24 rue du Port, 56000 Vannes",
          Email = "sophie.bernard@example.fr",
          Telephone = "06 30 40 50 60"
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
          TypeBien = "Appartement T2",
          Surface = 42,
          Etage = 2,
          NumeroLot = "12",
          Adresse = "12 rue de la République",
          CodePostal = "35000",
          Ville = "Rennes",
          IdentifiantFiscal = "350238001234",
          Loyer = 750m,
          Charges = 50m,
          EstLoue = true
        },

        new Bien {
          TypeBien = "Studio",
          Surface = 28,
          Etage = 1,
          NumeroLot = "4",
          Adresse = "8 avenue Victor-Hugo",
          CodePostal = "44000",
          Ville = "Nantes",
          IdentifiantFiscal = "440109005678",
          Loyer = 590m,
          Charges = 40m,
          EstLoue = true
        },

        new Bien {
          TypeBien = "Appartement T3",
          Surface = 61,
          Etage = 3,
          NumeroLot = "8",
          Adresse = "24 rue du Port",
          CodePostal = "56000",
          Ville = "Vannes",
          IdentifiantFiscal = "560260009876",
          Loyer = 950m,
          Charges = 70m,
          EstLoue = true
        },

        new Bien {
          TypeBien = "Maison T4",
          Surface = 92,
          Ville = "Lorient",
          Loyer = 1100m,
          Charges = 50m,
          EstLoue = true
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

    existingProperty.TypeBien = property.TypeBien;
    existingProperty.Surface = property.Surface;
    existingProperty.Etage = property.Etage;
    existingProperty.NumeroLot = property.NumeroLot;
    existingProperty.Adresse = property.Adresse;
    existingProperty.CodePostal = property.CodePostal;
    existingProperty.Ville = property.Ville;
    existingProperty.IdentifiantFiscal = property.IdentifiantFiscal;
    existingProperty.Loyer = property.Loyer;
    existingProperty.Charges = property.Charges;
    existingProperty.EstLoue = property.EstLoue;

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

    existingTenant.Prenom = tenant.Prenom;
    existingTenant.Nom = tenant.Nom;
    existingTenant.Adresse = tenant.Adresse;
    existingTenant.Email = tenant.Email;
    existingTenant.Telephone = tenant.Telephone;

    await db.SaveChangesAsync();

    return true;
  }

  public async Task<List<Echeance>> GetEcheancesAsync() {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Echeances
        .AsNoTracking()
        .Include(Echeance => Echeance.Bien)
        .Include(Echeance => Echeance.Locataire)
        .OrderBy(Echeance => Echeance.DateEcheance)
        .ToListAsync();
  }

  public async Task<List<Bail>> GetBauxAsync() {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Baux
        .AsNoTracking()
        .Include(bail => bail.Bien)
        .Include(bail => bail.Locataire)
        .OrderByDescending(bail => bail.EstActif)
        .ThenByDescending(bail => bail.DateDebut)
        .ToListAsync();
  }
  public async Task<Bail?> GetBailAsync(int id) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Baux
        .AsNoTracking()
        .Include(bail => bail.Bien)
        .Include(bail => bail.Locataire)
        .FirstOrDefaultAsync(bail => bail.Id == id);
  }


  public async Task AddBailAsync(Bail bail) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    db.Baux.Add(bail);

    await db.SaveChangesAsync();
  }


  public async Task<bool> UpdateBailAsync(Bail bail) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    Bail? bailExistant =
        await db.Baux
            .FirstOrDefaultAsync(b => b.Id == bail.Id);

    if (bailExistant == null)
      return false;

    bailExistant.BienId = bail.BienId;
    bailExistant.LocataireId = bail.LocataireId;
    bailExistant.Type = bail.Type;
    bailExistant.DateDebut = bail.DateDebut;
    bailExistant.DureeMois = bail.DureeMois;
    bailExistant.Loyer = bail.Loyer;
    bailExistant.Charges = bail.Charges;
    bailExistant.DepotGarantie = bail.DepotGarantie;
    bailExistant.JourPaiement = bail.JourPaiement;
    bailExistant.EstActif = bail.EstActif;

    await db.SaveChangesAsync();

    return true;
  }

  public async Task<Echeance?> GenererEcheanceAsync(
    int bailId,
    DateTime mois) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    Bail? bail =
        await db.Baux
            .Include(b => b.Bien)
            .Include(b => b.Locataire)
            .FirstOrDefaultAsync(b => b.Id == bailId);

    if (bail == null || !bail.EstActif)
      return null;

    DateTime premierJourMois =
        new(mois.Year,mois.Month,1);

    DateTime premierJourBail =
        new(bail.DateDebut.Year,bail.DateDebut.Month,1);

    if (premierJourMois < premierJourBail)
      return null;

    if (bail.DureeMois.HasValue) {

      DateTime premierMoisHorsBail =
          premierJourBail.AddMonths(
              bail.DureeMois.Value);

      if (premierJourMois >= premierMoisHorsBail)
        return null;
    }


    if (bail == null || !bail.EstActif)
      return null;

    int jour =
        Math.Min(
            bail.JourPaiement,
            DateTime.DaysInMonth(
                mois.Year,
                mois.Month));

    DateTime dateEcheance =
        new(
            mois.Year,
            mois.Month,
            jour);

    bool existeDeja =
        await db.Echeances.AnyAsync(
            e => e.BailId == bailId &&
                 e.DateEcheance.Year == mois.Year &&
                 e.DateEcheance.Month == mois.Month);

    if (existeDeja)
      return null;

    Echeance echeance = new() {
      BailId = bail.Id,

      // Encore conservés pendant la phase de transition.
      BienId = bail.BienId,
      LocataireId = bail.LocataireId,

      DateEcheance = dateEcheance,
      Loyer = bail.Loyer,
      Charges = bail.Charges,
      Statut =
        dateEcheance < DateTime.Today
          ? StatusEcheance.EnRetard
          : dateEcheance == DateTime.Today
              ? StatusEcheance.APayer
              : StatusEcheance.Avenir
    };

    db.Echeances.Add(echeance);

    await db.SaveChangesAsync();

    return echeance;
  }

  public async Task GenererEcheancesManquantesAsync(
    DateTime dateReference) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    List<int> bailIds =
        await db.Baux
            .Where(bail => bail.EstActif)
            .Select(bail => bail.Id)
            .ToListAsync();

    foreach (int bailId in bailIds) {

      await GenererEcheanceAsync(
          bailId,
          dateReference);
    }
  }

  public async Task<bool> AddPaiementAsync(
    Paiement paiement) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    Echeance? echeance =
        await db.Echeances
            .FirstOrDefaultAsync(
                e => e.Id == paiement.EcheanceId);

    if (echeance == null ||
        paiement.Montant <= 0)
      return false;


    decimal montantDejaPaye =
        await db.Paiements
            .Where(
                p => p.EcheanceId ==
                     paiement.EcheanceId)
            .SumAsync(
                p => (decimal?)p.Montant)
        ?? 0m;


    decimal resteAPayer =
        echeance.Total - montantDejaPaye;

    // Échéance déjà soldée.
    if (resteAPayer <= 0)
      return false;

    // On refuse pour l'instant les trop-perçus.
    if (paiement.Montant > resteAPayer)
      return false;


    db.Paiements.Add(paiement);


    decimal totalApresPaiement =
        montantDejaPaye +
        paiement.Montant;


    if (totalApresPaiement >= echeance.Total) {

      echeance.Statut =
          StatusEcheance.Payee;
    }
    else {

      echeance.Statut =
          echeance.DateEcheance < DateTime.Today
              ? StatusEcheance.EnRetard
              : echeance.DateEcheance == DateTime.Today
                  ? StatusEcheance.APayer
                  : StatusEcheance.Avenir;
    }


    await db.SaveChangesAsync();

    return true;
  }

  public async Task<List<Paiement>> GetPaiementsAsync(int echeanceId) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Paiements
        .AsNoTracking()
        .Where(
            paiement =>
                paiement.EcheanceId == echeanceId)
        .OrderByDescending(
            paiement =>
                paiement.DatePaiement)
        .ToListAsync();
  }

  public async Task<List<Paiement>> GetPaiementsAsync() {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Paiements
        .AsNoTracking()
        .Include(paiement => paiement.Echeance)
        .OrderByDescending(
            paiement => paiement.DatePaiement)
        .ToListAsync();
  }

  public async Task<Echeance?> GetEcheanceAsync(int id) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Echeances
        .AsNoTracking()
        .Include(echeance => echeance.Bien)
        .Include(echeance => echeance.Locataire)
        .FirstOrDefaultAsync(
            echeance => echeance.Id == id);
  }

  public async Task<List<Echeance>>
    GetEcheancesPayeesAsync() {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Echeances
        .AsNoTracking()
        .Include(echeance => echeance.Bien)
        .Include(echeance => echeance.Locataire)
        .Include(echeance => echeance.Bail)
        .Where(echeance =>
            echeance.Statut == StatusEcheance.Payee)
        .OrderByDescending(
            echeance => echeance.DateEcheance)
        .ToListAsync();
  }

  public async Task<Bailleur?> GetBailleurAsync() {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    return await db.Bailleurs
        .AsNoTracking()
        .FirstOrDefaultAsync();
  }


  public async Task SaveBailleurAsync(
      Bailleur bailleur) {

    await using ApplicationDbContext db =
        await _dbFactory.CreateDbContextAsync();

    Bailleur? bailleurExistant =
        await db.Bailleurs
            .FirstOrDefaultAsync();

    if (bailleurExistant == null) {

      db.Bailleurs.Add(bailleur);
    }
    else {

      bailleurExistant.Prenom =
          bailleur.Prenom;

      bailleurExistant.Nom =
          bailleur.Nom;

      bailleurExistant.Adresse =
          bailleur.Adresse;

      bailleurExistant.CodePostal =
          bailleur.CodePostal;

      bailleurExistant.Ville =
          bailleur.Ville;

      bailleurExistant.Email =
          bailleur.Email;

      bailleurExistant.Telephone =
          bailleur.Telephone;
    }

    await db.SaveChangesAsync();
  }
}