using FlowBill.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlowBill.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Product> Producten { get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<BestellingItem> BestellingItems { get; set; }
        public DbSet<Factuur> Facturen { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relatie configuratie
            modelBuilder.Entity<Bestelling>()
                .HasOne(b => b.Klant)
                .WithMany(k => k.Bestellingen)
                .HasForeignKey(b => b.KlantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bestelling>()
                .HasOne(b => b.Factuur)
                .WithOne(f => f.Bestelling)
                .HasForeignKey<Factuur>(f => f.BestellingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BestellingItem>()
                .HasOne(bi => bi.Bestelling)
                .WithMany(b => b.BestellingItems)
                .HasForeignKey(bi => bi.BestellingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BestellingItem>()
                .HasOne(bi => bi.Product)
                .WithMany(p => p.BestellingItems)
                .HasForeignKey(bi => bi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data - Products (keeping existing products)
            modelBuilder.Entity<Product>().HasData(
                // Oorspronkelijke producten
                new Product { Id = 1, Naam = "Website ontwikkeling", Omschrijving = "Complete website ontwikkeling", Prijs = 2500.00m, BTWPercentage = 21, Voorraad = 100, SKU = "WEB001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 2, Naam = "Hosting - Jaarlijks", Omschrijving = "Webhosting voor 1 jaar", Prijs = 120.00m, BTWPercentage = 21, Voorraad = 100, SKU = "HOST001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 3, Naam = "SEO Optimalisatie", Omschrijving = "Zoekmotor optimalisatie", Prijs = 750.00m, BTWPercentage = 21, Voorraad = 100, SKU = "SEO001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 4, Naam = "Logo ontwerp", Omschrijving = "Professioneel logo ontwerp", Prijs = 350.00m, BTWPercentage = 21, Voorraad = 100, SKU = "LOGO001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Games
                new Product { Id = 5, Naam = "The Last of Us Part II", Omschrijving = "Spannend actie-avonturen game voor PlayStation 5", Prijs = 59.99m, BTWPercentage = 21, Voorraad = 45, SKU = "GAME001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 6, Naam = "FIFA 25", Omschrijving = "Populaire voetbalsimulatie game", Prijs = 69.99m, BTWPercentage = 21, Voorraad = 60, SKU = "GAME002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 7, Naam = "Minecraft", Omschrijving = "Sandbox bouw- en survival game", Prijs = 29.99m, BTWPercentage = 21, Voorraad = 120, SKU = "GAME003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 8, Naam = "Call of Duty: Modern Warfare III", Omschrijving = "Eerste persoons shooter game", Prijs = 69.99m, BTWPercentage = 21, Voorraad = 35, SKU = "GAME004", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 9, Naam = "Zelda: Tears of the Kingdom", Omschrijving = "Avonturen game voor Nintendo Switch", Prijs = 59.99m, BTWPercentage = 21, Voorraad = 50, SKU = "GAME005", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 10, Naam = "God of War Ragnarök", Omschrijving = "Mythologische actie-avonturen game", Prijs = 59.99m, BTWPercentage = 21, Voorraad = 40, SKU = "GAME006", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 11, Naam = "Mario Kart 8 Deluxe", Omschrijving = "Racing game voor Nintendo Switch", Prijs = 49.99m, BTWPercentage = 21, Voorraad = 75, SKU = "GAME007", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 12, Naam = "Elden Ring", Omschrijving = "Action RPG game", Prijs = 49.99m, BTWPercentage = 21, Voorraad = 55, SKU = "GAME008", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Smartphones
                new Product { Id = 13, Naam = "iPhone 15 Pro Max 256GB", Omschrijving = "Premium smartphone van Apple", Prijs = 1349.00m, BTWPercentage = 21, Voorraad = 25, SKU = "GSM001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 14, Naam = "Samsung Galaxy S24 Ultra 512GB", Omschrijving = "Krachtige Android smartphone", Prijs = 1299.00m, BTWPercentage = 21, Voorraad = 30, SKU = "GSM002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 15, Naam = "Google Pixel 8 Pro 128GB", Omschrijving = "Smartphone met pure Android ervaring", Prijs = 899.00m, BTWPercentage = 21, Voorraad = 40, SKU = "GSM003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 16, Naam = "OnePlus 12 256GB", Omschrijving = "Snelle en betaalbare flagship smartphone", Prijs = 799.00m, BTWPercentage = 21, Voorraad = 35, SKU = "GSM004", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 17, Naam = "iPhone 14 128GB", Omschrijving = "Vorige generatie iPhone, nog steeds populair", Prijs = 799.00m, BTWPercentage = 21, Voorraad = 50, SKU = "GSM005", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 18, Naam = "Xiaomi 14 Pro 256GB", Omschrijving = "Betaalbare smartphone met goede specs", Prijs = 649.00m, BTWPercentage = 21, Voorraad = 45, SKU = "GSM006", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 19, Naam = "Samsung Galaxy A54 128GB", Omschrijving = "Mid-range smartphone met goede features", Prijs = 449.00m, BTWPercentage = 21, Voorraad = 60, SKU = "GSM007", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Boeken
                new Product { Id = 20, Naam = "Atomic Habits - James Clear", Omschrijving = "Bestseller over het opbouwen van goede gewoonten", Prijs = 24.99m, BTWPercentage = 9, Voorraad = 80, SKU = "BOOK001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 21, Naam = "Het Diner - Herman Koch", Omschrijving = "Nederlandse thriller roman", Prijs = 19.99m, BTWPercentage = 9, Voorraad = 65, SKU = "BOOK002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 22, Naam = "Sapiens - Yuval Noah Harari", Omschrijving = "Geschiedenis van de mensheid", Prijs = 29.99m, BTWPercentage = 9, Voorraad = 70, SKU = "BOOK003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 23, Naam = "De Meeste Mensen Deugen - Rutger Bregman", Omschrijving = "Optimistisch perspectief op de mensheid", Prijs = 22.99m, BTWPercentage = 9, Voorraad = 55, SKU = "BOOK004", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 24, Naam = "1984 - George Orwell", Omschrijving = "Klassieke dystopische roman", Prijs = 14.99m, BTWPercentage = 9, Voorraad = 90, SKU = "BOOK005", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 25, Naam = "Clean Code - Robert Martin", Omschrijving = "Programmeerboek over code kwaliteit", Prijs = 49.99m, BTWPercentage = 9, Voorraad = 40, SKU = "BOOK006", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 26, Naam = "Harry Potter - Volledige set", Omschrijving = "Complete Harry Potter boekenreeks", Prijs = 89.99m, BTWPercentage = 9, Voorraad = 35, SKU = "BOOK007", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 27, Naam = "De Alchemist - Paulo Coelho", Omschrijving = "Inspirerende roman over zelfontdekking", Prijs = 19.99m, BTWPercentage = 9, Voorraad = 60, SKU = "BOOK008", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Laptops & Computers
                new Product { Id = 28, Naam = "MacBook Pro 14 inch M3 Pro", Omschrijving = "Professionele laptop voor creators", Prijs = 2399.00m, BTWPercentage = 21, Voorraad = 20, SKU = "COMP001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 29, Naam = "Dell XPS 15", Omschrijving = "Krachtige Windows laptop", Prijs = 1799.00m, BTWPercentage = 21, Voorraad = 25, SKU = "COMP002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 30, Naam = "Lenovo ThinkPad X1 Carbon", Omschrijving = "Business laptop met lange batterijduur", Prijs = 1599.00m, BTWPercentage = 21, Voorraad = 30, SKU = "COMP003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 31, Naam = "ASUS ROG Strix Gaming Laptop", Omschrijving = "Gaming laptop met RTX 4070", Prijs = 1899.00m, BTWPercentage = 21, Voorraad = 18, SKU = "COMP004", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 32, Naam = "HP Pavilion 15", Omschrijving = "Betaalbare laptop voor dagelijks gebruik", Prijs = 699.00m, BTWPercentage = 21, Voorraad = 45, SKU = "COMP005", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Audio & Headphones
                new Product { Id = 33, Naam = "Sony WH-1000XM5", Omschrijving = "Premium noise-cancelling headphones", Prijs = 379.00m, BTWPercentage = 21, Voorraad = 50, SKU = "AUDIO001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 34, Naam = "Apple AirPods Pro (2e generatie)", Omschrijving = "Draadloze oordopjes met ANC", Prijs = 279.00m, BTWPercentage = 21, Voorraad = 65, SKU = "AUDIO002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 35, Naam = "Bose QuietComfort 45", Omschrijving = "Comfortabele noise-cancelling headphones", Prijs = 329.00m, BTWPercentage = 21, Voorraad = 40, SKU = "AUDIO003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 36, Naam = "JBL Flip 6", Omschrijving = "Draagbare Bluetooth speaker", Prijs = 129.00m, BTWPercentage = 21, Voorraad = 75, SKU = "AUDIO004", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 37, Naam = "Sonos One SL", Omschrijving = "Smart speaker voor thuis", Prijs = 199.00m, BTWPercentage = 21, Voorraad = 55, SKU = "AUDIO005", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Tablets
                new Product { Id = 38, Naam = "iPad Pro 12.9 inch 256GB", Omschrijving = "Professionele tablet met M2 chip", Prijs = 1349.00m, BTWPercentage = 21, Voorraad = 30, SKU = "TAB001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 39, Naam = "Samsung Galaxy Tab S9", Omschrijving = "Premium Android tablet", Prijs = 899.00m, BTWPercentage = 21, Voorraad = 35, SKU = "TAB002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 40, Naam = "iPad Air 10.9 inch 128GB", Omschrijving = "Veelzijdige tablet voor werk en vrije tijd", Prijs = 699.00m, BTWPercentage = 21, Voorraad = 45, SKU = "TAB003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 41, Naam = "Amazon Fire HD 10", Omschrijving = "Betaalbare tablet voor entertainment", Prijs = 159.00m, BTWPercentage = 21, Voorraad = 70, SKU = "TAB004", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Smartwatches & Wearables
                new Product { Id = 42, Naam = "Apple Watch Series 9", Omschrijving = "Smartwatch met gezondheidsmonitoring", Prijs = 449.00m, BTWPercentage = 21, Voorraad = 55, SKU = "WATCH001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 43, Naam = "Samsung Galaxy Watch 6", Omschrijving = "Android smartwatch met vele functies", Prijs = 329.00m, BTWPercentage = 21, Voorraad = 60, SKU = "WATCH002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 44, Naam = "Garmin Forerunner 265", Omschrijving = "Sporthorloge voor hardlopers", Prijs = 449.00m, BTWPercentage = 21, Voorraad = 40, SKU = "WATCH003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 45, Naam = "Fitbit Charge 6", Omschrijving = "Fitness tracker met hartslagmeter", Prijs = 159.00m, BTWPercentage = 21, Voorraad = 80, SKU = "WATCH004", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Cameras & Photography
                new Product { Id = 46, Naam = "Sony A7 IV", Omschrijving = "Full-frame mirrorless camera", Prijs = 2699.00m, BTWPercentage = 21, Voorraad = 15, SKU = "CAM001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 47, Naam = "Canon EOS R6 Mark II", Omschrijving = "Veelzijdige full-frame camera", Prijs = 2499.00m, BTWPercentage = 21, Voorraad = 18, SKU = "CAM002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 48, Naam = "DJI Mini 4 Pro", Omschrijving = "Compacte drone met 4K camera", Prijs = 759.00m, BTWPercentage = 21, Voorraad = 30, SKU = "CAM003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 49, Naam = "GoPro Hero 12 Black", Omschrijving = "Actiecamera voor extreme sporten", Prijs = 449.00m, BTWPercentage = 21, Voorraad = 50, SKU = "CAM004", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Monitors & Displays
                new Product { Id = 50, Naam = "LG UltraGear 27 inch 4K Gaming Monitor", Omschrijving = "Gaming monitor met hoge refresh rate", Prijs = 699.00m, BTWPercentage = 21, Voorraad = 25, SKU = "MON001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 51, Naam = "Dell UltraSharp 32 inch 4K Monitor", Omschrijving = "Professioneel beeldscherm voor werk", Prijs = 849.00m, BTWPercentage = 21, Voorraad = 30, SKU = "MON002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 52, Naam = "Samsung Odyssey G7", Omschrijving = "Curved gaming monitor", Prijs = 599.00m, BTWPercentage = 21, Voorraad = 35, SKU = "MON003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Gaming Consoles & Accessories
                new Product { Id = 53, Naam = "PlayStation 5", Omschrijving = "Next-gen gaming console", Prijs = 549.00m, BTWPercentage = 21, Voorraad = 40, SKU = "CONS001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 54, Naam = "Xbox Series X", Omschrijving = "Krachtige gaming console van Microsoft", Prijs = 499.00m, BTWPercentage = 21, Voorraad = 35, SKU = "CONS002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 55, Naam = "Nintendo Switch OLED", Omschrijving = "Hybride gaming console", Prijs = 349.00m, BTWPercentage = 21, Voorraad = 50, SKU = "CONS003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 56, Naam = "PlayStation VR2", Omschrijving = "Virtual reality headset voor PS5", Prijs = 549.00m, BTWPercentage = 21, Voorraad = 20, SKU = "CONS004", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 57, Naam = "Xbox Elite Controller Series 2", Omschrijving = "Premium gaming controller", Prijs = 179.00m, BTWPercentage = 21, Voorraad = 45, SKU = "CONS005", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Software & Subscriptions
                new Product { Id = 58, Naam = "Microsoft Office 365 Personal - 1 jaar", Omschrijving = "Productiviteitssoftware abonnement", Prijs = 69.00m, BTWPercentage = 21, Voorraad = 200, SKU = "SOFT001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 59, Naam = "Adobe Creative Cloud - 1 jaar", Omschrijving = "Complete suite voor creatieven", Prijs = 719.00m, BTWPercentage = 21, Voorraad = 150, SKU = "SOFT002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 60, Naam = "Norton 360 Deluxe - 1 jaar", Omschrijving = "Antivirus en beveiligingssoftware", Prijs = 39.99m, BTWPercentage = 21, Voorraad = 180, SKU = "SOFT003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },

                // Accessories & Peripherals
                new Product { Id = 61, Naam = "Logitech MX Master 3S", Omschrijving = "Draadloze premium muis", Prijs = 109.00m, BTWPercentage = 21, Voorraad = 70, SKU = "ACC001", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 62, Naam = "Keychron K8 Pro Mechanical Keyboard", Omschrijving = "Draadloos mechanisch toetsenbord", Prijs = 119.00m, BTWPercentage = 21, Voorraad = 55, SKU = "ACC002", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 63, Naam = "Anker PowerCore 20000mAh", Omschrijving = "Draagbare powerbank", Prijs = 49.99m, BTWPercentage = 21, Voorraad = 90, SKU = "ACC003", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 64, Naam = "SanDisk Extreme Pro 1TB SSD", Omschrijving = "Externe SSD voor opslag", Prijs = 149.00m, BTWPercentage = 21, Voorraad = 60, SKU = "ACC004", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) },
                new Product { Id = 65, Naam = "Webcam Logitech C920", Omschrijving = "Full HD webcam voor videobellen", Prijs = 79.00m, BTWPercentage = 21, Voorraad = 65, SKU = "ACC005", IsActief = true, AangemaaktOp = new DateTime(2024, 1, 1) }
            );

            // Seed data - 10 Customers
            modelBuilder.Entity<Klant>().HasData(
                new Klant
                {
                    Id = 1,
                    Bedrijfsnaam = "Voorbeeld Bedrijf B.V.",
                    Contactpersoon = "Jan Jansen",
                    Email = "info@voorbeeld.nl",
                    Telefoon = "0612345678",
                    Adres = "Voorbeeldstraat 1",
                    Postcode = "1234 AB",
                    Stad = "Amsterdam",
                    BTWNummer = "NL123456789B01",
                    KVKNummer = "12345678",
                    AangemaaktOp = new DateTime(2024, 1, 1)
                },
                new Klant
                {
                    Id = 2,
                    Bedrijfsnaam = "TechSolutions Nederland",
                    Contactpersoon = "Sophie de Vries",
                    Email = "sophie@techsolutions.nl",
                    Telefoon = "0687654321",
                    Adres = "Innovatielaan 45",
                    Postcode = "3045 BC",
                    Stad = "Rotterdam",
                    BTWNummer = "NL987654321B02",
                    KVKNummer = "87654321",
                    AangemaaktOp = new DateTime(2024, 1, 5)
                },
                new Klant
                {
                    Id = 3,
                    Bedrijfsnaam = "Bakkerij Van Dam",
                    Contactpersoon = "Pieter van Dam",
                    Email = "pieter@bakkerijvandam.nl",
                    Telefoon = "0623456789",
                    Adres = "Marktplein 12",
                    Postcode = "4567 CD",
                    Stad = "Utrecht",
                    BTWNummer = "NL234567890B03",
                    KVKNummer = "23456789",
                    AangemaaktOp = new DateTime(2024, 1, 10)
                },
                new Klant
                {
                    Id = 4,
                    Bedrijfsnaam = "Green Energy Solutions",
                    Contactpersoon = "Emma Mulder",
                    Email = "emma@greenenergy.nl",
                    Telefoon = "0634567890",
                    Adres = "Duurzaamweg 78",
                    Postcode = "2345 DE",
                    Stad = "Den Haag",
                    BTWNummer = "NL345678901B04",
                    KVKNummer = "34567890",
                    AangemaaktOp = new DateTime(2024, 1, 15)
                },
                new Klant
                {
                    Id = 5,
                    Bedrijfsnaam = "Consultancy Partners",
                    Contactpersoon = "Marco Visser",
                    Email = "marco@consultancy.nl",
                    Telefoon = "0645678901",
                    Adres = "Businesspark 100",
                    Postcode = "5678 EF",
                    Stad = "Eindhoven",
                    BTWNummer = "NL456789012B05",
                    KVKNummer = "45678901",
                    AangemaaktOp = new DateTime(2024, 1, 20)
                },
                new Klant
                {
                    Id = 6,
                    Bedrijfsnaam = "Creative Media Group",
                    Contactpersoon = "Lisa Bakker",
                    Email = "lisa@creativemedia.nl",
                    Telefoon = "0656789012",
                    Adres = "Designstraat 23",
                    Postcode = "6789 FG",
                    Stad = "Groningen",
                    BTWNummer = "NL567890123B06",
                    KVKNummer = "56789012",
                    AangemaaktOp = new DateTime(2024, 2, 1)
                },
                new Klant
                {
                    Id = 7,
                    Bedrijfsnaam = "Bouwbedrijf Hendriks",
                    Contactpersoon = "Tom Hendriks",
                    Email = "tom@bouwbedrijf.nl",
                    Telefoon = "0667890123",
                    Adres = "Kraanweg 56",
                    Postcode = "7890 GH",
                    Stad = "Breda",
                    BTWNummer = "NL678901234B07",
                    KVKNummer = "67890123",
                    AangemaaktOp = new DateTime(2024, 2, 5)
                },
                new Klant
                {
                    Id = 8,
                    Bedrijfsnaam = "Restaurant De Gouden Lepel",
                    Contactpersoon = "Anna de Wit",
                    Email = "anna@goudenlepel.nl",
                    Telefoon = "0678901234",
                    Adres = "Marktstraat 8",
                    Postcode = "8901 HI",
                    Stad = "Maastricht",
                    BTWNummer = "NL789012345B08",
                    KVKNummer = "78901234",
                    AangemaaktOp = new DateTime(2024, 2, 10)
                },
                new Klant
                {
                    Id = 9,
                    Bedrijfsnaam = "Fitness First Nederland",
                    Contactpersoon = "David Peters",
                    Email = "david@fitnessfirst.nl",
                    Telefoon = "0689012345",
                    Adres = "Sportlaan 99",
                    Postcode = "9012 IJ",
                    Stad = "Nijmegen",
                    BTWNummer = "NL890123456B09",
                    KVKNummer = "89012345",
                    AangemaaktOp = new DateTime(2024, 2, 15)
                },
                new Klant
                {
                    Id = 10,
                    Bedrijfsnaam = "Online Marketing Pro",
                    Contactpersoon = "Sarah van Leeuwen",
                    Email = "sarah@onlinemarketing.nl",
                    Telefoon = "0690123456",
                    Adres = "Digitalweg 200",
                    Postcode = "1012 JK",
                    Stad = "Amsterdam",
                    BTWNummer = "NL901234567B10",
                    KVKNummer = "90123456",
                    AangemaaktOp = new DateTime(2024, 2, 20)
                }
            );
        }
    }
}