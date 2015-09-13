using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UmarSeat.Models
{
    public class Sector
    {
        [Key]
        public int id_Sector { get; set; }
        [Required]
        [Display(Name="Sector Name")]
        [RegularExpression(@"^([A-Z \.\&\'\-]+)$", ErrorMessage = "Only Capital Letters")]
        public string sectorName { get; set; }

        
        [Display(Name="Country Name")]
        public string country { get; set; }
        [Display(Name="Airline Name")]
        public string airline { get; set; }
        [Display(Name="Sector Type")]
        public string category { get; set; }

        [ScaffoldColumn(false)]
        public int id_Subscription { get; set; }
        [NotMapped]
        public List<SelectListItem> ListAirline { get; set; }
        [NotMapped]
        public List<SelectListItem> ListCountry { get; set; }
    }
}