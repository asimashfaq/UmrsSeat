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
        public string pnrNumber { get; set; }
        public int totalSeats { get; set; }
        public int avaliableSeats { get; set; }
        public int groupSplit { get; set; }
        public int sellSeats { get; set; }
        public int transferSeats { get; set; }
        public int receiveSeats { get; set; }
        public string branchName { get; set; }
        public int idSubscription { get; set; }
        public string pnrStatus { get; set; }
        public string pnrLock { get; set; }
        [NotMapped]
        public SeatConfirmation sc { get; set; }
        [NotMapped]
        public StockTransfer st { get; set; }
        [Timestamp]
        [ConcurrencyCheck]
        public byte[] RowVersion { get; set; }
    }
}