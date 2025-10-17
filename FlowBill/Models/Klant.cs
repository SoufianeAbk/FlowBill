using System.ComponentModel.DataAnnotations;

namespace FlowBill.Models
{
    public class Klant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bedrijfsnaam is verplicht")]
        [Display(Name = "Bedrijfsnaam")]
        public string Bedrijfsnaam { get; set; }

        [Display(Name = "Contactpersoon")]
        public string? Contactpersoon { get; set; }

        [Required(ErrorMessage = "E-mail is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Telefoonnummer")]
        [Phone(ErrorMessage = "Ongeldig telefoonnummer")]
        public string? Telefoon { get; set; }

        [Required(ErrorMessage = "Adres is verplicht")]
        public string Adres { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Stad is verplicht")]
        public string Stad { get; set; }

        [Display(Name = "BTW-nummer")]
        public string? BTWNummer { get; set; }

        [Display(Name = "KVK-nummer")]
        public string? KVKNummer { get; set; }

        [Display(Name = "Aangemaakt op")]
        public DateTime AangemaaktOp { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<Bestelling> Bestellingen { get; set; } = new List<Bestelling>();
    }
}