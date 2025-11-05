using FlowBill.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlowBill.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IConfiguration? _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Product> Producten { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
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

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Producten)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Check if we're using SQLite to handle DateTime properly
            var isSQLite = Database.ProviderName?.Contains("Sqlite") ?? false;

            // Seed data - Product Categories
            var categories = new List<ProductCategory>
            {
                new ProductCategory
                {
                    Id = 1,
                    Naam = "Webdiensten",
                    Omschrijving = "Professionele webdiensten en online oplossingen",
                    IconClass = "fas fa-globe",
                    IsActief = true
                },
                new ProductCategory
                {
                    Id = 2,
                    Naam = "Games",
                    Omschrijving = "Populaire videogames voor verschillende platforms",
                    IconClass = "fas fa-gamepad",
                    IsActief = true
                },
                new ProductCategory
                {
                    Id = 3,
                    Naam = "Sport",
                    Omschrijving = "Sportartikelen en fitnessequipment",
                    IconClass = "fas fa-dumbbell",
                    IsActief = true
                }
            };

            modelBuilder.Entity<ProductCategory>().HasData(categories);

            // Seed data - Products
            var products = new List<Product>
            {
                // Webdiensten (Categorie 1)
                new Product
                {
                    Id = 1,
                    Naam = "Website ontwikkeling",
                    CategoryId = 1,
                    Omschrijving = "Complete website ontwikkeling op maat",
                    Prijs = 2500.00m,
                    BTWPercentage = 21,
                    Voorraad = 100,
                    SKU = "WEB001",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 2,
                    Naam = "Hosting - Jaarlijks",
                    CategoryId = 1,
                    Omschrijving = "Betrouwbare webhosting voor 1 jaar",
                    Prijs = 120.00m,
                    BTWPercentage = 21,
                    Voorraad = 100,
                    SKU = "HOST001",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 3,
                    Naam = "SEO Optimalisatie",
                    CategoryId = 1,
                    Omschrijving = "Zoekmotor optimalisatie voor betere vindbaarheid",
                    Prijs = 750.00m,
                    BTWPercentage = 21,
                    Voorraad = 100,
                    SKU = "SEO001",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 4,
                    Naam = "Logo ontwerp",
                    CategoryId = 1,
                    Omschrijving = "Professioneel logo ontwerp inclusief 3 concepten",
                    Prijs = 350.00m,
                    BTWPercentage = 21,
                    Voorraad = 100,
                    SKU = "LOGO001",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                },
                
                // Games (Categorie 2)
                new Product
                {
                    Id = 5,
                    Naam = "The Last of Us Part II",
                    CategoryId = 2,
                    Omschrijving = "Spannend actie-avonturen game voor PlayStation 5",
                    Prijs = 59.99m,
                    BTWPercentage = 21,
                    Voorraad = 45,
                    SKU = "GAME001",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 6,
                    Naam = "FIFA 25",
                    CategoryId = 2,
                    Omschrijving = "De nieuwste voetbalsimulatie game",
                    Prijs = 69.99m,
                    BTWPercentage = 21,
                    Voorraad = 60,
                    SKU = "GAME002",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 7,
                    Naam = "Red Dead Redemption 2",
                    CategoryId = 2,
                    Omschrijving = "Western actie-avontuur game voor meerdere platforms",
                    Prijs = 49.99m,
                    BTWPercentage = 21,
                    Voorraad = 35,
                    SKU = "GAME003",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 8,
                    Naam = "Mario Kart 8 Deluxe",
                    CategoryId = 2,
                    Omschrijving = "Populaire racegame voor Nintendo Switch",
                    Prijs = 54.99m,
                    BTWPercentage = 21,
                    Voorraad = 50,
                    SKU = "GAME004",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                },

                // Sport (Categorie 3)
                new Product
                {
                    Id = 9,
                    Naam = "Yoga Mat Premium",
                    CategoryId = 3,
                    Omschrijving = "Extra dikke yogamat met anti-slip oppervlak (6mm)",
                    Prijs = 29.99m,
                    BTWPercentage = 21,
                    Voorraad = 75,
                    SKU = "SPORT001",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 10,
                    Naam = "Dumbbells Set 2x10kg",
                    CategoryId = 3,
                    Omschrijving = "Verstelbare dumbbells set voor thuisfitness",
                    Prijs = 89.99m,
                    BTWPercentage = 21,
                    Voorraad = 40,
                    SKU = "SPORT002",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 11,
                    Naam = "Hardloopschoenen Pro",
                    CategoryId = 3,
                    Omschrijving = "Professionele hardloopschoenen met extra demping",
                    Prijs = 129.99m,
                    BTWPercentage = 21,
                    Voorraad = 55,
                    SKU = "SPORT003",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 12,
                    Naam = "Fitness Tracker Smart",
                    CategoryId = 3,
                    Omschrijving = "Slimme fitness tracker met hartslagmeter en GPS",
                    Prijs = 79.99m,
                    BTWPercentage = 21,
                    Voorraad = 65,
                    SKU = "SPORT004",
                    IsActief = true,
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
                }
            };

            modelBuilder.Entity<Product>().HasData(products);

            // Seed data - Customers  
            var customers = new List<Klant>
            {
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
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 1)
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
                    AangemaaktOp = isSQLite ? DateTime.UtcNow : new DateTime(2024, 1, 5)
                }
            };

            modelBuilder.Entity<Klant>().HasData(customers);
        }
    }
}