using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Bakers.Models
{
    public class Variety
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }
        // Verbergen
        public Boolean IsHidden { get; set; }

        // Relations
        // one to many
        [NotMapped]
        public List<int> ProductIds { get; set; }
        [Display(Name = "Producten")]
        public List<Product>? Products { get; set; }
    }
}
