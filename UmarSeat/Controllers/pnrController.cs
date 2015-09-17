using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UmarSeat.Models;
using UmarSeat.Helpers;

namespace UmarSeat.Controllers
{
    public class pnrController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: pnr
        [CheckSessionOut]
        public ActionResult Index()
        {
            ApplicationDbContext db1 = new ApplicationDbContext();
            int idSubscription = 1002;
            pnrLog pl = new pnrLog();
            List<pnrLog> pnrAvaliable = new List<pnrLog>();
            pl.ListPNR = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Session["branchName"].ToString()))
            {
                pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubscription ).OrderBy(x => x.pnrLogId).ToList();
                pnrAvaliable.ForEach(pr =>
                {


                    pl.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber + "(" + pr.branchName + ")", Value = pr.pnrNumber + "," + pr.branchName });
                });
            }
            else
            {
                string sb = Session["branchName"].ToString();
                pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubscription  && x.branchName == sb).OrderBy(x => x.pnrLogId).ToList();
                pnrAvaliable.ForEach(pr =>
                {

                    pl.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber, Value = pr.pnrNumber });
                });

            }
            return View(pl);
        }
        [CheckSessionOut]
        public ActionResult log()
        {
            List<pnrLog> pnrList2 = new List<pnrLog>();
            List<Task> task = new List<Task>();
            int idSubscription = 1002;
            string br = Session["branchName"].ToString();


            task.Add(Task.Factory.StartNew(()=>{
                ApplicationDbContext db1 = new ApplicationDbContext();
                int totalpnr = 0;
                if(string.IsNullOrEmpty(br))
                {
                    totalpnr = db1.pnrLogs.Where(x => x.idSubscription == idSubscription).Count();
                }
                else
                {
                    totalpnr = db1.pnrLogs.Where(x => x.idSubscription == idSubscription && x.branchName == br).Count();
                }
               
                ViewBag.totalpnr = totalpnr;
               


            }));
            task.Add(Task.Factory.StartNew(()=> {
                ApplicationDbContext db1 = new ApplicationDbContext();
                DateTime today = DateTime.Today;                    // earliest time today 
                DateTime tomorrow = DateTime.Today.AddDays(1);      // earliest time tomorrow
                ViewBag.todate = today;
                var todayexpires = 0;
               
                if (string.IsNullOrEmpty(br))
                {
                    todayexpires = db1.SeatConfirmation.Where(x => x.id_Subscription == idSubscription && x.timeLimit >= today && x.timeLimit < tomorrow).Count();
                }
                else
                {
                    todayexpires = db1.SeatConfirmation.Where(x => x.id_Subscription == idSubscription && x.timeLimit >= today && x.timeLimit < tomorrow && x.recevingBranch == br).Count();
                }

                ViewBag.totaltodayexpire = todayexpires;
            }));
            task.Add(Task.Factory.StartNew(() => {
                ApplicationDbContext db1 = new ApplicationDbContext();
                
                if (string.IsNullOrEmpty(br))
                {
                    ViewBag.totalavalible = db1.pnrLogs.Where(x => x.idSubscription == idSubscription && x.pnrStatus == "Avaliable").Count();
                }
                else
                {
                    ViewBag.totalavalible = db1.pnrLogs.Where(x => x.idSubscription == idSubscription && x.pnrStatus == "Avaliable" && x.branchName == br).Count();
                }

            }));
            task.Add(Task.Factory.StartNew(() => {
                ApplicationDbContext db1 = new ApplicationDbContext();
               
                if (string.IsNullOrEmpty(br))
                {
                    ViewBag.totalsold = db1.pnrLogs.Where(x => x.idSubscription == idSubscription && x.pnrStatus == "Sold").Count();
                }
                else
                {
                    ViewBag.totalsold = db1.pnrLogs.Where(x => x.idSubscription == idSubscription && x.pnrStatus == "Sold" && x.branchName == br).Count();
                }
            }));
            task.Add(Task.Factory.StartNew(() => {
               ApplicationDbContext db1 = new ApplicationDbContext();
                
                if (string.IsNullOrEmpty(br))
                {
                    var seatsavaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubscription && x.pnrStatus == "Avaliable")
                                       .GroupBy(x => x.idSubscription).Select(x => new { ts = x.Sum(y => y.avaliableSeats) }).Single();
                    ViewBag.totalseatsAvaliable = seatsavaliable.ts;
                }
                else
                {
                    var seatsavaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubscription && x.pnrStatus == "Avaliable" && x.branchName == br)
                                   .GroupBy(x => x.idSubscription).Select(x => new { ts = x.Sum(y => y.avaliableSeats) }).Single();
                    ViewBag.totalseatsAvaliable = seatsavaliable.ts;
                }
                
            }));


            task.Add(Task.Factory.StartNew(() => {
                ApplicationDbContext db1 = new ApplicationDbContext();
                List<pnrLog> pnrList = new List<pnrLog>();
                if (string.IsNullOrEmpty(br))
                {
                    pnrList = db1.pnrLogs.Where(x => x.createdAt.Year == DateTime.Now.Year).ToList();
                }
                else
                {
                    pnrList = db1.pnrLogs.Where(x => x.createdAt.Year == DateTime.Now.Year && x.branchName == br).ToList();
                }
                pnrList.ForEach(x => {
                    db = new ApplicationDbContext();
                    x.sc = db.SeatConfirmation.Where(sc => sc.pnrNumber == x.pnrNumber && sc.newPnrNumber == null && sc.recevingBranch == x.branchName && x.createdAt.Month == DateTime.Now.Month).FirstOrDefault();
                    if (x.sc == null)
                    {
                        x.sc = db.SeatConfirmation.Where(sc => sc.newPnrNumber == x.pnrNumber && sc.recevingBranch == x.branchName && x.createdAt.Month == DateTime.Now.Month).FirstOrDefault();
                        if (x.sc == null)
                        {
                            x.sc = db.SeatConfirmation.Where(sc => sc.newPnrNumber == x.pnrNumber).FirstOrDefault();
                            if (x.sc == null)
                            {
                                x.sc = db.SeatConfirmation.Where(sc => sc.pnrNumber == x.pnrNumber).FirstOrDefault();
                                if(x.sc != null)
                                {
                                    x.sc.recevingBranch = x.branchName;
                                    pnrList2.Add(x);
                                }

                            }
                            else
                            {
                                x.sc.recevingBranch = x.branchName;
                                pnrList2.Add(x);
                            }

                        }
                        else
                        {
                            x.sc.recevingBranch = x.branchName;
                            pnrList2.Add(x);
                        }

                    }
                    else
                    {
                        x.sc.recevingBranch = x.branchName;
                        pnrList2.Add(x);
                    }




                });

                decimal count = Convert.ToDecimal(pnrList2.Count.ToString());
                pnrList2 = pnrList2.OrderByDescending(x => x.pnrLogId).ToList();
                pnrList2 = pnrList2.Skip(10 * (1 - 1)).Take(10).ToList();

                decimal pages = count / 10;
                ViewBag.pages = (int)Math.Ceiling(pages); ;
                ViewBag.total = count;
                if (count > 0)
                {
                    ViewBag.start = 1;
                }
                else
                {
                    ViewBag.start = 0;
                }
                int end = 5;
                if (end >= count)
                {
                    end = (int)Math.Ceiling(count);
                }
                else
                {

                }
                ViewBag.prev = 1;
                ViewBag.next = 2;
                ViewBag.current = 1;
                ViewBag.length = 10;
            }));
            Task.WaitAll(task.ToArray());
            return View(pnrList2);
        }
        [CheckSessionOut]
        public ActionResult getlog(string length, string pageNum)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var pageSize = int.Parse(length);
            var numPage = int.Parse(pageNum);
            var model = new List<pnrLog>();
            string bn = Session["branchName"].ToString();
            if (string.IsNullOrEmpty(bn))
            {
                model = db.pnrLogs.Where(x => x.idSubscription == idSubcription).OrderByDescending(x => x.pnrLogId).Skip(pageSize * (numPage - 1)).Take(pageSize).ToList();
            }
            else
            {
                model =  db.pnrLogs.Where(x => x.idSubscription == idSubcription ).OrderByDescending(x => x.pnrLogId).Skip(pageSize * (numPage - 1)).Take(pageSize).ToList();
            }

            decimal count = Convert.ToDecimal(db.StockTransfer.ToList().Count.ToString());
            decimal pages = count / 10;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
            ViewBag.start = pageSize * (numPage - 1) + 1;
            int end = 10;
            if (end >= count)
            {
                end = (int)Math.Ceiling(count);
            }
            else
            {

            }
            ViewBag.prev = numPage - 1;
            ViewBag.next = numPage + 1;
            ViewBag.length = length;
            ViewBag.current = numPage;
            return PartialView("_loglist", model);
        }

    }
}