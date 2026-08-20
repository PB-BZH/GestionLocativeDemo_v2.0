namespace GestionLocativeDemo.Models;

public sealed class Locataire {

  public int Id { get; set; }

  public string Prenom { get; set; } = "";

  public string Nom { get; set; } = "";

  public string Adresse { get; set; } = "";

  public string Email { get; set; } = "";

  public string Telephone { get; set; } = "";

  public string FullName =>
      $"{Prenom} {Nom}";
}