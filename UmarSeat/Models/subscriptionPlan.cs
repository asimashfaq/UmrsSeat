using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UmarSeat.Models
{
    public class subscriptionPlan
    {
        [Key]
        [ScaffoldColumn(false)]
        public int id_SubscriptionPlan { get; set; }
        [RegularExpression(@"^([a-zA-Z \.\&\'\-]+)$", ErrorMessage = "Invalid Characters")]
        [Required(ErrorMessage ="Enter Subscription Type e.g BASIC,SLIVER,GOLD")]
        [Display(Name = "Subscription Type")]
        public string nameSubscriptionPlan { get; set; }
        [Required]
        [Display(Name= "Subscription Duration")]
        [RegularExpression(@"^([0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Characters")]
        public int duration { get; set; }
        [Required]
      
        public DurationType subscriptionDurationType { get; set; }

        [Display(Name="Subscription Price")]
        public float subscriptionPrice { get; set; }
        [ScaffoldColumn(false)]
        public DateTime createdAt { get; set; }
        [ScaffoldColumn(false)]
        public DateTime updatedAt { get; set; }
    
    }

    public enum DurationType
    {
        Day,
        Days,
        Month,
        Months,
        Year,
        Years,
        LifeTime
    };
}