using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            List<pnrLog> pnrList = db.pnrLogs.Where(x=> x.createdAt.Year == DateTime.Now.Year).ToList();
            List<pnrLog> pnrList2 = new List<pnrLog>();
            pnrList.ForEach(x => {
                x.sc = db.SeatConfirmation.Where(sc => sc.pnrNumber == x.pnrNumber && sc.newPnrNumber == null && sc.recevingBranch == x.branchName && x.createdAt.Month == DateTime.Now.Month).FirstOrDefault();
                if(x.sc == null)
                {
                    x.sc = db.SeatConfirmation.Where(sc => sc.newPnrNumber == x.pnrNumber  && sc.recevingBranch == x.branchName && x.createdAt.Month == DateTime.Now.Month).FirstOrDefault();
                    if (x.sc == null)
                    {
                        x.sc = db.SeatConfirmation.Where(sc => sc.newPnrNumber == x.pnrNumber && x.createdAt.Month == DateTime.Now.Month).FirstOrDefault();
                        if (x.sc == null)
                        {
                            x.sc = db.SeatConfirmation.Where(sc => sc.pnrNumber == x.pnrNumber && x.createdAt.Month == DateTime.Now.Month).FirstOrDefault();

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
            return View(pnrList2);
        }
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