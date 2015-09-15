using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UmarSeat.Models;

namespace UmarSeat.Controllers
{
    public class pnrController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: pnr
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult log()
        {
            List<pnrLog> pnrList = db.pnrLogs.ToList();
            pnrList.ForEach(x => {
                x.sc = db.SeatConfirmation.Where(sc => sc.pnrNumber == x.pnrNumber && sc.newPnrNumber == null && sc.recevingBranch == x.branchName).FirstOrDefault();
                if(x.sc == null)
                {
                    x.sc = db.SeatConfirmation.Where(sc => sc.newPnrNumber == x.pnrNumber  && sc.recevingBranch == x.branchName).FirstOrDefault();
                }
                if (x.sc == null)
                {
                    x.sc = db.SeatConfirmation.Where(sc => sc.newPnrNumber == x.pnrNumber).FirstOrDefault();
                   
                }
                if (x.sc == null)
                {
                    x.sc = db.SeatConfirmation.Where(sc => sc.pnrNumber == x.pnrNumber).FirstOrDefault();
                  
                }

                x.sc.recevingBranch = x.branchName;
            });
            return View(pnrList);
        }
    }
}