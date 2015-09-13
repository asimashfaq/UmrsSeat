using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UmarSeat.Models
{
    public class SearchSeatModel
    {
        public string pnrNumber { get; set; }
        public string newPnrNumber { get; set; }
        public string airline { get; set; }
        public string stockId { get; set; }
        public string recevingBranch { get; set; }
        public string category { get; set; }
        public string creationRange { get; set; }
        public string outBoundRange { get; set; }
        public string timeLimitRange { get; set; }
    }
}