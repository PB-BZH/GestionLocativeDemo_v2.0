using Microsoft.EntityFrameworkCore;

namespace GestionLocativeDemo.Models;

public sealed class Bien {

  public int Id { get; set; }

  public string TypeBien { get; set; } = "";

  public double Surface { get; set; }

  public int? Etage { get; set; }

  public string NumeroLot { get; set; } = "";

  public string Adresse { get; set; } = "";

  public string CodePostal { get; set; } = "";

  public string Ville { get; set; } = "";

  public string IdentifiantFiscal { get; set; } = "";

  [Precision(18,2)]
  public decimal Loyer { get; set; }

  [Precision(18,2)]
  public decimal Charges { get; set; }

  public bool EstLoue { get; set; }
}