using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionLocativeDemo.Migrations {
  /// <inheritdoc />
  public partial class AddBaux: Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "Baux",
          columns: table => new {
            Id = table.Column<int>(
                  type: "int",
                  nullable: false)
                  .Annotation(
                      "SqlServer:Identity",
                      "1, 1"),

            BienId = table.Column<int>(
                  type: "int",
                  nullable: false),

            LocataireId = table.Column<int>(
                  type: "int",
                  nullable: false),

            Type = table.Column<int>(
                  type: "int",
                  nullable: false),

            DateDebut = table.Column<DateTime>(
                  type: "datetime2",
                  nullable: false),

            DureeMois = table.Column<int>(
                  type: "int",
                  nullable: true),

            Loyer = table.Column<decimal>(
                  type: "decimal(18,2)",
                  precision: 18,
                  scale: 2,
                  nullable: false),

            Charges = table.Column<decimal>(
                  type: "decimal(18,2)",
                  precision: 18,
                  scale: 2,
                  nullable: false),

            DepotGarantie = table.Column<decimal>(
                  type: "decimal(18,2)",
                  precision: 18,
                  scale: 2,
                  nullable: false),

            JourPaiement = table.Column<int>(
                  type: "int",
                  nullable: false),

            EstActif = table.Column<bool>(
                  type: "bit",
                  nullable: false)
          },
          constraints: table => {
            table.PrimaryKey(
              "PK_Baux",
              x => x.Id);

            table.ForeignKey(
              name: "FK_Baux_Properties_BienId",
              column: x => x.BienId,
              principalTable: "Properties",
              principalColumn: "Id",
              onDelete: ReferentialAction.Restrict);

            table.ForeignKey(
              name: "FK_Baux_Tenants_LocataireId",
              column: x => x.LocataireId,
              principalTable: "Tenants",
              principalColumn: "Id",
              onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_Baux_BienId",
          table: "Baux",
          column: "BienId");

      migrationBuilder.CreateIndex(
          name: "IX_Baux_LocataireId",
          table: "Baux",
          column: "LocataireId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "Baux");
    }
  }
}
