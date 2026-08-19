namespace GestionLocativeDemo.Models;

public sealed class Tenant {

  public int Id { get; set; }

  public string FirstName { get; set; } = "";

  public string LastName { get; set; } = "";

  public string Address { get; set; } = "";

  public string Email { get; set; } = "";

  public string Phone { get; set; } = "";

  public string FullName =>
      $"{FirstName} {LastName}";
}