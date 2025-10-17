using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowBill.Models
{
    public class Factuur
    {
        public int Id { get; set; }

        [Display(Name = "Factuurnummer")]
        public string FactuurNummer { get; set; }

        [Required]
        public int BestellingId { get; set; }

        [Display(Name = "Factuurdatum")]
        public DateTime FactuurDatum { get; set; } = DateTime.Now;

        [Display(Name = "Vervaldatum")]
        public DateTime VervalDatum { get; set; } = DateTime.Now.AddDays(30);

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
        public FactuurStatus Status { get; set; } = FactuurStatus.Openstaand;

        [Display(Name = "PDF Pad")]
        public string? PDFPad { get; set; }

        [Display(Name = "E-mail verzonden")]
        public bool EmailVerzonden { get; set; } = false;

        [Display(Name = "E-mail verzonden op")]
        public DateTime? EmailVerzondenOp { get; set; }

        // Navigation properties
        public virtual Bestelling Bestelling { get; set; }
    }

    public enum FactuurStatus
    {
        Openstaand,
        Betaald,
        Vervallen,
        Geannuleerd
    }
}