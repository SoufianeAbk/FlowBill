using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowBill.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Productnaam is verplicht")]
        [Display(Name = "Productnaam")]
        public string Naam { get; set; }

        [Display(Name = "Omschrijving")]
        public string? Omschrijving { get; set; }

        [Required(ErrorMessage = "Prijs is verplicht")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Prijs moet groter zijn dan 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Prijs (excl. BTW)")]
        public decimal Prijs { get; set; }

        [Required(ErrorMessage = "BTW percentage is verplicht")]
        [Range(0, 100, ErrorMessage = "BTW moet tussen 0 en 100 zijn")]
        [Display(Name = "BTW %")]
        public int BTWPercentage { get; set; } = 21;

        [Display(Name = "Voorraad")]
        public int Voorraad { get; set; }

        [Display(Name = "SKU")]
        public string? SKU { get; set; }

        [Display(Name = "Actief")]
        public bool IsActief { get; set; } = true;

        [Display(Name = "Aangemaakt op")]
        public DateTime AangemaaktOp { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<BestellingItem> BestellingItems { get; set; } = new List<BestellingItem>();
    }
}