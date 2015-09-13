using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UmarSeat.Models
{
    [Table("Persons")]
    public class person
    {
        [Key]
        public int id_Person { get; set; }



        [ScaffoldColumn(false)]
        public string userId { get; set; }
        [ForeignKey("userId")]
        public virtual ApplicationUser User { get; set; }


        [Required]
        [Display(Name="First Name")]
        [RegularExpression(@"^([a-zA-Z \.\&\'\-]+)$", ErrorMessage = "Invalid Characters")]
        public string firstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^([a-zA-Z \.\&\'\-]+)$", ErrorMessage = "Invalid Characters")]
        public string lastName { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage = "Email is is not valid.")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required]
        [Display(Name = "Mobile Number")]
     
     
        public string mobileNumber { get; set; }

        [Display(Name="Branch Name")]
        public string branchName { get; set; }

        [ScaffoldColumn(false)]
        public string telephoneNumber { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? createdAt { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? updatedAt { get; set; }
        [ScaffoldColumn(false)]
        public string accountStatus { get; set; }
        [ScaffoldColumn(false)]
        [NotMapped]
        public List<SelectListItem> listBranches { get; set; }

    }
}