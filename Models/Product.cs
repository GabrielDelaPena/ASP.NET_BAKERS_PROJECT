using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Bakers.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Prijs")]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Afbeelding")]
        public string Image { get; set; }
        [Required]
        [Display(Name = "Favoriet")]
        public Boolean Favorite { get; set; }
        // Verbergen
        public Boolean IsHidden { get; set; }

        // Relations
        // one to many
        public int? VarietyId { get; set; }
        [Display(Name = "Soort")]
        public Variety? Variety { get; set; }

        // many to many
        [Display(Name = "BestelNummers")]
        public List<Order>? Orders { get; set; }
        [NotMapped]
        public List<int>? OrderIds { get; set; }
    }
}
