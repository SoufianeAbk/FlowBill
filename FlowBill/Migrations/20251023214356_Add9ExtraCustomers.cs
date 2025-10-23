using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowBill.Migrations
{
    /// <inheritdoc />
    public partial class Add9ExtraCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Klanten",
                columns: new[] { "Id", "AangemaaktOp", "Adres", "BTWNummer", "Bedrijfsnaam", "Contactpersoon", "Email", "KVKNummer", "Postcode", "Stad", "Telefoon" },
                values: new object[,]
                {
                    { 2, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Innovatielaan 45", "NL987654321B02", "TechSolutions Nederland", "Sophie de Vries", "sophie@techsolutions.nl", "87654321", "3045 BC", "Rotterdam", "0687654321" },
                    { 3, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marktplein 12", "NL234567890B03", "Bakkerij Van Dam", "Pieter van Dam", "pieter@bakkerijvandam.nl", "23456789", "4567 CD", "Utrecht", "0623456789" },
                    { 4, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Duurzaamweg 78", "NL345678901B04", "Green Energy Solutions", "Emma Mulder", "emma@greenenergy.nl", "34567890", "2345 DE", "Den Haag", "0634567890" },
                    { 5, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Businesspark 100", "NL456789012B05", "Consultancy Partners", "Marco Visser", "marco@consultancy.nl", "45678901", "5678 EF", "Eindhoven", "0645678901" },
                    { 6, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Designstraat 23", "NL567890123B06", "Creative Media Group", "Lisa Bakker", "lisa@creativemedia.nl", "56789012", "6789 FG", "Groningen", "0656789012" },
                    { 7, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kraanweg 56", "NL678901234B07", "Bouwbedrijf Hendriks", "Tom Hendriks", "tom@bouwbedrijf.nl", "67890123", "7890 GH", "Breda", "0667890123" },
                    { 8, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marktstraat 8", "NL789012345B08", "Restaurant De Gouden Lepel", "Anna de Wit", "anna@goudenlepel.nl", "78901234", "8901 HI", "Maastricht", "0678901234" },
                    { 9, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sportlaan 99", "NL890123456B09", "Fitness First Nederland", "David Peters", "david@fitnessfirst.nl", "89012345", "9012 IJ", "Nijmegen", "0689012345" },
                    { 10, new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Digitalweg 200", "NL901234567B10", "Online Marketing Pro", "Sarah van Leeuwen", "sarah@onlinemarketing.nl", "90123456", "1012 JK", "Amsterdam", "0690123456" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
