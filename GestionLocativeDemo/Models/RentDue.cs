using Microsoft.EntityFrameworkCore;

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

  public Property Property { get; set; } = null!;

  public int TenantId { get; set; }

  public Tenant Tenant { get; set; } = null!;

  public DateTime DueDate { get; set; }

  [Precision(18,2)]
  public decimal Rent { get; set; }

  [Precision(18,2)]
  public decimal Charges { get; set; }

  public RentDueStatus Status { get; set; }

  public decimal Total =>
      Rent + Charges;
}