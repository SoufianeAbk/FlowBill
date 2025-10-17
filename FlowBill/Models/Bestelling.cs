using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowBill.Models
{
    public class Bestelling
    {
        public int Id { get; set; }

        [Display(Name = "Bestelnummer")]
        public string BestelNummer { get; set; }

        [Required(ErrorMessage = "Klant is verplicht")]
        [Display(Name = "Klant")]
        public int KlantId { get; set; }

        [Display(Name = "Besteldatum")]
        public DateTime BestelDatum { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Subtotaal")]
        public decimal SubTotaal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "BTW Bedrag")]
        public decimal BTWBedrag { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Totaal")]
        public decimal Totaal { get; set; }

        [Display(Name = "Status")]
        public BestellingStatus Status { get; set; } = BestellingStatus.InBehandeling;

        [Display(Name = "Opmerkingen")]
        public string? Opmerkingen { get; set; }

        // Navigation properties
        public virtual Klant Klant { get; set; }
        public virtual ICollection<BestellingItem> BestellingItems { get; set; } = new List<BestellingItem>();
        public virtual Factuur? Factuur { get; set; }
    }

    public enum BestellingStatus
    {
        InBehandeling,
        Goedgekeurd,
        Verzonden,
        Afgeleverd,
        Geannuleerd
    }
}