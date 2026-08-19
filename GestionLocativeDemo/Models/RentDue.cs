namespace GestionLocativeDemo.Models;

public enum RentDueStatus {
  Upcoming,
  ToPay,
  Paid,
  Late
}

public sealed class RentDue {

  public int Id { get; set; }

  public int PropertyId { get; set; }

  public int TenantId { get; set; }

  public DateTime DueDate { get; set; }

  public decimal Rent { get; set; }

  public decimal Charges { get; set; }

  public RentDueStatus Status { get; set; }

  public decimal Total =>
      Rent + Charges;
}