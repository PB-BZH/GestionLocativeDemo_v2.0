using System.Globalization;
using GestionLocativeDemo.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GestionLocativeDemo.Services;

public sealed class QuittancePdfService {

  public byte[] Generer(
      Bailleur bailleur,
      Echeance echeance) {

    string numeroQuittance = $"Q-{echeance.DateEcheance:yyyy-MM}-{echeance.Id:D5}";
    CultureInfo culture = CultureInfo.GetCultureInfo("fr-FR");
    string periode = echeance.DateEcheance.ToString("MMMM yyyy",culture);
    string periodeTitre = char.ToUpper(periode[0],culture) + periode[1..];
    string periodeAvecPreposition = GetPeriodeAvecPreposition(echeance.DateEcheance,culture);
    Document document = Document.Create(container => {

      container.Page(page => {
        page.Size(PageSizes.A4);
        page.Margin(2,Unit.Centimetre);
        page.DefaultTextStyle(style => style.FontSize(11));

        page.Header()
            .Row(row => {

              row.RelativeItem()
                  .Column(column => {

                    column.Item()
                        .Text("QUITTANCE DE LOYER")
                        .FontSize(22)
                        .Bold();

                    column.Item()
                        .PaddingTop(4)
                        .Text(periodeTitre)
                        .FontSize(13)
                        .FontColor(Colors.Grey.Darken1);
                  });


              row.ConstantItem(160)
                  .AlignRight()
                  .Column(column => {

                    column.Item()
                        .Text("Référence")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken1);

                    column.Item()
                        .Text(numeroQuittance)
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
                        .Column(bailleurColumn => {

                          bailleurColumn.Item()
                              .Text("BAILLEUR")
                              .Bold();

                          bailleurColumn.Item()
                              .Text(bailleur.NomComplet);

                          bailleurColumn.Item()
                              .Text(bailleur.Adresse);

                          bailleurColumn.Item()
                              .Text(
                                  $"{bailleur.CodePostal} {bailleur.Ville}");

                          if (!string.IsNullOrWhiteSpace(
                                  bailleur.Email)) {

                            bailleurColumn.Item()
                                .Text(bailleur.Email);
                          }

                          if (!string.IsNullOrWhiteSpace(
                                  bailleur.Telephone)) {

                            bailleurColumn.Item()
                                .Text(bailleur.Telephone);
                          }
                        });


                    row.RelativeItem()
                        .Column(locataireColumn => {

                          locataireColumn.Item()
                              .Text("LOCATAIRE")
                              .Bold();

                          locataireColumn.Item()
                              .Text(
                                  echeance.Locataire.NomComplet);

                          locataireColumn.Item()
                              .Text(
                                  echeance.Locataire.Adresse);
                        });
                  });


              column.Item()
                  .LineHorizontal(1)
                  .LineColor(Colors.Grey.Lighten2);


              column.Item()
                  .Column(logement => {

                    logement.Item()
                        .Text("LOGEMENT")
                        .Bold();

                    logement.Item()
                        .Text(
                            echeance.Bien.TypeBien);

                    logement.Item()
                        .Text(
                            echeance.Bien.Adresse);

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
                  .Table(table => {

                    table.ColumnsDefinition(columns => {

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
                  });

              column.Item()
                  .Background(Colors.Grey.Lighten3)
                  .Border(1)
                  .BorderColor(Colors.Grey.Lighten1)
                  .Padding(12)
                  .Row(row => {

                    row.RelativeItem()
                        .Text("TOTAL ACQUITTÉ")
                        .Bold()
                        .FontSize(12);

                    row.ConstantItem(140)
                        .AlignRight()
                        .Text(
                            echeance.Total
                                .ToString("N2",culture) +
                            " €")
                        .Bold()
                        .FontSize(14);
                  });

              column.Item()
                  .PaddingTop(15)
                  .Text(text => {

                    text.Span(
                        $"Je soussigné {bailleur.NomComplet}, " +
                        $"bailleur du logement désigné ci-dessus, " +
                        $"déclare avoir reçu de " +
                        $"{echeance.Locataire.NomComplet} " +
                        $"la somme de ");

                    text.Span(
                            echeance.Total
                                .ToString("N2",culture) +
                            " €")
                        .Bold();

                    text.Span(
                        $" au titre du loyer et des charges " +
                        $"pour le mois {periodeAvecPreposition}.");
                  });

              column.Item()
                  .PaddingTop(10)
                  .Text(
                      "Cette quittance atteste du règlement intégral " +
                      "des sommes dues au titre du loyer et des charges " +
                      "pour la période indiquée.")
                  .FontSize(9)
                  .FontColor(Colors.Grey.Darken1);

              column.Item()
                  .PaddingTop(20)
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
                        .Text(bailleur.NomComplet)
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

  private static string GetPeriodeAvecPreposition(DateTime date,CultureInfo culture) {
    string periode = date.ToString("MMMM yyyy",culture);
    string mois = date.ToString("MMMM",culture);
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