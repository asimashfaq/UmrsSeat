using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UmarSeat.Models
{
    public class SeatConfirmation
    {
        [Key]
        public int id_SeatConfirmation { get; set; }
        [Display(Name="PNR #")]
        public string pnrNumber { get; set; }
        [Display(Name = "New PNR #")]
        public string newPnrNumber { get; set; }
        [Display(Name = "Airline")]
        public string airLine { get; set; }
        [Display(Name = "Stock Id")]
        public string stockId { get; set; }
        [Display(Name = "OutBound Date")]
        public DateTime outBoundDate { get; set; }
        [Display(Name = "InBound Date")]
        public DateTime inBoundDate { get; set; }
        [Display(Name = "OutBound Sector")]
        public string outBoundSector { get; set; }
        [Display(Name = "InBound Sector")]
        public string inBoundSector { get; set; }
        [Display(Name = "# of Seats")]
        public int noOfSeats { get; set; }
        [Display(Name = "Cost (PKR)")]
        public int cost { get; set; }
        [Display(Name = "Category")]
        public string category { get; set; }
        [Display(Name = "Receving Branch")]
        public string recevingBranch { get; set; }
        [Display(Name = "EMD #")]
        public string emdNumber { get; set; }
        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }
        [ScaffoldColumn(false)]
        public int id_Subscription { get; set; }
        [ScaffoldColumn(false)]
        public DateTime UpdatedAt { get; set; }
        [Display(Name = "Time Limit")]
        public DateTime timeLimit { get; set; }
        [ScaffoldColumn(false)]
        public bool isDelete { get; set; }
        public string country {get;set;}
        [NotMapped]
        public List<SelectListItem> ListBranches { get; set; }
        [NotMapped]
        public List<SelectListItem> ListSectors { get; set; }
        [NotMapped]
        public List<SelectListItem> ListAirline { get; set; }
        [NotMapped]
        public List<SelectListItem> ListCategory { get; set; }
        [NotMapped]
        public List<SelectListItem> ListCountry { get; set; }
        [NotMapped]
        public List<SelectListItem> ListStockId { get; set; }
        [NotMapped]
        public List<SelectListItem> ListPNR { get; set; }
        [NotMapped]
        public string ptype { get; set; }
        [NotMapped]
        public int avaliableSeats { get; set; }
        public string pnrStatus { get; set; }
        public string pnrStatus1 { get; set; }
        [Timestamp]
        [ConcurrencyCheck]
        public byte[] RowVersion { get; set; }
        public bool Locked { get; set; }



    }
}