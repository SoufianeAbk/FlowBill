using System.ComponentModel.DataAnnotations;

namespace FlowBill.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Categorienaam is verplicht")]
        [Display(Name = "Categorienaam")]
        public string Naam { get; set; }

        [Display(Name = "Omschrijving")]
        public string? Omschrijving { get; set; }

        [Display(Name = "Icoon")]
        public string? IconClass { get; set; }

        [Display(Name = "Actief")]
        public bool IsActief { get; set; } = true;

        // Navigation property
        public virtual ICollection<Product> Producten { get; set; } = new List<Product>();
    }
}