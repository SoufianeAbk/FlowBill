using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowBill.Models
{
    public class BestellingItem
    {
        public int Id { get; set; }

        [Required]
        public int BestellingId { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Aantal is verplicht")]
        [Range(1, int.MaxValue, ErrorMessage = "Aantal moet minimaal 1 zijn")]
        [Display(Name = "Aantal")]
        public int Aantal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Prijs per stuk")]
        public decimal PrijsPerStuk { get; set; }

        [Display(Name = "BTW %")]
        public int BTWPercentage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Totaal")]
        public decimal Totaal { get; set; }

        // Navigation properties
        public virtual Bestelling Bestelling { get; set; }
        public virtual Product Product { get; set; }
    }
}