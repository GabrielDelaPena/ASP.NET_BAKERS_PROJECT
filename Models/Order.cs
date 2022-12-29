using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Bakers.Areas.Identity.Data;

namespace Bakers.Models
{
    public class Order
    {
        public int Id { get; set; }
        // public Client client { get; set; } wordt later toegevoegd!!!
        [Required]
        [Display(Name = "BestelDatum")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "Straat")]
        public string Street { get; set; }
        [Required]
        [Display(Name = "Postcode")]
        public string Zip { get; set; }
        [Required]
        [Display(Name = "Woonplaats")]
        public string City { get; set; }
        [Display(Name = "Geleverd")]
        public Boolean Delivered { get; set; } = false;
        //Verbergen
        public Boolean IsHidden { get; set; }

        // Relations
        // many to many
        [Display(Name = "Producten")]
        public List<Product>? Products { get; set; }
        [NotMapped]
        public List<int>? ProductIds { get; set; }

        [Display(Name = "User")]
        public ApplicationUser? User { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
    }
}
