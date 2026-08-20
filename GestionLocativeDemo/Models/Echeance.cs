using Microsoft.EntityFrameworkCore;

namespace GestionLocativeDemo.Models;

public enum StatusEcheance {
  Avenir = 0,
  APayer = 1,
  Payee = 2,
  EnRetard = 3
}

public sealed class Echeance {

  public int Id { get; set; }

  public int BienId { get; set; }

  public Bien Bien { get; set; } = null!;

  public int LocataireId { get; set; }

  public Locataire Locataire { get; set; } = null!;

  public DateTime DateEcheance { get; set; }

  [Precision(18,2)]
  public decimal Loyer { get; set; }

  [Precision(18,2)]
  public decimal Charges { get; set; }

  public StatusEcheance Statut { get; set; }

  public decimal Total =>
      Loyer + Charges;
}