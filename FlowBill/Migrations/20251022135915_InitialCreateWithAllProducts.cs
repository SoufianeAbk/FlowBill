using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowBill.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithAllProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Producten",
                columns: new[] { "Id", "AangemaaktOp", "BTWPercentage", "IsActief", "Naam", "Omschrijving", "Prijs", "SKU", "Voorraad" },
                values: new object[,]
                {
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "The Last of Us Part II", "Spannend actie-avonturen game voor PlayStation 5", 59.99m, "GAME001", 45 },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "FIFA 25", "Populaire voetbalsimulatie game", 69.99m, "GAME002", 60 },
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Minecraft", "Sandbox bouw- en survival game", 29.99m, "GAME003", 120 },
                    { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Call of Duty: Modern Warfare III", "Eerste persoons shooter game", 69.99m, "GAME004", 35 },
                    { 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Zelda: Tears of the Kingdom", "Avonturen game voor Nintendo Switch", 59.99m, "GAME005", 50 },
                    { 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "God of War Ragnarök", "Mythologische actie-avonturen game", 59.99m, "GAME006", 40 },
                    { 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Mario Kart 8 Deluxe", "Racing game voor Nintendo Switch", 49.99m, "GAME007", 75 },
                    { 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Elden Ring", "Action RPG game", 49.99m, "GAME008", 55 },
                    { 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "iPhone 15 Pro Max 256GB", "Premium smartphone van Apple", 1349.00m, "GSM001", 25 },
                    { 14, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Samsung Galaxy S24 Ultra 512GB", "Krachtige Android smartphone", 1299.00m, "GSM002", 30 },
                    { 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Google Pixel 8 Pro 128GB", "Smartphone met pure Android ervaring", 899.00m, "GSM003", 40 },
                    { 16, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "OnePlus 12 256GB", "Snelle en betaalbare flagship smartphone", 799.00m, "GSM004", 35 },
                    { 17, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "iPhone 14 128GB", "Vorige generatie iPhone, nog steeds populair", 799.00m, "GSM005", 50 },
                    { 18, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Xiaomi 14 Pro 256GB", "Betaalbare smartphone met goede specs", 649.00m, "GSM006", 45 },
                    { 19, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Samsung Galaxy A54 128GB", "Mid-range smartphone met goede features", 449.00m, "GSM007", 60 },
                    { 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, true, "Atomic Habits - James Clear", "Bestseller over het opbouwen van goede gewoonten", 24.99m, "BOOK001", 80 },
                    { 21, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, true, "Het Diner - Herman Koch", "Nederlandse thriller roman", 19.99m, "BOOK002", 65 },
                    { 22, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, true, "Sapiens - Yuval Noah Harari", "Geschiedenis van de mensheid", 29.99m, "BOOK003", 70 },
                    { 23, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, true, "De Meeste Mensen Deugen - Rutger Bregman", "Optimistisch perspectief op de mensheid", 22.99m, "BOOK004", 55 },
                    { 24, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, true, "1984 - George Orwell", "Klassieke dystopische roman", 14.99m, "BOOK005", 90 },
                    { 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, true, "Clean Code - Robert Martin", "Programmeerboek over code kwaliteit", 49.99m, "BOOK006", 40 },
                    { 26, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, true, "Harry Potter - Volledige set", "Complete Harry Potter boekenreeks", 89.99m, "BOOK007", 35 },
                    { 27, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, true, "De Alchemist - Paulo Coelho", "Inspirerende roman over zelfontdekking", 19.99m, "BOOK008", 60 },
                    { 28, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "MacBook Pro 14 inch M3 Pro", "Professionele laptop voor creators", 2399.00m, "COMP001", 20 },
                    { 29, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Dell XPS 15", "Krachtige Windows laptop", 1799.00m, "COMP002", 25 },
                    { 30, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Lenovo ThinkPad X1 Carbon", "Business laptop met lange batterijduur", 1599.00m, "COMP003", 30 },
                    { 31, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "ASUS ROG Strix Gaming Laptop", "Gaming laptop met RTX 4070", 1899.00m, "COMP004", 18 },
                    { 32, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "HP Pavilion 15", "Betaalbare laptop voor dagelijks gebruik", 699.00m, "COMP005", 45 },
                    { 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Sony WH-1000XM5", "Premium noise-cancelling headphones", 379.00m, "AUDIO001", 50 },
                    { 34, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Apple AirPods Pro (2e generatie)", "Draadloze oordopjes met ANC", 279.00m, "AUDIO002", 65 },
                    { 35, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Bose QuietComfort 45", "Comfortabele noise-cancelling headphones", 329.00m, "AUDIO003", 40 },
                    { 36, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "JBL Flip 6", "Draagbare Bluetooth speaker", 129.00m, "AUDIO004", 75 },
                    { 37, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Sonos One SL", "Smart speaker voor thuis", 199.00m, "AUDIO005", 55 },
                    { 38, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "iPad Pro 12.9 inch 256GB", "Professionele tablet met M2 chip", 1349.00m, "TAB001", 30 },
                    { 39, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Samsung Galaxy Tab S9", "Premium Android tablet", 899.00m, "TAB002", 35 },
                    { 40, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "iPad Air 10.9 inch 128GB", "Veelzijdige tablet voor werk en vrije tijd", 699.00m, "TAB003", 45 },
                    { 41, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Amazon Fire HD 10", "Betaalbare tablet voor entertainment", 159.00m, "TAB004", 70 },
                    { 42, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Apple Watch Series 9", "Smartwatch met gezondheidsmonitoring", 449.00m, "WATCH001", 55 },
                    { 43, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Samsung Galaxy Watch 6", "Android smartwatch met vele functies", 329.00m, "WATCH002", 60 },
                    { 44, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Garmin Forerunner 265", "Sporthorloge voor hardlopers", 449.00m, "WATCH003", 40 },
                    { 45, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Fitbit Charge 6", "Fitness tracker met hartslagmeter", 159.00m, "WATCH004", 80 },
                    { 46, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Sony A7 IV", "Full-frame mirrorless camera", 2699.00m, "CAM001", 15 },
                    { 47, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Canon EOS R6 Mark II", "Veelzijdige full-frame camera", 2499.00m, "CAM002", 18 },
                    { 48, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "DJI Mini 4 Pro", "Compacte drone met 4K camera", 759.00m, "CAM003", 30 },
                    { 49, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "GoPro Hero 12 Black", "Actiecamera voor extreme sporten", 449.00m, "CAM004", 50 },
                    { 50, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "LG UltraGear 27 inch 4K Gaming Monitor", "Gaming monitor met hoge refresh rate", 699.00m, "MON001", 25 },
                    { 51, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Dell UltraSharp 32 inch 4K Monitor", "Professioneel beeldscherm voor werk", 849.00m, "MON002", 30 },
                    { 52, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Samsung Odyssey G7", "Curved gaming monitor", 599.00m, "MON003", 35 },
                    { 53, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "PlayStation 5", "Next-gen gaming console", 549.00m, "CONS001", 40 },
                    { 54, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Xbox Series X", "Krachtige gaming console van Microsoft", 499.00m, "CONS002", 35 },
                    { 55, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Nintendo Switch OLED", "Hybride gaming console", 349.00m, "CONS003", 50 },
                    { 56, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "PlayStation VR2", "Virtual reality headset voor PS5", 549.00m, "CONS004", 20 },
                    { 57, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Xbox Elite Controller Series 2", "Premium gaming controller", 179.00m, "CONS005", 45 },
                    { 58, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Microsoft Office 365 Personal - 1 jaar", "Productiviteitssoftware abonnement", 69.00m, "SOFT001", 200 },
                    { 59, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Adobe Creative Cloud - 1 jaar", "Complete suite voor creatieven", 719.00m, "SOFT002", 150 },
                    { 60, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Norton 360 Deluxe - 1 jaar", "Antivirus en beveiligingssoftware", 39.99m, "SOFT003", 180 },
                    { 61, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Logitech MX Master 3S", "Draadloze premium muis", 109.00m, "ACC001", 70 },
                    { 62, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Keychron K8 Pro Mechanical Keyboard", "Draadloos mechanisch toetsenbord", 119.00m, "ACC002", 55 },
                    { 63, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Anker PowerCore 20000mAh", "Draagbare powerbank", 49.99m, "ACC003", 90 },
                    { 64, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "SanDisk Extreme Pro 1TB SSD", "Externe SSD voor opslag", 149.00m, "ACC004", 60 },
                    { 65, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, true, "Webcam Logitech C920", "Full HD webcam voor videobellen", 79.00m, "ACC005", 65 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 6);

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

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 65);
        }
    }
}
