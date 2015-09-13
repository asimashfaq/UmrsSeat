using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UmarSeat.Models
{
    public class Category
    {
        [Key]
        public int id_Category { get; set; }
        [Required]
        [Display(Name="Category Name")]
        public string categoryName { get; set; }
        [ScaffoldColumn(false)]
        public int id_Subscription { get; set; }
    }
}