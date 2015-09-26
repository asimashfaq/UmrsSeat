using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UmarSeat.Models
{
    public class City
    {
        [Key]
        public int id_City { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string countryFull { get; set; }
       
    }
}