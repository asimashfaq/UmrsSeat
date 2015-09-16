using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UmarSeat.Models
{
    public class pnrLog
    {
        [Key]
        public int pnrLogId { get; set; }
        [Display(Name ="PNR #")]
        public string pnrNumber { get; set; }
        [Display(Name = "Total Seats")]
        public int totalSeats { get; set; }
        [Display(Name = "Avaliable Seats")]
        public int avaliableSeats { get; set; }
        [Display(Name = "Group Split")]
        public int groupSplit { get; set; }
        [Display(Name = "Sell Seats")]
        public int sellSeats { get; set; }
        [Display(Name = "Transfer Seats")]
        public int transferSeats { get; set; }
        [Display(Name = "Receive Seats")]
        public int receiveSeats { get; set; }
        [Display(Name = "Branch Name")]
        public string branchName { get; set; }
        [Display(Name = "ID Subscription")]
        public int idSubscription { get; set; }
        [Display(Name = "PNR Status")]
        public string pnrStatus { get; set; }  
        public string pnrLock { get; set; }
        [NotMapped]
        public SeatConfirmation sc { get; set; }
        [NotMapped]
        public StockTransfer st { get; set; }
        [Timestamp]
        [ConcurrencyCheck]
        public byte[] RowVersion { get; set; }
        [Display(Name = "Created At")]
        public DateTime createdAt { get; set; }
        public pnrLog()
        {
            createdAt = DateTime.Now;
        }
    }
}