using System.Globalization;
using GestionLocativeDemo.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GestionLocativeDemo.Services;

public sealed class AvisEcheancePdfService {

  public byte[] Generer(
      Bailleur bailleur,
      Echeance echeance,
      decimal montantDejaPaye) {

    CultureInfo culture =
        CultureInfo.GetCultureInfo("fr-FR");

    string periode =
        echeance.DateEcheance
            .ToString("MMMM yyyy",culture);

    string periodeAvecPreposition =
    GetPeriodeAvecPreposition(
        echeance.DateEcheance,
        culture);

    string periodeTitre =
        char.ToUpper(periode[0],culture) +
        periode[1..];

    decimal resteAPayer =
        Math.Max(
            0m,
            echeance.Total - montantDejaPaye);

    string reference =
        $"AE-{echeance.DateEcheance:yyyy-MM}-{echeance.Id:D5}";


    Document document =
        Document.Create(container => {

          container.Page(page => {

            page.Size(PageSizes.A4);

            page.Margin(
                2,
                Unit.Centimetre);

            page.DefaultTextStyle(
                style =>
                    style.FontSize(11));


            page.Header()
                .Row(row => {

                  row.RelativeItem()
                      .Column(column => {

                        column.Item()
                            .Text("AVIS D'ÉCHÉANCE")
                            .FontSize(22)
                            .Bold();

                        column.Item()
                            .PaddingTop(4)
                            .Text(periodeTitre)
                            .FontSize(13)
                            .FontColor(
                                Colors.Grey.Darken1);
                      });


                  row.ConstantItem(160)
                      .AlignRight()
                      .Column(column => {

                        column.Item()
                            .Text("Référence")
                            .FontSize(9)
                            .FontColor(
                                Colors.Grey.Darken1);

                        column.Item()
                            .Text(reference)
                            .FontSize(10)
                            .Bold();
                      });
                });


            page.Content()
                .PaddingVertical(25)
                .Column(column => {

                  column.Spacing(18);


                  column.Item()
                      .Row(row => {

                        row.RelativeItem()
                            .Column(info => {

                              info.Item()
                                  .Text("BAILLEUR")
                                  .Bold();

                              info.Item()
                                  .Text(
                                      bailleur.NomComplet);

                              info.Item()
                                  .Text(
                                      bailleur.Adresse);

                              info.Item()
                                  .Text(
                                      $"{bailleur.CodePostal} " +
                                      $"{bailleur.Ville}");

                              if (!string.IsNullOrWhiteSpace(
                                      bailleur.Email)) {

                                info.Item()
                                    .Text(
                                        bailleur.Email);
                              }
                            });


                        row.RelativeItem()
                            .Column(info => {

                              info.Item()
                                  .Text("LOCATAIRE")
                                  .Bold();

                              info.Item()
                                  .Text(
                                      echeance
                                          .Locataire
                                          .NomComplet);

                              info.Item()
                                  .Text(
                                      echeance
                                          .Locataire
                                          .Adresse);
                            });
                      });


                  column.Item()
                      .LineHorizontal(1)
                      .LineColor(
                          Colors.Grey.Lighten2);


                  column.Item()
                      .Column(logement => {

                        logement.Item()
                            .Text("LOGEMENT")
                            .Bold();

                        logement.Item()
                            .Text(
                                echeance
                                    .Bien
                                    .TypeBien);

                        logement.Item()
                            .Text(
                                echeance
                                    .Bien
                                    .Adresse);

                        logement.Item()
                            .Text(
                                $"{echeance.Bien.CodePostal} " +
                                $"{echeance.Bien.Ville}");
                      });


                  column.Item()
                      .Text(
                          $"Période concernée : {periode}")
                      .Bold();


                  column.Item()
                    .Text(
                        $"Date d'échéance : " +
                        $"{echeance.DateEcheance:dd/MM/yyyy}")
                    .Bold();

                  column.Item()
                      .Table(table => {

                        table.ColumnsDefinition(
                            columns => {

                              columns.RelativeColumn();
                              columns.ConstantColumn(120);
                            });


                        table.Cell()
                            .PaddingVertical(6)
                            .Text("Loyer hors charges");

                        table.Cell()
                            .AlignRight()
                            .PaddingVertical(6)
                            .Text(
                                echeance.Loyer
                                    .ToString("N2",culture) +
                                " €");


                        table.Cell()
                            .PaddingVertical(6)
                            .Text("Charges");

                        table.Cell()
                            .AlignRight()
                            .PaddingVertical(6)
                            .Text(
                                echeance.Charges
                                    .ToString("N2",culture) +
                                " €");


                        if (montantDejaPaye > 0) {

                          table.Cell()
                              .PaddingVertical(6)
                              .Text("Déjà réglé");

                          table.Cell()
                              .AlignRight()
                              .PaddingVertical(6)
                              .Text(
                                  montantDejaPaye
                                      .ToString("N2",culture) +
                                  " €");
                        }
                      });


                  column.Item()
                      .Background(
                          Colors.Grey.Lighten3)
                      .Border(1)
                      .BorderColor(
                          Colors.Grey.Lighten1)
                      .Padding(12)
                      .Row(row => {

                        row.RelativeItem()
                            .Text("RESTE À RÉGLER")
                            .Bold()
                            .FontSize(12);

                        row.ConstantItem(140)
                            .AlignRight()
                            .Text(
                                resteAPayer
                                    .ToString("N2",culture) +
                                " €")
                            .Bold()
                            .FontSize(14);
                      });


                  column.Item()
                      .PaddingTop(15)
                      .Text(
                          $"Nous vous informons que la somme de " +
                          $"{resteAPayer.ToString("N2",culture)} € " +
                          $"reste due au titre du loyer et des charges " +
                          $"pour le mois {periodeAvecPreposition}.");


                  column.Item()
                      .PaddingTop(15)
                      .AlignRight()
                      .Column(signature => {

                        signature.Item()
                            .Text(
                                $"Fait à {bailleur.Ville},");

                        signature.Item()
                            .Text(
                                $"le {DateTime.Today:dd/MM/yyyy}");

                        signature.Item()
                            .PaddingTop(15)
                            .Text(
                                bailleur.NomComplet)
                            .Bold();
                      });
                });


            page.Footer()
                .AlignCenter()
                .Text(text => {

                  text.Span("Page ");
                  text.CurrentPageNumber();
                  text.Span(" / ");
                  text.TotalPages();
                });
          });
        });


    return document.GeneratePdf();
  }

  private static string GetPeriodeAvecPreposition(
    DateTime date,
    CultureInfo culture) {

    string periode =
        date.ToString("MMMM yyyy",culture);

    string mois =
        date.ToString("MMMM",culture);

    bool elision =
        mois.StartsWith("a",StringComparison.OrdinalIgnoreCase) ||
        mois.StartsWith("e",StringComparison.OrdinalIgnoreCase) ||
        mois.StartsWith("i",StringComparison.OrdinalIgnoreCase) ||
        mois.StartsWith("o",StringComparison.OrdinalIgnoreCase) ||
        mois.StartsWith("u",StringComparison.OrdinalIgnoreCase) ||
        mois.StartsWith("é",StringComparison.OrdinalIgnoreCase);

    return elision
        ? $"d'{periode}"
        : $"de {periode}";
  }
}