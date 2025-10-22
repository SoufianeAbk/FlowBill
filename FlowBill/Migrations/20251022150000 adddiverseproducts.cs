using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowBill.Migrations
{
    /// <inheritdoc />
    public partial class AddDiverseProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Games
            migrationBuilder.InsertData(
                table: "Producten",
                columns: new[] { "Id", "Naam", "Omschrijving", "Prijs", "BTWPercentage", "Voorraad", "SKU", "IsActief", "AangemaaktOp" },
                values: new object[,]
                {
                    { 5, "The Last of Us Part II", "Spannend actie-avonturen game voor PlayStation 5", 59.99m, 21, 45, "GAME001", true, new DateTime(2024, 1, 1) },
                    { 6, "FIFA 25", "Populaire voetbalsimulatie game", 69.99m, 21, 60, "GAME002", true, new DateTime(2024, 1, 1) },
                    { 7, "Minecraft", "Sandbox bouw- en survival game", 29.99m, 21, 120, "GAME003", true, new DateTime(2024, 1, 1) },
                    { 8, "Call of Duty: Modern Warfare III", "Eerste persoons shooter game", 69.99m, 21, 35, "GAME004", true, new DateTime(2024, 1, 1) },
                    { 9, "Zelda: Tears of the Kingdom", "Avonturen game voor Nintendo Switch", 59.99m, 21, 50, "GAME005", true, new DateTime(2024, 1, 1) },
                    { 10, "God of War Ragnarök", "Mythologische actie-avonturen game", 59.99m, 21, 40, "GAME006", true, new DateTime(2024, 1, 1) },
                    { 11, "Mario Kart 8 Deluxe", "Racing game voor Nintendo Switch", 49.99m, 21, 75, "GAME007", true, new DateTime(2024, 1, 1) },
                    { 12, "Elden Ring", "Action RPG game", 49.99m, 21, 55, "GAME008", true, new DateTime(2024, 1, 1) },

                    // Smartphones
                    { 13, "iPhone 15 Pro Max 256GB", "Premium smartphone van Apple", 1349.00m, 21, 25, "GSM001", true, new DateTime(2024, 1, 1) },
                    { 14, "Samsung Galaxy S24 Ultra 512GB", "Krachtige Android smartphone", 1299.00m, 21, 30, "GSM002", true, new DateTime(2024, 1, 1) },
                    { 15, "Google Pixel 8 Pro 128GB", "Smartphone met pure Android ervaring", 899.00m, 21, 40, "GSM003", true, new DateTime(2024, 1, 1) },
                    { 16, "OnePlus 12 256GB", "Snelle en betaalbare flagship smartphone", 799.00m, 21, 35, "GSM004", true, new DateTime(2024, 1, 1) },
                    { 17, "iPhone 14 128GB", "Vorige generatie iPhone, nog steeds populair", 799.00m, 21, 50, "GSM005", true, new DateTime(2024, 1, 1) },
                    { 18, "Xiaomi 14 Pro 256GB", "Betaalbare smartphone met goede specs", 649.00m, 21, 45, "GSM006", true, new DateTime(2024, 1, 1) },
                    { 19, "Samsung Galaxy A54 128GB", "Mid-range smartphone met goede features", 449.00m, 21, 60, "GSM007", true, new DateTime(2024, 1, 1) },

                    // Boeken
                    { 20, "Atomic Habits - James Clear", "Bestseller over het opbouwen van goede gewoonten", 24.99m, 9, 80, "BOOK001", true, new DateTime(2024, 1, 1) },
                    { 21, "Het Diner - Herman Koch", "Nederlandse thriller roman", 19.99m, 9, 65, "BOOK002", true, new DateTime(2024, 1, 1) },
                    { 22, "Sapiens - Yuval Noah Harari", "Geschiedenis van de mensheid", 29.99m, 9, 70, "BOOK003", true, new DateTime(2024, 1, 1) },
                    { 23, "De Meeste Mensen Deugen - Rutger Bregman", "Optimistisch perspectief op de mensheid", 22.99m, 9, 55, "BOOK004", true, new DateTime(2024, 1, 1) },
                    { 24, "1984 - George Orwell", "Klassieke dystopische roman", 14.99m, 9, 90, "BOOK005", true, new DateTime(2024, 1, 1) },
                    { 25, "Clean Code - Robert Martin", "Programmeerboek over code kwaliteit", 49.99m, 9, 40, "BOOK006", true, new DateTime(2024, 1, 1) },
                    { 26, "Harry Potter - Volledige set", "Complete Harry Potter boekenreeks", 89.99m, 9, 35, "BOOK007", true, new DateTime(2024, 1, 1) },
                    { 27, "De Alchemist - Paulo Coelho", "Inspirerende roman over zelfontdekking", 19.99m, 9, 60, "BOOK008", true, new DateTime(2024, 1, 1) },

                    // Laptops & Computers
                    { 28, "MacBook Pro 14 inch M3 Pro", "Professionele laptop voor creators", 2399.00m, 21, 20, "COMP001", true, new DateTime(2024, 1, 1) },
                    { 29, "Dell XPS 15", "Krachtige Windows laptop", 1799.00m, 21, 25, "COMP002", true, new DateTime(2024, 1, 1) },
                    { 30, "Lenovo ThinkPad X1 Carbon", "Business laptop met lange batterijduur", 1599.00m, 21, 30, "COMP003", true, new DateTime(2024, 1, 1) },
                    { 31, "ASUS ROG Strix Gaming Laptop", "Gaming laptop met RTX 4070", 1899.00m, 21, 18, "COMP004", true, new DateTime(2024, 1, 1) },
                    { 32, "HP Pavilion 15", "Betaalbare laptop voor dagelijks gebruik", 699.00m, 21, 45, "COMP005", true, new DateTime(2024, 1, 1) },

                    // Audio & Headphones
                    { 33, "Sony WH-1000XM5", "Premium noise-cancelling headphones", 379.00m, 21, 50, "AUDIO001", true, new DateTime(2024, 1, 1) },
                    { 34, "Apple AirPods Pro (2e generatie)", "Draadloze oordopjes met ANC", 279.00m, 21, 65, "AUDIO002", true, new DateTime(2024, 1, 1) },
                    { 35, "Bose QuietComfort 45", "Comfortabele noise-cancelling headphones", 329.00m, 21, 40, "AUDIO003", true, new DateTime(2024, 1, 1) },
                    { 36, "JBL Flip 6", "Draagbare Bluetooth speaker", 129.00m, 21, 75, "AUDIO004", true, new DateTime(2024, 1, 1) },
                    { 37, "Sonos One SL", "Smart speaker voor thuis", 199.00m, 21, 55, "AUDIO005", true, new DateTime(2024, 1, 1) },

                    // Tablets
                    { 38, "iPad Pro 12.9 inch 256GB", "Professionele tablet met M2 chip", 1349.00m, 21, 30, "TAB001", true, new DateTime(2024, 1, 1) },
                    { 39, "Samsung Galaxy Tab S9", "Premium Android tablet", 899.00m, 21, 35, "TAB002", true, new DateTime(2024, 1, 1) },
                    { 40, "iPad Air 10.9 inch 128GB", "Veelzijdige tablet voor werk en vrije tijd", 699.00m, 21, 45, "TAB003", true, new DateTime(2024, 1, 1) },
                    { 41, "Amazon Fire HD 10", "Betaalbare tablet voor entertainment", 159.00m, 21, 70, "TAB004", true, new DateTime(2024, 1, 1) },

                    // Smartwatches & Wearables
                    { 42, "Apple Watch Series 9", "Smartwatch met gezondheidsmonitoring", 449.00m, 21, 55, "WATCH001", true, new DateTime(2024, 1, 1) },
                    { 43, "Samsung Galaxy Watch 6", "Android smartwatch met vele functies", 329.00m, 21, 60, "WATCH002", true, new DateTime(2024, 1, 1) },
                    { 44, "Garmin Forerunner 265", "Sporthorloge voor hardlopers", 449.00m, 21, 40, "WATCH003", true, new DateTime(2024, 1, 1) },
                    { 45, "Fitbit Charge 6", "Fitness tracker met hartslagmeter", 159.00m, 21, 80, "WATCH004", true, new DateTime(2024, 1, 1) },

                    // Cameras & Photography
                    { 46, "Sony A7 IV", "Full-frame mirrorless camera", 2699.00m, 21, 15, "CAM001", true, new DateTime(2024, 1, 1) },
                    { 47, "Canon EOS R6 Mark II", "Veelzijdige full-frame camera", 2499.00m, 21, 18, "CAM002", true, new DateTime(2024, 1, 1) },
                    { 48, "DJI Mini 4 Pro", "Compacte drone met 4K camera", 759.00m, 21, 30, "CAM003", true, new DateTime(2024, 1, 1) },
                    { 49, "GoPro Hero 12 Black", "Actiecamera voor extreme sporten", 449.00m, 21, 50, "CAM004", true, new DateTime(2024, 1, 1) },

                    // Monitors & Displays
                    { 50, "LG UltraGear 27 inch 4K Gaming Monitor", "Gaming monitor met hoge refresh rate", 699.00m, 21, 25, "MON001", true, new DateTime(2024, 1, 1) },
                    { 51, "Dell UltraSharp 32 inch 4K Monitor", "Professioneel beeldscherm voor werk", 849.00m, 21, 30, "MON002", true, new DateTime(2024, 1, 1) },
                    { 52, "Samsung Odyssey G7", "Curved gaming monitor", 599.00m, 21, 35, "MON003", true, new DateTime(2024, 1, 1) },

                    // Gaming Consoles & Accessories
                    { 53, "PlayStation 5", "Next-gen gaming console", 549.00m, 21, 40, "CONS001", true, new DateTime(2024, 1, 1) },
                    { 54, "Xbox Series X", "Krachtige gaming console van Microsoft", 499.00m, 21, 35, "CONS002", true, new DateTime(2024, 1, 1) },
                    { 55, "Nintendo Switch OLED", "Hybride gaming console", 349.00m, 21, 50, "CONS003", true, new DateTime(2024, 1, 1) },
                    { 56, "PlayStation VR2", "Virtual reality headset voor PS5", 549.00m, 21, 20, "CONS004", true, new DateTime(2024, 1, 1) },
                    { 57, "Xbox Elite Controller Series 2", "Premium gaming controller", 179.00m, 21, 45, "CONS005", true, new DateTime(2024, 1, 1) },

                    // Software & Subscriptions
                    { 58, "Microsoft Office 365 Personal - 1 jaar", "Productiviteitssoftware abonnement", 69.00m, 21, 200, "SOFT001", true, new DateTime(2024, 1, 1) },
                    { 59, "Adobe Creative Cloud - 1 jaar", "Complete suite voor creatieven", 719.00m, 21, 150, "SOFT002", true, new DateTime(2024, 1, 1) },
                    { 60, "Norton 360 Deluxe - 1 jaar", "Antivirus en beveiligingssoftware", 39.99m, 21, 180, "SOFT003", true, new DateTime(2024, 1, 1) },

                    // Accessories & Peripherals
                    { 61, "Logitech MX Master 3S", "Draadloze premium muis", 109.00m, 21, 70, "ACC001", true, new DateTime(2024, 1, 1) },
                    { 62, "Keychron K8 Pro Mechanical Keyboard", "Draadloos mechanisch toetsenbord", 119.00m, 21, 55, "ACC002", true, new DateTime(2024, 1, 1) },
                    { 63, "Anker PowerCore 20000mAh", "Draagbare powerbank", 49.99m, 21, 90, "ACC003", true, new DateTime(2024, 1, 1) },
                    { 64, "SanDisk Extreme Pro 1TB SSD", "Externe SSD voor opslag", 149.00m, 21, 60, "ACC004", true, new DateTime(2024, 1, 1) },
                    { 65, "Webcam Logitech C920", "Full HD webcam voor videobellen", 79.00m, 21, 65, "ACC005", true, new DateTime(2024, 1, 1) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValues: new object[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65 });
        }
    }
}