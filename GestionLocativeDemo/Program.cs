using GestionLocativeDemo.Components;
using GestionLocativeDemo.Components.Account;
using GestionLocativeDemo.Data;
using GestionLocativeDemo.Models;
using GestionLocativeDemo.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License =
    QuestPDF.Infrastructure.LicenseType.Community;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<QuittancePdfService>();
builder.Services.AddScoped<AvisEcheancePdfService>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider,IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options => {
  options.DefaultScheme = IdentityConstants.ApplicationScheme;
  options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => {
  options.SignIn.RequireConfirmedAccount = true;
  options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>,IdentityNoOpEmailSender>();
builder.Services.AddScoped<DemoDataService>();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope()) {

  DemoDataService demoData =
      scope.ServiceProvider.GetRequiredService<DemoDataService>();

  await demoData.SeedPropertiesAsync();
  await demoData.SeedLocatairesAsync();

  await demoData.GenererEcheancesManquantesAsync(
    DateTime.Today);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseMigrationsEndPoint();
}
else {
  app.UseExceptionHandler("/Error",createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found",createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapGet(
    "/api/documents/quittance/{echeanceId:int}",
    async Task<IResult> (
        int echeanceId,
        DemoDataService demoData,
        QuittancePdfService pdfService) => {

          Bailleur? bailleur =
              await demoData.GetBailleurAsync();

          if (bailleur == null)
            return Results.BadRequest(
                "Le profil bailleur n'est pas renseigné.");


          Echeance? echeance =
              await demoData.GetEcheanceAsync(
                  echeanceId);

          if (echeance == null)
            return Results.NotFound(
                "Échéance introuvable.");


          List<Paiement> paiements =
              await demoData.GetPaiementsAsync(
                  echeanceId);

          decimal montantPaye =
              paiements.Sum(
                  paiement =>
                      paiement.Montant);


          if (montantPaye < echeance.Total)
            return Results.BadRequest(
                "La quittance ne peut être générée : " +
                "l'échéance n'est pas entièrement réglée.");


          byte[] pdf =
              pdfService.Generer(
                  bailleur,
                  echeance);


          string nomFichier =
              $"Quittance_" +
              $"{echeance.Locataire.Nom}_" +
              $"{echeance.DateEcheance:yyyy-MM}.pdf";


          return Results.File(
              pdf,
              contentType: "application/pdf",
              fileDownloadName: nomFichier);
        });
app.MapGet(
    "/api/documents/avis-echeance/{echeanceId:int}",
    async Task<IResult> (
        int echeanceId,
        DemoDataService demoData,
        AvisEcheancePdfService pdfService) => {

          Bailleur? bailleur =
              await demoData.GetBailleurAsync();

          if (bailleur == null)
            return Results.BadRequest(
                "Le profil bailleur n'est pas renseigné.");


          Echeance? echeance =
              await demoData.GetEcheanceAsync(
                  echeanceId);

          if (echeance == null)
            return Results.NotFound(
                "Échéance introuvable.");


          List<Paiement> paiements =
              await demoData.GetPaiementsAsync(
                  echeanceId);

          decimal montantPaye =
              paiements.Sum(
                  paiement =>
                      paiement.Montant);

          decimal reste =
              echeance.Total -
              montantPaye;


          if (reste <= 0)
            return Results.BadRequest(
                "Cette échéance est entièrement réglée. " +
                "Une quittance doit être générée.");


          byte[] pdf =
              pdfService.Generer(
                  bailleur,
                  echeance,
                  montantPaye);


          string nomFichier =
              $"Avis_Echeance_" +
              $"{echeance.Locataire.Nom}_" +
              $"{echeance.DateEcheance:yyyy-MM}.pdf";


          return Results.File(
              pdf,
              contentType: "application/pdf",
              fileDownloadName: nomFichier);
        });

app.Run();
