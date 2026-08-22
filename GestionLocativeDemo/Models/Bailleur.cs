namespace GestionLocativeDemo.Models;

public sealed class Bailleur {

  public int Id { get; set; }

  public string Prenom { get; set; } = "";

  public string Nom { get; set; } = "";

  public string Adresse { get; set; } = "";

  public string CodePostal { get; set; } = "";

  public string Ville { get; set; } = "";

  public string Email { get; set; } = "";

  public string Telephone { get; set; } = "";

  public string NomComplet =>
      $"{Prenom} {Nom}".Trim();
}