using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionLocativeDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddBailToEcheances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BailId",
                table: "RentDues",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentDues_BailId",
                table: "RentDues",
                column: "BailId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentDues_Baux_BailId",
                table: "RentDues",
                column: "BailId",
                principalTable: "Baux",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentDues_Baux_BailId",
                table: "RentDues");

            migrationBuilder.DropIndex(
                name: "IX_RentDues_BailId",
                table: "RentDues");

            migrationBuilder.DropColumn(
                name: "BailId",
                table: "RentDues");
        }
    }
}
