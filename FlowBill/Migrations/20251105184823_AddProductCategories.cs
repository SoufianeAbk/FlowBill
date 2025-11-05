using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowBill.Migrations
{
    /// <inheritdoc />
    public partial class AddProductCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Producten",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActief = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "IconClass", "IsActief", "Naam", "Omschrijving" },
                values: new object[,]
                {
                    { 1, "fas fa-globe", true, "Webdiensten", "Professionele webdiensten en online oplossingen" },
                    { 2, "fas fa-gamepad", true, "Games", "Populaire videogames voor verschillende platforms" },
                    { 3, "fas fa-dumbbell", true, "Sport", "Sportartikelen en fitnessequipment" }
                });

            migrationBuilder.UpdateData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "Omschrijving" },
                values: new object[] { 1, "Complete website ontwikkeling op maat" });

            migrationBuilder.UpdateData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Omschrijving" },
                values: new object[] { 1, "Betrouwbare webhosting voor 1 jaar" });

            migrationBuilder.UpdateData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Omschrijving" },
                values: new object[] { 1, "Zoekmotor optimalisatie voor betere vindbaarheid" });

            migrationBuilder.UpdateData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Omschrijving" },
                values: new object[] { 1, "Professioneel logo ontwerp inclusief 3 concepten" });

            migrationBuilder.UpdateData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 5,
                column: "CategoryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CategoryId", "Omschrijving" },
                values: new object[] { 2, "De nieuwste voetbalsimulatie game" });

            migrationBuilder.InsertData(
                table: "Producten",
                columns: new[] { "Id", "AangemaaktOp", "BTWPercentage", "CategoryId", "IsActief", "Naam", "Omschrijving", "Prijs", "SKU", "Voorraad" },
                values: new object[,]
                {
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 2, true, "Red Dead Redemption 2", "Western actie-avontuur game voor meerdere platforms", 49.99m, "GAME003", 35 },
                    { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 2, true, "Mario Kart 8 Deluxe", "Populaire racegame voor Nintendo Switch", 54.99m, "GAME004", 50 },
                    { 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 3, true, "Yoga Mat Premium", "Extra dikke yogamat met anti-slip oppervlak (6mm)", 29.99m, "SPORT001", 75 },
                    { 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 3, true, "Dumbbells Set 2x10kg", "Verstelbare dumbbells set voor thuisfitness", 89.99m, "SPORT002", 40 },
                    { 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 3, true, "Hardloopschoenen Pro", "Professionele hardloopschoenen met extra demping", 129.99m, "SPORT003", 55 },
                    { 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 3, true, "Fitness Tracker Smart", "Slimme fitness tracker met hartslagmeter en GPS", 79.99m, "SPORT004", 65 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Producten_CategoryId",
                table: "Producten",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Producten_ProductCategories_CategoryId",
                table: "Producten",
                column: "CategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producten_ProductCategories_CategoryId",
                table: "Producten");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_Producten_CategoryId",
                table: "Producten");

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Producten");

            migrationBuilder.UpdateData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 1,
                column: "Omschrijving",
                value: "Complete website ontwikkeling");

            migrationBuilder.UpdateData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 2,
                column: "Omschrijving",
                value: "Webhosting voor 1 jaar");

            migrationBuilder.UpdateData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 3,
                column: "Omschrijving",
                value: "Zoekmotor optimalisatie");

            migrationBuilder.UpdateData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 4,
                column: "Omschrijving",
                value: "Professioneel logo ontwerp");

            migrationBuilder.UpdateData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 6,
                column: "Omschrijving",
                value: "Populaire voetbalsimulatie game");
        }
    }
}
