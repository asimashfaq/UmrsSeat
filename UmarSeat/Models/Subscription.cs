using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UmarSeat.Models
{
    public class Subscription
    {
        [Key]
        [ScaffoldColumn(false)]
        public int id_Subscription { get; set; }
        [ScaffoldColumn(false)]
        public DateTime cDate { get; set; }
        [ScaffoldColumn(false)]
        public DateTime startDate { get; set; }
        [ScaffoldColumn(false)]
        public DateTime endDate { get; set; }
        [ScaffoldColumn(false)]
        public String UserId { get; set;}
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
    }

    public enum SubscriptionStatus {
        Active,
        [Description("Pending Admin Approval")]
        PendingAdminApproval,
        [Description("Pending Payment")]
        PendingPayment,
        Suspended,
        Expired,
        Blocked
    }
}