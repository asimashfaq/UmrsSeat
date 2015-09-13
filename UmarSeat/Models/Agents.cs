using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UmarSeat.Models
{
    public class Agents 
    {
        [Key]
        public int id_Agent { get; set; }
        public int id_Subscription { get; set; }
        public int id_Person { get; set; }
        [Display(Name="Company Name")]
        public string CompanyName { get; set; }
        
        [ForeignKey("id_Person")]
        public person Person { get; set; }

    }
}