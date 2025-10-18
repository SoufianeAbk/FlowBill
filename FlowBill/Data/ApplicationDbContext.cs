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

            // Seed data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Naam = "Website ontwikkeling", Omschrijving = "Complete website ontwikkeling", Prijs = 2500.00m, BTWPercentage = 21, Voorraad = 100, SKU = "WEB001", IsActief = true },
                new Product { Id = 2, Naam = "Hosting - Jaarlijks", Omschrijving = "Webhosting voor 1 jaar", Prijs = 120.00m, BTWPercentage = 21, Voorraad = 100, SKU = "HOST001", IsActief = true },
                new Product { Id = 3, Naam = "SEO Optimalisatie", Omschrijving = "Zoekmotor optimalisatie", Prijs = 750.00m, BTWPercentage = 21, Voorraad = 100, SKU = "SEO001", IsActief = true },
                new Product { Id = 4, Naam = "Logo ontwerp", Omschrijving = "Professioneel logo ontwerp", Prijs = 350.00m, BTWPercentage = 21, Voorraad = 100, SKU = "LOGO001", IsActief = true }
            );

            modelBuilder.Entity<Klant>().HasData(
                new Klant { Id = 1, Bedrijfsnaam = "Voorbeeld Bedrijf B.V.", Contactpersoon = "Jan Jansen", Email = "info@voorbeeld.nl", Telefoon = "0612345678", Adres = "Voorbeeldstraat 1", Postcode = "1234 AB", Stad = "Amsterdam", BTWNummer = "NL123456789B01", KVKNummer = "12345678" }
            );
        }
    }
}