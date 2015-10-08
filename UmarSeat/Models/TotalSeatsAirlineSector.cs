using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UmarSeat.Models
{
    public class TotalSeatsAirlineSector
    {
        public int TotalSeats { get; set; }
        public string airLine { get; set; }
        public string outBoundSector { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd}")]
        public DateTime outBoundDate { get; set; }
        public int id_Subscription { get; set; }
    }
    public class SaleSeatsAirlineSector
    {
        public int TotalSeats { get; set; }
        public string airLine { get; set; }
        public string outBoundSector { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd}")]
        public DateTime outBoundDate { get; set; }
        public string pnrNumber { get; set; }
        public string recevingBranch { get; set; }
        public int id_Subscription { get; set; }
       public object data { get; set; }
    }
}