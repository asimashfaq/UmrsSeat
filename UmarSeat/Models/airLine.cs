using Microsoft.Data.OData.Query.SemanticAst;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UmarSeat.Models
{
    public class airLine
    {
        [Key]
        [ScaffoldColumn(false)]
        public int id_AirLine { get; set; }
        [Required]
        [Display(Name="Airline Name")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Characters")]
        public string airlineName { get; set; }
        [ScaffoldColumn(false)]
        public int airlineContactPersonId { get; set; }
        [ScaffoldColumn(false)]
        public int id_Subscription { get; set; }
        [ForeignKey("id_Subscription")]
        public Subscription Subscription { get; set; }
        [ScaffoldColumn(false)]
        public DateTime createdAt { get; set; }
         [Display(Name = "Airline Country Name")]
        public string Country { get; set; }
      
        [NotMapped]
         public List<SelectListItem> ListCountry { get; set; }
    }
}