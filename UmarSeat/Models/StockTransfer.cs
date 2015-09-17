using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UmarSeat.Models
{
    public class StockTransfer
    {
        [Key]
        public int id_StockTransfer { get; set; }
        [Display(Name="Country Name")]
        public string country { get; set; }
        [Display(Name = "PNR #")]
        public string pnrNumber { get; set; }
        [Display(Name = "Airline")]
        public string airLine { get; set; }
        [Display(Name = "Stock Id")]
        public string stockId { get; set; }
        [Display(Name = "Agent Name")]
        public string idAgent { get; set; }
         [Display(Name = "Transfering Branch")]
        public string transferingBranch { get; set; }
         [Display(Name = "Receving Branch")]
        public string recevingBranch { get; set; }
         [Display(Name = "Sellling Branch")]
        public string sellingBranch { get; set; }
         [Display(Name = "Cost")]
        public int cost { get; set; }
         [Display(Name = "Margin")]
        public int margin { get; set; }
         [Display(Name = "Selling Price")]
        public int sellingPrice { get; set; }
         [Display(Name = "# of Seats")]
        public int noOfSeats { get; set; }
         [Display(Name = "Package ?")]
        public bool isPackage { get; set; }
         [Display(Name="Tickted?")]
         public bool isTickted { get; set; }


         [Display(Name = "Advance Amount")]
        public int advanceAmount { get; set; }
         [Display(Name = "Advance Date")]
        public DateTime? advanceDate { get; set; }
         [Display(Name = "GDS PNR #")]
        public string gdsPnrNumber { get; set; }
         [Display(Name = "Catalyst Invoice #")]
        public string catalystInvoiceNumber { get; set; }
        public DateTime createAt
        {
            get;
            set;
        }
        public DateTime UpdateAt
        {
            get;
            set;
        }
        public int id_Subscription { get; set; }
        public bool isDelete { get; set; }
        [NotMapped]
        public List<SelectListItem> ListBranches { get; set; }
        [NotMapped]
        public List<SelectListItem> ListAgents { get; set; }
        [NotMapped]
        public List<SelectListItem> ListAirline { get; set; }
        [NotMapped]
        public List<SelectListItem> ListCountry { get; set; }
        [NotMapped]
        public List<SelectListItem> ListStockId { get; set; }
        [NotMapped]
        public List<SelectListItem> ListPNR { get; set; }
        [NotMapped]
        public string ptype { get; set; }
        public string pnrStatus { get; set; }
        [NotMapped]
        public Agents agent { get; set; }

    }
    public class SearchStockModel
    {
        public string pnrNumber { get; set; }
        public string airLine { get; set; }
        public string stockId { get; set; }
        public string idAgent { get; set; }
        public string transferingBranch { get; set; }
        public string recevingBranch { get; set; }
        public string sellingBranch { get; set; }
        public float cost { get; set; }
        public float margin { get; set; }
        public float sellingPrice { get; set; }
        public int noOfSeats { get; set; }
        public bool isPackage { get; set; }
        public float advanceAmount { get; set; }
        public string gdsPnrNumber { get; set; }
        public string catalystInvoiceNumber { get; set; }
        public string creationRange { get; set; }
        public string advanceRange { get; set; }
    }
}