namespace GestionLocativeDemo.Models;

public sealed class Property {

  public int Id { get; set; }

  public string Type { get; set; } = "";

  public double Surface { get; set; }

  public int? Floor { get; set; }

  public string LotNumber { get; set; } = "";

  public string Address { get; set; } = "";

  public string PostalCode { get; set; } = "";

  public string City { get; set; } = "";

  public string FiscalIdentifier { get; set; } = "";

  public decimal Rent { get; set; }

  public decimal Charges { get; set; }

  public bool IsRented { get; set; }
}