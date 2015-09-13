using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UmarSeat.Models
{
    public class Stock
    {
        [Key]
        public int id_Stock { get; set; }
        [Display(Name="Stock Name")]
        [Required]
        public string stockName { get; set; }
        public int id_Subscription { get; set; }
    }
}