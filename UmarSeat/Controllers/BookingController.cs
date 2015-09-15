using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UmarSeat.Models;
using System.IO;
using System.Data.OleDb;
using UmarSeat.Hubs;
using UmarSeat.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Transactions;

namespace UmarSeat.Controllers
{
    [Authorize]
    public class BookingController : ControllerWithHub<BookingHub>
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Booking/
        public async Task<ActionResult> Index(string pnr = "", string airline = "", string category = "", string recevingBranch = "", string stockId = "", string creationRange = "", string outboundRange = "", string timeLimitRange = "")
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            SearchSeatModel sc = new SearchSeatModel();
            sc.pnrNumber = pnr;
            sc.category = category;
            sc.recevingBranch = recevingBranch;
            
            sc.stockId = stockId;
            sc.creationRange = creationRange;
            sc.outBoundRange = outboundRange;
            sc.timeLimitRange = timeLimitRange;
            List<SeatConfirmation> list = null;

            if(sc.pnrNumber !="" || sc.category != "" || sc.recevingBranch !="" || sc.stockId != "" || sc.creationRange !="" || sc.outBoundRange != "" || sc.timeLimitRange !="")
            {
                if (string.IsNullOrEmpty(Session["branchName"].ToString()))
                {
                    list = filterdata(sc, db.SeatConfirmation.Where(x => x.newPnrNumber == null && idSubcription == x.id_Subscription).ToList());
                }               
                else
                {
                    string sb = Session["branchName"].ToString();
                    list = filterdata(sc, db.SeatConfirmation.Where(x => x.newPnrNumber == null && idSubcription == x.id_Subscription && x.recevingBranch == sb).ToList());
                }
                decimal count = Convert.ToDecimal(list.Count.ToString());
                decimal pages = 1;
                ViewBag.pages = (int)Math.Ceiling(pages); ;
                ViewBag.total = count;
                ViewBag.start = 1;
                ViewBag.end = count;
                ViewBag.prev = 1;
                ViewBag.next = 1;
                ViewBag.length = count;
                ViewBag.current = 1;
            }
            else
            {
                if (string.IsNullOrEmpty(Session["branchName"].ToString()))
                {
                    list = db.SeatConfirmation.Where(x => x.newPnrNumber == null && idSubcription == x.id_Subscription).ToList();
                }
                else
                {
                    string sb = Session["branchName"].ToString();
                    list =  db.SeatConfirmation.Where(x => x.newPnrNumber == null && idSubcription == x.id_Subscription && x.recevingBranch == sb).ToList();
                }
                
                decimal count = Convert.ToDecimal(list.ToList().Count.ToString());
                list =  list.OrderByDescending(x => x.id_SeatConfirmation).Skip(5 * (1 - 1)).Take(5).ToList();
                decimal pages = count / 5;
                ViewBag.pages = (int)Math.Ceiling(pages); ;
                ViewBag.total = count;
                ViewBag.start = 1;
             
                ViewBag.prev = 1;
                ViewBag.next = 2;
                ViewBag.current = 1;
                ViewBag.length = 5;

                int end =  5;
                if (end >= count)
                {
                    end = (int)Math.Ceiling(count);
                }
                else
                {

                }
                ViewBag.end = end;
            }
            
            return View(list);
        }
        public async Task<ActionResult> groupsplitlist(string pnr = "", string newpnrnumber = "", string category = "", string recevingBranch = "", string stockId = "", string creationRange = "", string outboundRange = "", string timeLimitRange = "")
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            SearchSeatModel sc = new SearchSeatModel();
            sc.pnrNumber = pnr;
            sc.category = category;
            sc.recevingBranch = recevingBranch;

            sc.stockId = stockId;
            sc.creationRange = creationRange;
            sc.outBoundRange = outboundRange;
            sc.timeLimitRange = timeLimitRange;
            List<SeatConfirmation> list = null;
            if (sc.pnrNumber != "" || sc.category != "" || sc.recevingBranch != "" || sc.stockId != "" || sc.creationRange != "" || sc.outBoundRange != "" || sc.timeLimitRange != "")
            {

                if (string.IsNullOrEmpty(Session["branchName"].ToString()))
                {
                    list = filterdata(sc, db.SeatConfirmation.Where(x => x.newPnrNumber != null && idSubcription == x.id_Subscription).ToList());
                }
                else
                {
                    string sb = Session["branchName"].ToString();
                    list = filterdata(sc, db.SeatConfirmation.Where(x => x.newPnrNumber != null && idSubcription == x.id_Subscription && x.recevingBranch == sb).ToList());
                }
              
                decimal count = Convert.ToDecimal(list.Count.ToString());
                decimal pages = 1;
                ViewBag.pages = (int)Math.Ceiling(pages); ;
                ViewBag.total = count;
                ViewBag.start = 1;
                ViewBag.end = count;
                ViewBag.prev = 1;
                ViewBag.next = 1;
                ViewBag.length = count;
                ViewBag.current = 1;
            }
            else
            {
                if (string.IsNullOrEmpty(Session["branchName"].ToString()))
                {
                    list = db.SeatConfirmation.Where(x => x.newPnrNumber != null && idSubcription == x.id_Subscription).ToList();
                }
                else
                {
                    string sb = Session["branchName"].ToString();
                   list = db.SeatConfirmation.Where(x => x.newPnrNumber != null && idSubcription == x.id_Subscription && x.recevingBranch == sb).ToList();
                }
               
                decimal count = Convert.ToDecimal(list.ToList().Count.ToString());
                list = list.OrderByDescending(x => x.id_SeatConfirmation).Skip(5 * (1 - 1)).Take(5).ToList();
                decimal pages = count / 5;
                ViewBag.pages = (int)Math.Ceiling(pages); ;
                ViewBag.total = count;
                ViewBag.start = 1;
                int end = 5;
                if (end >= count)
                {
                    end = (int)Math.Ceiling(count);
                }
                else
                {

                }
                ViewBag.end = end;
                ViewBag.prev = 1;
                ViewBag.next = 2;
                ViewBag.current = 1;
                ViewBag.length = 5;
            }

            return View(list);
        }

        
        [HttpGet]
        public async Task<ActionResult> GetSeats(string length, string pageNum)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var pageSize = int.Parse(length);
            var numPage = int.Parse(pageNum);
            var model = new List<SeatConfirmation>();
            if (string.IsNullOrEmpty(Session["branchName"].ToString()))
            {
                 model = await db.SeatConfirmation.Where(x => x.newPnrNumber == null && x.id_Subscription == idSubcription).OrderBy(x => x.id_SeatConfirmation).Skip(pageSize * (numPage - 1)).Take(pageSize).ToListAsync();
            }
            else
            {
                string sb = Session["branchName"].ToString();
                model = await db.SeatConfirmation.Where(x => x.newPnrNumber == null && x.id_Subscription == idSubcription && x.recevingBranch == sb).OrderBy(x => x.id_SeatConfirmation).Skip(pageSize * (numPage - 1)).Take(pageSize).ToListAsync();
            }
         
            decimal count = Convert.ToDecimal(db.SeatConfirmation.Where(x => x.newPnrNumber == null && x.id_Subscription == idSubcription).ToList().Count.ToString());
            decimal pages = count / 5;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
            ViewBag.start = pageSize * (numPage - 1) +1;
            int end = 5;
            if (end >= count)
            {
                end = (int)Math.Ceiling(count);
            }
            else
            {

            }
            ViewBag.end = end;
            ViewBag.prev = numPage - 1;
            ViewBag.next = numPage +1;
            ViewBag.length = length;
            ViewBag.current = numPage;
            return PartialView("_sclist", model);
        }
        [HttpPost]
        public ActionResult advanceSearch(SearchSeatModel ssm)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<SeatConfirmation> ss = new List<SeatConfirmation>();
            if (string.IsNullOrEmpty(Session["branchName"].ToString()))
            {
                 ss = filterdata(ssm, db.SeatConfirmation.Where(x => x.newPnrNumber == null && x.id_Subscription == idSubcription).ToList());
            }
            else
            {
                string sb = Session["branchName"].ToString();
                ss = filterdata(ssm, db.SeatConfirmation.Where(x => x.newPnrNumber == null && x.id_Subscription == idSubcription && x.recevingBranch ==sb).ToList());
            }
         
            decimal count = Convert.ToDecimal(ss.Count.ToString());
            decimal pages = 1;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
            ViewBag.start = 1;
            ViewBag.end = count;
            ViewBag.prev = 1;
            ViewBag.next = 1;
            ViewBag.length = count;
            ViewBag.current = 1;

            return PartialView("_sclist", ss);
        }

        private List<SeatConfirmation> filterdata(SearchSeatModel ssm, List<SeatConfirmation> ss)
        {
            
            if (ssm.airline != null && ssm.airline !="")
            {
                ss = ss.Where(x => x.airLine.Contains(ssm.airline)).ToList();
            }
            if (ssm.newPnrNumber != null && ssm.newPnrNumber != "")
            {
                ss = ss.Where(x => x.newPnrNumber.Contains(ssm.newPnrNumber)).ToList();
            }
            if (ssm.pnrNumber != null && ssm.pnrNumber != "")
            {
                ss = ss.Where(x => x.pnrNumber.Contains(ssm.pnrNumber)).ToList();
            }
            if (ssm.category != null && ssm.category != "")
            {
                ss = ss.Where(x => x.category.Contains(ssm.category)).ToList();
            }
            if (ssm.recevingBranch != null && ssm.recevingBranch != "")
            {
                ss = ss.Where(x => x.recevingBranch.Contains(ssm.recevingBranch)).ToList();
            }
            if (ssm.stockId != null && ssm.stockId != "")
            {
                ss = ss.Where(x => x.stockId.Contains(ssm.stockId)).ToList();
            }
            if (ssm.creationRange != null && ssm.creationRange != "")
            {
                string[] dates = ssm.creationRange.Split('-');
                DateTime sdate = Convert.ToDateTime(dates[0].Trim());
                DateTime edate = Convert.ToDateTime(dates[1].Trim());
                ss = ss.Where(x => x.CreatedAt >= sdate && x.CreatedAt <= edate).ToList();
            }

            if (ssm.outBoundRange != null && ssm.outBoundRange != "")
            {
                string[] dates = ssm.outBoundRange.Split('-');
                DateTime sdate = Convert.ToDateTime(dates[0].Trim());
                DateTime edate = Convert.ToDateTime(dates[1].Trim());
                ss = ss.Where(x => x.outBoundDate >= sdate && x.outBoundDate <= edate).ToList();
            }
            if (ssm.timeLimitRange != null && ssm.timeLimitRange != "")
            {
                string[] dates = ssm.timeLimitRange.Split('-');
                DateTime sdate = Convert.ToDateTime(dates[0].Trim());
                DateTime edate = Convert.ToDateTime(dates[1].Trim());
                ss = ss.Where(x => x.timeLimit >= sdate && x.timeLimit <= edate).ToList();
            }
            return ss;
        }





        [HttpGet]
        public async Task<ActionResult> nGetSeats(string length, string pageNum)
        {
            var pageSize = int.Parse(length);
            var numPage = int.Parse(pageNum);
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var model = new List<SeatConfirmation>();
            if (string.IsNullOrEmpty(Session["branchName"].ToString()))
            {
                model = await db.SeatConfirmation.Where(x => x.newPnrNumber != null && x.id_Subscription == idSubcription).OrderByDescending(x => x.id_SeatConfirmation).Skip(pageSize * (numPage - 1)).Take(pageSize).ToListAsync();
            }
            else
            {
                string sb = Session["branchName"].ToString();
                model = await db.SeatConfirmation.Where(x => x.newPnrNumber != null && x.id_Subscription == idSubcription && x.recevingBranch == sb).OrderByDescending(x => x.id_SeatConfirmation).Skip(pageSize * (numPage - 1)).Take(pageSize).ToListAsync();
            }
            decimal count = Convert.ToDecimal(db.SeatConfirmation.Where(x => x.newPnrNumber != null && x.id_Subscription == idSubcription).ToList().Count.ToString());
            decimal pages = count / 5;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
            ViewBag.start = pageSize * (numPage - 1)+1;
            int end = 5;
            if (end >= count)
            {
                end = (int)Math.Ceiling(count);
            }
            else
            {

            }
            ViewBag.end = end;
            ViewBag.prev = numPage - 1;
            ViewBag.next = numPage + 1;
            ViewBag.length = length;
            ViewBag.current = numPage;
            return PartialView("_nsclist", model);
        }
        [HttpPost]
        public ActionResult nadvanceSearch(SearchSeatModel ssm)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<SeatConfirmation> ss = new List<SeatConfirmation>();
            if (string.IsNullOrEmpty(Session["branchName"].ToString()))
            {
                ss = filterdata(ssm, db.SeatConfirmation.Where(x => x.newPnrNumber != null && x.id_Subscription == idSubcription).ToList());
            }
            else
            {
                string sb = Session["branchName"].ToString();
                ss = filterdata(ssm, db.SeatConfirmation.Where(x => x.newPnrNumber != null && x.id_Subscription == idSubcription && x.recevingBranch == sb).ToList());
            }
           
            if (ssm.newPnrNumber != null)
            {

                ss = ss.Where(x => !string.IsNullOrEmpty(x.newPnrNumber) ).ToList();
                ss = ss.Where(x => x.newPnrNumber.Contains(ssm.newPnrNumber)).ToList();
                //ss = (from i in ss where i.newPnrNumber.Contains(ssm.newPnrNumber) select i).ToList();
            }
            if (ssm.pnrNumber != null)
            {
                ss = ss.Where(x => x.pnrNumber.Contains(ssm.pnrNumber)).ToList();
            }
            if (ssm.category != null)
            {
                ss = ss.Where(x => x.category.Contains(ssm.category)).ToList();
            }
            if (ssm.recevingBranch != null)
            {
                ss = ss.Where(x => x.recevingBranch.Contains(ssm.recevingBranch)).ToList();
            }
            if (ssm.stockId != null)
            {
                ss = ss.Where(x => x.stockId.Contains(ssm.stockId)).ToList();
            }
            if (ssm.creationRange != null)
            {
                string[] dates = ssm.creationRange.Split('-');
                DateTime sdate = Convert.ToDateTime(dates[0].Trim());
                DateTime edate = Convert.ToDateTime(dates[1].Trim());
                ss = ss.Where(x => x.CreatedAt >= sdate && x.CreatedAt <= edate).ToList();
            }

            if (ssm.outBoundRange != null)
            {
                string[] dates = ssm.outBoundRange.Split('-');
                DateTime sdate = Convert.ToDateTime(dates[0].Trim());
                DateTime edate = Convert.ToDateTime(dates[1].Trim());
                ss = ss.Where(x => x.outBoundDate >= sdate && x.outBoundDate <= edate).ToList();
            }
            if (ssm.timeLimitRange != null)
            {
                string[] dates = ssm.timeLimitRange.Split('-');
                DateTime sdate = Convert.ToDateTime(dates[0].Trim());
                DateTime edate = Convert.ToDateTime(dates[1].Trim());
                ss = ss.Where(x => x.timeLimit >= sdate && x.timeLimit <= edate).ToList();
            }
            decimal count = Convert.ToDecimal(ss.Count.ToString());
            decimal pages = 1;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
            ViewBag.start = 1;
            ViewBag.end = count;
            ViewBag.prev = 1;
            ViewBag.next = 1;
            ViewBag.length = count;
            ViewBag.current = 1;

            return PartialView("_nsclist", ss);
        }
        // GET: /Booking/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeatConfirmation seatconfirmation = await db.SeatConfirmation.FindAsync(id);
            if (seatconfirmation == null)
            {
                return HttpNotFound();
            }
            return View(seatconfirmation);
        }

        public async Task<ActionResult> scjson(string pnr)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            if (pnr == null || pnr == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeatConfirmation seatconfirmation = await db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.id_Subscription == idSubcription).FirstOrDefaultAsync();
            if (seatconfirmation == null)
            {
                return HttpNotFound();
            }
            return Json(seatconfirmation,JsonRequestBehavior.AllowGet);
        }

        // GET: /Booking/Create
        public ActionResult Entry()
        {
            SeatConfirmation sc = new SeatConfirmation();

            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<airLine> airline = db1.Airline.Where(x=> x.id_Subscription== idSubcription).ToList();
                    db1.Dispose();
                    sc.ListAirline = new List<SelectListItem>();
                    airline.ForEach(x =>
                    {
                        sc.ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
                    });  
                }));
                 tasks.Add(Task.Factory.StartNew(() => {
                     ApplicationDbContext db1 = new ApplicationDbContext();
                     List<Sector> sectors = db1.Sector.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                    sc.ListSectors = new List<SelectListItem>();
                    sectors.ForEach(x => {
                        sc.ListSectors.Add(new SelectListItem { Text = x.sectorName, Value = x.sectorName.ToString() });
                    });
                
                }));
                 tasks.Add(Task.Factory.StartNew(() => {
                     ApplicationDbContext db1 = new ApplicationDbContext();
                     List<branches> branches = db1.Branch.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                    sc.ListBranches = new List<SelectListItem>();
                    branches.ForEach(x =>
                    {
                        sc.ListBranches.Add(new SelectListItem { Text = x.branchName, Value = x.branchName.ToString() });
                    });
                }));
                 tasks.Add(Task.Factory.StartNew(() => {
                     ApplicationDbContext db1 = new ApplicationDbContext();
                     List<Category> Categories = db1.Category.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                    sc.ListCategory = new List<SelectListItem>();
                    Categories.ForEach(x =>
                    {
                        sc.ListCategory.Add(new SelectListItem { Text = x.categoryName, Value = x.categoryName.ToString() });
                    });
                }));
                 tasks.Add(Task.Factory.StartNew(() =>
                 {
                     ApplicationDbContext db1 = new ApplicationDbContext();
                     List<Country> Country = db1.Country.OrderBy(x => x.Name).ToList(); db1.Dispose();
                     sc.ListCountry = new List<SelectListItem>();
                     Country.ForEach(x =>
                     {
                         sc.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                     });
                 }));

                 tasks.Add(Task.Factory.StartNew(() =>
                 {
                     ApplicationDbContext db1 = new ApplicationDbContext();
                     List<Stock> Stocks = db1.Stock.Where(x => x.id_Subscription == idSubcription).OrderBy(x => x.id_Stock).ToList(); db1.Dispose();
                     sc.ListStockId = new List<SelectListItem>();
                     Stocks.ForEach(x =>
                     {
                         sc.ListStockId.Add(new SelectListItem { Text = x.stockName, Value = x.stockName.ToString() });
                     });
                 }));

                 Task.WaitAll(tasks.ToArray());

                
              
            
            return View(sc);
        }

        public async Task<ActionResult> GroupSplit()
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            SeatConfirmation sc = new SeatConfirmation();
            await Task.Factory.StartNew(() =>
            {


                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<airLine> airline = db1.Airline.Where(x => x.id_Subscription == idSubcription).ToList();
                    db1.Dispose();
                    sc.ListAirline = new List<SelectListItem>();
                    airline.ForEach(x =>
                    {
                        sc.ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Sector> sectors = db1.Sector.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                    sc.ListSectors = new List<SelectListItem>();
                    sectors.ForEach(x =>
                    {
                        sc.ListSectors.Add(new SelectListItem { Text = x.sectorName, Value = x.sectorName.ToString() });
                    });

                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<branches> branches = db1.Branch.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                    sc.ListBranches = new List<SelectListItem>();
                    branches.ForEach(x =>
                    {
                        sc.ListBranches.Add(new SelectListItem { Text = x.branchName, Value = x.branchName.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Category> Categories = db1.Category.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                    sc.ListCategory = new List<SelectListItem>();
                    Categories.ForEach(x =>
                    {
                        sc.ListCategory.Add(new SelectListItem { Text = x.categoryName, Value = x.categoryName.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Country> Country = db.Country.OrderBy(x => x.Name).ToList(); db1.Dispose();
                    sc.ListCountry = new List<SelectListItem>();
                    Country.ForEach(x =>
                    {
                        sc.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Stock> Stocks = db1.Stock.Where(x => x.id_Subscription == idSubcription).OrderBy(x => x.id_Stock).ToList(); db1.Dispose();
                    sc.ListStockId = new List<SelectListItem>();
                    Stocks.ForEach(x =>
                    {
                        sc.ListStockId.Add(new SelectListItem { Text = x.stockName, Value = x.stockName.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<pnrLog> pnrAvaliable = new List<pnrLog>();
                    sc.ListPNR = new List<SelectListItem>();
                    if (string.IsNullOrEmpty(Session["branchName"].ToString()))
                    {
                        pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubcription && x.pnrStatus == "Avaliable" ).OrderBy(x => x.pnrLogId).ToList();
                        pnrAvaliable.ForEach(pr => {
                            sc.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber+" ("+pr.branchName+")", Value = pr.pnrNumber+","+pr.branchName });
                        });
                    }
                    else
                    {
                        string sb = Session["branchName"].ToString();
                        pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubcription && x.pnrStatus == "Avaliable" && x.branchName == sb).OrderBy(x => x.pnrLogId).ToList();
                        pnrAvaliable.ForEach(pr => {
                            sc.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber, Value = pr.pnrNumber.ToString() });
                        });
                    }
                   


                    
                    db1.Dispose();

                    
                }));

                Task.WaitAll(tasks.ToArray());



            });
            return View(sc);
        }

        public string getPnr(string pnr)
        {
            if(!String.IsNullOrEmpty(pnr))
            {

                string br = Session["branchName"].ToString();
                if (pnr.Contains(','))
                {
                    string[] pnrbr = pnr.Split(',');
                    pnr = pnrbr[0];
                    br = pnrbr[1];
                }
                int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                pnrLog pl = pnrCalculator.caluclateStats(pnr, br, idSubcription);

                

                    SeatConfirmation st = db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber == null && x.recevingBranch == br).FirstOrDefault();
                    if (st == null)
                    {
                        st = db.SeatConfirmation.Where(x => x.newPnrNumber == pnr ).FirstOrDefault();


                    }
                    st.recevingBranch = br;
                    st.ptype = "Avalible Stock";
                    st.avaliableSeats = st.noOfSeats;
                    Dictionary<string, object> pnrdata = new Dictionary<string, object>();
                    pnrdata.Add("pnrInfo", st);


                    pnrdata.Add("tst", pl.totalSeats-pl.groupSplit-pl.sellSeats-pl.transferSeats);
                    pnrdata.Add("tgs", pl.groupSplit);
                    pnrdata.Add("tss", pl.sellSeats);
                    if(pl.transferSeats>pl.receiveSeats)
                    pnrdata.Add("tts", pl.transferSeats-pl.receiveSeats);
                    else
                    pnrdata.Add("tts", pl.transferSeats);
                pnrdata.Add("tsa", pl.avaliableSeats);
                if (pl.avaliableSeats == 0) return null;
                    return JsonConvert.SerializeObject(pnrdata);

               


            }
            return "";
        }

        private Dictionary<string, object> getPnrStats(string pnr, string br)
        {
            Dictionary<string, object> pnrdata = new Dictionary<string, object>();

            pnrLog pl = db.pnrLogs.Where(x => x.pnrNumber == pnr && x.branchName == br).SingleOrDefault();
            if (pl != null)
            {

                pnrdata.Add("tst", pl.totalSeats);
                pnrdata.Add("tgs", pl.groupSplit);
                pnrdata.Add("tss", pl.sellSeats);
                pnrdata.Add("tts", pl.transferSeats - pl.receiveSeats);
                pnrdata.Add("tsa", pl.avaliableSeats);
            }
            return pnrdata;
        }

        // POST: /Booking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      
        public async Task<string> Entry(SeatConfirmation seatconfirmation)
        {

            ResponseRequest rr = new ResponseRequest();
        
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {
                    var query12 = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errors12 = query12.ToList();
                    if (errors12.Count > 0)
                    {
                        foreach (var e in errors12)
                        {
                            errors.Add(new ResponseRequest() { isSuccess = false, ErrorMessage = e });
                        }
                    }
                    if(seatconfirmation.outBoundDate> seatconfirmation.inBoundDate)
                    {
                        
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "outboundate", ErrorMessage = "Outbound date " + seatconfirmation.outBoundDate + " cannot be less than Inbound date " + seatconfirmation.inBoundDate });
                    }
                    if(seatconfirmation.inBoundSector == seatconfirmation.outBoundSector)
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "inbounder", ErrorMessage = "Outbound Sector " + seatconfirmation.outBoundSector + " cannot be same as Inbound Sector " + seatconfirmation.inBoundSector });
                    }


                    string br = Session["branchName"].ToString();
                    if (seatconfirmation.pnrNumber.Contains(','))
                    {
                        string[] pnrbr = seatconfirmation.pnrNumber.Split(',');
                        seatconfirmation.pnrNumber = pnrbr[0];
                        seatconfirmation.recevingBranch = pnrbr[1];
                    }

                    if (seatconfirmation.pnrNumber != null && errors.Count ==0)
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                            var sc = db.SeatConfirmation.Where(x=> x.pnrNumber == seatconfirmation.pnrNumber && x.newPnrNumber == null &&x.recevingBranch == seatconfirmation.recevingBranch).SingleOrDefault();
                        if(sc == null)
                            {
                                 seatconfirmation.CreatedAt = DateTime.Now;
                                seatconfirmation.UpdatedAt = DateTime.Now;
                                seatconfirmation.pnrStatus = "Avaliable";
                                seatconfirmation.id_Subscription = idSubcription;
                                db.SeatConfirmation.Add(seatconfirmation);
                                await db.SaveChangesAsync();
                                rr.isSuccess = true;
                                rr.Message = "Insert Successfully";
                                errors.Add(rr);
                            }

                        else{
                            rr.isSuccess = false;
                            rr.Message = "Already Exists";
                            errors.Add(rr);
                        }
                           
                       
                   
                    }
                }
                catch (Exception ex)
                {
                    
                    rr.isSuccess = false;
                    rr.Message = "Exception occur: " + ex.ToString() + " " + ex.InnerException.ToString();
                    errors.Add(rr);
                }
                
            }
            else
            {

               
            }

            var query = from state in ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;
            var errors1 = query.ToList();
            if (errors1.Count > 0)
            {
                foreach(var e in errors1)
                {
                    errors.Add(new ResponseRequest() {  isSuccess = false, ErrorMessage = e});
                }
            }
            
          
            return JsonConvert.SerializeObject(errors); ;
            
        }

        [HttpPost]
        
        public async Task<string> GroupSplit( SeatConfirmation seatconfirmation)
        {
            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (seatconfirmation.outBoundDate > seatconfirmation.inBoundDate)
                    {

                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "outboundate", ErrorMessage = "Outbound date " + seatconfirmation.outBoundDate + " cannot be less than Inbound date " + seatconfirmation.inBoundDate });
                    }
                    if (seatconfirmation.inBoundSector == seatconfirmation.outBoundSector)
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "inbounder", ErrorMessage = "Outbound Sector " + seatconfirmation.outBoundSector + " cannot be same as Inbound Sector " + seatconfirmation.inBoundSector });
                    }

                    string br = Session["branchName"].ToString();
                    if (seatconfirmation.pnrNumber.Contains(','))
                    {
                        string[] pnrbr = seatconfirmation.pnrNumber.Split(',');
                        seatconfirmation.pnrNumber = pnrbr[0];
                        seatconfirmation.recevingBranch = pnrbr[1];
                    }

                    pnrLog pl1 = db.pnrLogs.Where(x => x.pnrNumber == seatconfirmation.pnrNumber && x.branchName == seatconfirmation.recevingBranch).SingleOrDefault();
                    if (pl1.avaliableSeats >= seatconfirmation.noOfSeats)
                    {
                        if (errors.Count == 0)
                        {
                           

                            if (seatconfirmation.newPnrNumber != null)
                            {
                                int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());

                                seatconfirmation.CreatedAt = DateTime.Now;
                                seatconfirmation.UpdatedAt = DateTime.Now;
                                seatconfirmation.id_Subscription = idSubcription;
                                seatconfirmation.pnrStatus = "Avaliable";
                              
                                using (TransactionScope ts = new TransactionScope())
                                {

                                    db.SeatConfirmation.Add(seatconfirmation);
                                    await db.SaveChangesAsync();

                                    pnrLog pl = db.pnrLogs.Where(x => x.pnrNumber == seatconfirmation.newPnrNumber && x.branchName == seatconfirmation.recevingBranch).SingleOrDefault();
                                    if (pl != null)
                                    {

                                    }
                                    else
                                    {
                                        pl = new pnrLog();
                                        pl.pnrStatus = "Avaliable";
                                        pl.branchName = seatconfirmation.recevingBranch;
                                        pl.pnrNumber = seatconfirmation.newPnrNumber;
                                        pl.idSubscription = seatconfirmation.id_Subscription;
                                        pl.pnrLock = "";
                                        pl.avaliableSeats = pl.totalSeats = seatconfirmation.noOfSeats;
                                        db.pnrLogs.Add(pl);
                                        db.SaveChanges();

                                    }
                                  
                                    if ((pl1.avaliableSeats - seatconfirmation.noOfSeats) <= 0)
                                    {   
                                        pl1.pnrStatus = "Sold";
                                        pl1.groupSplit = pl1.groupSplit + seatconfirmation.noOfSeats;
                                        pl1.pnrLock = "Locked";
                                        db.Entry(pl1).OriginalValues["RowVersion"] = pl.RowVersion;
                                        db.SaveChanges();


                                        var st = db.SeatConfirmation.Where(x => (x.pnrNumber == seatconfirmation.pnrNumber ||
                                        x.newPnrNumber == seatconfirmation.pnrNumber) && x.recevingBranch == seatconfirmation.recevingBranch).SingleOrDefault();
                                        if (st != null)
                                        {
                                            st.pnrStatus1 = st.pnrStatus = "Sold";
                                            db.Entry(st).OriginalValues["RowVersion"] = st.RowVersion;
                                            db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        pl1.pnrStatus = "Avaliable";
                                        pl1.avaliableSeats = pl1.avaliableSeats - seatconfirmation.noOfSeats;
                                        pl1.groupSplit = pl1.groupSplit + seatconfirmation.noOfSeats;
                                        db.Entry(pl1).OriginalValues["RowVersion"] = pl1.RowVersion;
                                        db.SaveChanges();
                                    }

                                    

                                    rr.isSuccess = true;
                                    rr.Message = "Insert Successfully";
                                    errors.Add(rr);
                                    ts.Complete();
                                }


                               


                            }

 
                        }
                    }
                    else
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "noOfSeats", ErrorMessage = "# of seats greater than # of seats avaliable" });
                    }



                  
                }
                catch (Exception ex)
                {

                    rr.isSuccess = false;
                    rr.Message = "Exception occur: " + ex.ToString() + " " + ex.InnerException.ToString();
                    errors.Add(rr);
                }

            }
            else
            {


            }

            var query = from state in ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;
            var errors1 = query.ToList();
            if (errors1.Count > 0)
            {
                foreach (var e in errors1)
                {
                    errors.Add(new ResponseRequest() { isSuccess = false, ErrorMessage = e });
                }
            }


            return JsonConvert.SerializeObject(errors); ;
        }

        // GET: /Booking/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            SeatConfirmation sc = await db.SeatConfirmation.Where(x => x.id_SeatConfirmation == id && x.id_Subscription == idSubcription).SingleOrDefaultAsync();
            if (sc != null)
            {
               
                await Task.Factory.StartNew(() =>
                {


                    List<Task> tasks = new List<Task>();
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<airLine> airline = db1.Airline.Where(x => x.id_Subscription == idSubcription).ToList();
                        db1.Dispose();
                        sc.ListAirline = new List<SelectListItem>();
                        airline.ForEach(x =>
                        {
                            sc.ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
                        });
                    }));
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<Sector> sectors = db1.Sector.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                        sc.ListSectors = new List<SelectListItem>();
                        sectors.ForEach(x =>
                        {
                            sc.ListSectors.Add(new SelectListItem { Text = x.sectorName, Value = x.sectorName.ToString() });
                        });

                    }));
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<branches> branches = db1.Branch.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                        sc.ListBranches = new List<SelectListItem>();
                        branches.ForEach(x =>
                        {
                            sc.ListBranches.Add(new SelectListItem { Text = x.branchName, Value = x.branchName.ToString() });
                        });
                    }));
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<Category> Categories = db1.Category.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                        sc.ListCategory = new List<SelectListItem>();
                        Categories.ForEach(x =>
                        {
                            sc.ListCategory.Add(new SelectListItem { Text = x.categoryName, Value = x.categoryName.ToString() });
                        });
                    }));
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<Country> Country = db.Country.OrderBy(x => x.Name).ToList(); db1.Dispose();
                        sc.ListCountry = new List<SelectListItem>();
                        Country.ForEach(x =>
                        {
                            sc.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                        });
                    }));
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<Stock> Stocks = db1.Stock.Where(x => x.id_Subscription == idSubcription).OrderBy(x => x.id_Stock).ToList(); db1.Dispose();
                        sc.ListStockId = new List<SelectListItem>();
                        Stocks.ForEach(x =>
                        {
                            sc.ListStockId.Add(new SelectListItem { Text = x.stockName, Value = x.stockName.ToString() });
                        });
                    }));
                    Task.WaitAll(tasks.ToArray());



                });
            }
            return View(sc);
        }

        // POST: /Booking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      
        public async Task<string> Edit(SeatConfirmation seatconfirmation)
        {
            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (seatconfirmation.outBoundDate > seatconfirmation.inBoundDate)
                    {

                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "outboundate", ErrorMessage = "Outbound date " + seatconfirmation.outBoundDate + " cannot be less than Inbound date " + seatconfirmation.inBoundDate });
                    }
                    if (seatconfirmation.inBoundSector == seatconfirmation.outBoundSector)
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "inbounder", ErrorMessage = "Outbound Sector " + seatconfirmation.outBoundSector + " cannot be same as Inbound Sector " + seatconfirmation.inBoundSector });
                    }




                    else if (seatconfirmation.pnrNumber != null)
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                        var sc = db.SeatConfirmation.Where(x => x.id_SeatConfirmation == seatconfirmation.id_SeatConfirmation && x.id_SeatConfirmation== idSubcription).FirstOrDefault();

                        
                        if (sc != null)
                        {
                            sc.UpdatedAt = DateTime.Now;
                            sc.airLine = seatconfirmation.airLine;
                            sc.stockId = seatconfirmation.stockId;
                            sc.outBoundDate = seatconfirmation.outBoundDate;
                            sc.inBoundDate = seatconfirmation.inBoundDate;
                            sc.outBoundSector = seatconfirmation.outBoundSector;
                            sc.inBoundSector = seatconfirmation.inBoundSector;
                            sc.noOfSeats = seatconfirmation.noOfSeats;
                            sc.cost = seatconfirmation.cost;
                            sc.category = seatconfirmation.category;
                            sc.emdNumber = seatconfirmation.emdNumber;
                            sc.timeLimit = seatconfirmation.timeLimit;
                            db.Entry(sc).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            rr.isSuccess = true;
                            rr.Message = "Update Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.ErrorMessage = "Unable to find PNR #";
                            errors.Add(rr);
                        }

                      
                      
                    }
                }
                catch (Exception ex)
                {

                    rr.isSuccess = false;
                    rr.Message = "Exception occur: " + ex.ToString() + " " + ex.InnerException.ToString();
                    errors.Add(rr);
                }

                
            }
            var query = from state in ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;
            var errors1 = query.ToList();
            if (errors1.Count > 0)
            {
                foreach (var e in errors1)
                {
                    errors.Add(new ResponseRequest() { isSuccess = false, ErrorMessage = e });
                }
            }


            return JsonConvert.SerializeObject(errors); ;
        }


        public async Task<ActionResult> groupsplitedit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeatConfirmation sc = await db.SeatConfirmation.FindAsync(id);
            if (sc != null)
            {
                int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                await Task.Factory.StartNew(() =>
                {


                    List<Task> tasks = new List<Task>();
                   
                    tasks.Add(Task.Factory.StartNew(() =>
                    {

                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<Sector> sectors = db1.Sector.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                        sc.ListSectors = new List<SelectListItem>();
                        sectors.ForEach(x =>
                        {
                            sc.ListSectors.Add(new SelectListItem { Text = x.sectorName, Value = x.sectorName.ToString() });
                        });

                    }));
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<branches> branches = db1.Branch.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                        sc.ListBranches = new List<SelectListItem>();
                        branches.ForEach(x =>
                        {
                            sc.ListBranches.Add(new SelectListItem { Text = x.branchName, Value = x.branchName.ToString() });
                        });
                    }));
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<Category> Categories = db1.Category.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                        sc.ListCategory = new List<SelectListItem>();
                        Categories.ForEach(x =>
                        {
                            sc.ListCategory.Add(new SelectListItem { Text = x.categoryName, Value = x.categoryName.ToString() });
                        });
                    }));
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<Country> Country = db.Country.OrderBy(x => x.Name).ToList(); db1.Dispose();
                        sc.ListCountry = new List<SelectListItem>();
                        Country.ForEach(x =>
                        {
                            sc.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                        });
                    }));
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<Stock> Stocks = db1.Stock.Where(x => x.id_Subscription == idSubcription).OrderBy(x => x.id_Stock).ToList(); db1.Dispose();
                        sc.ListStockId = new List<SelectListItem>();
                        Stocks.ForEach(x =>
                        {
                            sc.ListStockId.Add(new SelectListItem { Text = x.stockName, Value = x.stockName.ToString() });
                        });
                    }));

                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        ApplicationDbContext db1 = new ApplicationDbContext();
                        List<pnrLog> pnrAvaliable = new List<pnrLog>();
                        sc.ListPNR = new List<SelectListItem>();
                        if (string.IsNullOrEmpty(Session["branchName"].ToString()))
                        {
                            //   Seatconfirmations = db1.SeatConfirmation.Where(x => x.id_Subscription == idSubcription && x.newPnrNumber == null && x.pnrStatus == "Avaliable" ).OrderBy(x => x.id_SeatConfirmation).ToList();
                        }
                        else
                        {
                            string sb = Session["branchName"].ToString();
                            pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubcription && x.pnrStatus == "Avaliable" && x.branchName == sb).OrderBy(x => x.pnrLogId).ToList();

                        }
                        pnrAvaliable.ForEach(pr =>
                        {
                            sc.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber, Value = pr.pnrNumber.ToString() });
                        });


                    }));
                    Task.WaitAll(tasks.ToArray());



                });
            }
            return View(sc);
        }

        // POST: /Booking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<string> groupsplitedit(SeatConfirmation seatconfirmation)
        {
            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (seatconfirmation.outBoundDate > seatconfirmation.inBoundDate)
                    {

                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "outboundate", ErrorMessage = "Outbound date " + seatconfirmation.outBoundDate + " cannot be less than Inbound date " + seatconfirmation.inBoundDate });
                    }
                    if (seatconfirmation.inBoundSector == seatconfirmation.outBoundSector)
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "inbounder", ErrorMessage = "Outbound Sector " + seatconfirmation.outBoundSector + " cannot be same as Inbound Sector " + seatconfirmation.inBoundSector });
                    }


                    string br = Session["branchName"].ToString();
                    if (seatconfirmation.pnrNumber.Contains(','))
                    {
                        string[] pnrbr = seatconfirmation.pnrNumber.Split(',');
                        seatconfirmation.pnrNumber = pnrbr[0];
                        seatconfirmation.recevingBranch = pnrbr[1];
                    }

                    else if (seatconfirmation.newPnrNumber != null)
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                        var sc = db.SeatConfirmation.Where(x => x.id_SeatConfirmation == seatconfirmation.id_SeatConfirmation && x.id_Subscription == idSubcription).FirstOrDefault();


                        if (sc != null)
                        {
                            sc.UpdatedAt = DateTime.Now;
                            
                            sc.stockId = seatconfirmation.stockId;
                            sc.outBoundDate = seatconfirmation.outBoundDate;
                            sc.inBoundDate = seatconfirmation.inBoundDate;
                            sc.outBoundSector = seatconfirmation.outBoundSector;
                            sc.inBoundSector = seatconfirmation.inBoundSector;
                            sc.noOfSeats = seatconfirmation.noOfSeats;
                            sc.cost = seatconfirmation.cost;
                            sc.category = seatconfirmation.category;
                            sc.emdNumber = seatconfirmation.emdNumber;
                            sc.timeLimit = seatconfirmation.timeLimit;
                            db.Entry(sc).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            rr.isSuccess = true;
                            rr.Message = "Update Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.ErrorMessage = "Unable to find New PNR #";
                            errors.Add(rr);
                        }



                    }
                }
                catch (Exception ex)
                {

                    rr.isSuccess = false;
                    rr.Message = "Exception occur: " + ex.ToString() + " " + ex.InnerException.ToString();
                    errors.Add(rr);
                }


            }
            var query = from state in ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;
            var errors1 = query.ToList();
            if (errors1.Count > 0)
            {
                foreach (var e in errors1)
                {
                    errors.Add(new ResponseRequest() { isSuccess = false, ErrorMessage = e });
                }
            }


            return JsonConvert.SerializeObject(errors); ;
        }

        // GET: /Booking/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            SeatConfirmation seatconfirmation = await db.SeatConfirmation.Where(x=> x.id_Subscription == idSubcription && x.id_SeatConfirmation== id).FirstOrDefaultAsync();
            if (seatconfirmation == null)
            {
                return HttpNotFound();
            }
            return View(seatconfirmation);
        }


        public async Task<ActionResult> groupsplitdelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            SeatConfirmation seatconfirmation = await db.SeatConfirmation.Where(x => x.id_Subscription == idSubcription && x.id_SeatConfirmation == id).FirstOrDefaultAsync();
            if (seatconfirmation == null)
            {
                return HttpNotFound();
            }
            return View(seatconfirmation);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            SeatConfirmation seatconfirmation = await db.SeatConfirmation.Where(x => x.id_Subscription == idSubcription && x.id_SeatConfirmation == id).FirstOrDefaultAsync();
            db.SeatConfirmation.Remove(seatconfirmation);
            await db.SaveChangesAsync();
            return null;
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
           string fileName = null;
            if (file != null && file.ContentLength > 0)
            {
                 fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                var extension = Path.GetExtension(path);
                if (extension == ".xlsx")
                    file.SaveAs(path);
            }
            return RedirectToAction("View", new { fileName = fileName });
        }
        public ActionResult View(string fileName)
        {
            fileName = "~/App_Data/" + fileName;
            string path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", Server.MapPath(fileName));

            //Fill the DataSet by the Sheets.
            var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
            var ds = new DataSet();
            List<string> colums = new List<string>();
            DataTable loadDT = new DataTable();

            List<SeatConfirmation> seatconfirmationList = new List<SeatConfirmation>();

            adapter.Fill(ds, "SeatConfirmation");
            DataTable data = ds.Tables["SeatConfirmation"];
            for (int i = 0; i < data.Rows.Count;i++ )
            {
                if(data.Rows[i][0].ToString() != "")
                {
                    SeatConfirmation sc = new SeatConfirmation();
                    sc.pnrNumber = data.Rows[i][0].ToString();
                    sc.airLine = data.Rows[i][1].ToString();
                    sc.stockId = data.Rows[i][2].ToString();
                    sc.outBoundDate = Convert.ToDateTime(data.Rows[i][3].ToString());
                    sc.inBoundDate = Convert.ToDateTime(data.Rows[i][4].ToString());
                    sc.outBoundSector = data.Rows[i][5].ToString();
                    sc.inBoundSector = data.Rows[i][6].ToString();
                    sc.noOfSeats = int.Parse(data.Rows[i][7].ToString());
                    sc.cost = int.Parse(data.Rows[i][8].ToString());
                    sc.category = data.Rows[i][9].ToString();
                    sc.recevingBranch = data.Rows[i][10].ToString();
                    sc.CreatedAt = sc.UpdatedAt = sc.timeLimit = Convert.ToDateTime(data.Rows[i][11].ToString());
                    int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                    sc.id_Subscription = idSubcription;
                    seatconfirmationList.Add(sc);

                    SeatConfirmation sc2 = db.SeatConfirmation.Where(x=> x.pnrNumber.ToLower() == sc.pnrNumber.ToLower()).FirstOrDefault();
                    if(sc2 == null)
                    {
                        List<Task> tasks = new List<Task>();
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                ApplicationDbContext db1 = new ApplicationDbContext();
                            airLine Air_line = db1.Airline.Where(x => x.airlineName.ToLower() == sc.airLine.ToLower()).FirstOrDefault();
                            if (Air_line == null)
                            {
                              
                                Air_line = new airLine() { airlineName = sc.airLine, id_Subscription = 1002, createdAt = DateTime.Now, };
                                db1.Airline.Add(Air_line);
                                db1.SaveChanges();
                                db1.Dispose();
                            }
                            }
                            catch (Exception)
                            {
                                
                                throw;
                            }

                        }));
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            try
                            {
                            ApplicationDbContext db1 = new ApplicationDbContext();
                            Sector _sector = db1.Sector.Where(x => x.sectorName.ToLower() == sc.outBoundSector.ToLower()).FirstOrDefault();
                            if (_sector == null)
                            {
                                
                                _sector = new Sector() { sectorName = sc.outBoundSector, id_Subscription = 1002 };
                                db1.Sector.Add(_sector);
                                db1.SaveChanges();
                                db1.Dispose();
                            }
                            }
                            catch (Exception)
                            {
                                
                                throw;
                            }
                        }));
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            try
                            {
                            ApplicationDbContext db1 = new ApplicationDbContext();
                            Sector _sector = db1.Sector.Where(x => x.sectorName.ToLower() == sc.inBoundSector.ToLower()).FirstOrDefault();
                            if (_sector == null)
                            {
                              
                                _sector = new Sector() { sectorName = sc.inBoundSector, id_Subscription = 1002 };
                                db1.Sector.Add(_sector);
                                db1.SaveChanges();
                                db1.Dispose();
                            }
                            }
                            catch (Exception)
                            {
                                
                                throw;
                            }
                        }));
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                string b = sc.recevingBranch;
                            string city = "";
                            if(b.ToLower().Contains("branch"))
                            {

                            }
                            else
                            {
                                city = b;
                                b = b + " Branch";
                            } ApplicationDbContext db1 = new ApplicationDbContext();
                            branches _branch = db1.Branch.Where(x => x.branchName.ToLower().Contains(sc.recevingBranch.ToLower())).FirstOrDefault();
                            if (_branch == null)
                            {
                              
                                _branch = new branches() { branchName = sc.recevingBranch, CreatedAt = DateTime.Now, id_Subscription = 1002 , branchCity = b};
                                db1.Branch.Add(_branch);
                                db1.SaveChanges();
                                db1.Dispose();
                            }
                            }
                            catch (Exception)
                            {
                                
                                throw;
                            }


                        }));

                        try
                        {
                              Task.WaitAll(tasks.ToArray());
                        db.SeatConfirmation.Add(sc);
                        db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            
                            throw;
                        }
                    }

                   
                  




                }
            }
            return null;
        }
    }
    public class ResponseRequest
    {
        public string Element { get; set; }
        public bool isSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public string Message { get; set; }
    }
}
