using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Bakers.Models;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace Bakers.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Display(Name = "Voornaam")]
    public string FirstName { get; set; }
    [Display(Name = "Achternaam")]
    public string LastName { get; set; }

    [Display(Name = "Orders")]
    public List<Order>? Orders { get; set; }
    [NotMapped]
    public List<int>? OrderIds { get; set; }
}

