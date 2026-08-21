using Microsoft.EntityFrameworkCore;

namespace GestionLocativeDemo.Models;

public enum ModePaiement {
  Virement,
  Prelevement,
  Cheque,
  Especes,
  Carte,
  Autre
}

public sealed class Paiement {

  public int Id { get; set; }

  public int EcheanceId { get; set; }

  public Echeance Echeance { get; set; } = null!;

  public DateTime DatePaiement { get; set; }

  [Precision(18,2)]
  public decimal Montant { get; set; }

  public ModePaiement Mode { get; set; }

  public string? Reference { get; set; }
}