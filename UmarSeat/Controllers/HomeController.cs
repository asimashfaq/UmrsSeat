using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UmarSeat.Helpers;
using UmarSeat.Models;

namespace UmarSeat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [CheckSessionOut]
        [Authorize]
        public ActionResult Index()
        {
            int idSubcription = 0;
            
            idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            
            ViewBag.latestRecord = db.SeatConfirmation.Where(x=>  x.id_Subscription == idSubcription).OrderByDescending(x => x.id_SeatConfirmation).Take(5).ToList();
            ViewBag.expriysoon = db.SeatConfirmation.Where(x => x.timeLimit >= DateTime.Now && x.id_Subscription == idSubcription).OrderBy(x => x.timeLimit).Take(5).ToList();
            ViewBag.outbound = db.SeatConfirmation.Where(x => x.outBoundDate >= DateTime.Now && x.id_Subscription == idSubcription).OrderBy(x => x.outBoundDate).Take(5).ToList();
            var subscription = db.Subscription.Where(x => x.id_Subscription == idSubcription).FirstOrDefault();
            ViewBag.stdata = subscription.startDate;
            ViewBag.edate = subscription.endDate;
            ViewBag.status = subscription.SubscriptionStatus;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}