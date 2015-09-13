using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UmarSeat.Models
{
    public class branches
    {
        [Key]
        public int id_branch { get; set; }
        [Required]
        [Display(Name="Branch Name")]
        public string branchName { get; set;}
      
        [Display(Name= "Branch Country")]
        public string branchCountry { get; set; }
      
        [Display(Name="Branch City")]
        public string branchCity { get; set;}
        [ScaffoldColumn(false)]
        public int id_Subscription { get; set; }
        [ScaffoldColumn(false)]
        public int id_BranchManager { get; set; }
      
        [Display(Name="Branch Address")]
        public string branchAddress { get; set; }
        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }
        [NotMapped]
        public List<SelectListItem> ListCity { get; set; }
        [NotMapped]
        public List<SelectListItem> ListCountry { get; set; }

    }
}