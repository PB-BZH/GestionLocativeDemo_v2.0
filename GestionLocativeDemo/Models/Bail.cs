using Microsoft.EntityFrameworkCore;

namespace GestionLocativeDemo.Models;

public enum TypeBail {
  HabitationNue,
  HabitationMeublee,
  Etudiant,
  Mobilite
}

public sealed class Bail {

  public int Id { get; set; }

  public int BienId { get; set; }

  public Bien Bien { get; set; } = null!;

  public int LocataireId { get; set; }

  public Locataire Locataire { get; set; } = null!;

  public TypeBail Type { get; set; }

  public DateTime DateDebut { get; set; }

  public int? DureeMois { get; set; }

  [Precision(18,2)]
  public decimal Loyer { get; set; }

  [Precision(18,2)]
  public decimal Charges { get; set; }

  [Precision(18,2)]
  public decimal DepotGarantie { get; set; }

  public int JourPaiement { get; set; }

  public bool EstActif { get; set; }
}