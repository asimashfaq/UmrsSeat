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
using Newtonsoft.Json;
using UmarSeat.Helpers;
using System.Transactions;

namespace UmarSeat.Controllers
{
    [Authorize]
    
    public class StockController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Stock/
        [ActionName("Selling")]
        [CheckSessionOut]
        public async Task<ActionResult> Index(string pnr, string catalystinvoicenumber, string airline, string stockid, string agentid, string gdspnrnumber, string advancerange, string creationrange)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            SearchStockModel ssm = new SearchStockModel();
            ssm.pnrNumber = pnr;
            ssm.catalystInvoiceNumber = catalystinvoicenumber;
            ssm.airLine = airline;
            ssm.stockId = stockid;
            ssm.idAgent = agentid;
            ssm.gdsPnrNumber = gdspnrnumber;
            ssm.advanceRange = advancerange;
            ssm.creationRange = creationrange;
            List<StockTransfer> list;
            if(ssm.pnrNumber != null || ssm.catalystInvoiceNumber != null || ssm.airLine != null || ssm.stockId != null || ssm.idAgent != null || ssm.gdsPnrNumber != null || ssm.advanceRange != null  && ssm.creationRange != null)
            {
                string bn = Session["branchName"].ToString();
                if(string.IsNullOrEmpty(bn))
                {
                    list = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == true && x.id_Subscription == idSubcription).OrderByDescending(x => x.id_StockTransfer).ToList());
                }
                else
                {
                    list = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == true && x.id_Subscription == idSubcription && x.transferingBranch == bn).OrderByDescending(x => x.id_StockTransfer).ToList());
                }

                
                decimal count = Convert.ToDecimal(list.Count.ToString());
                decimal pages = 1;
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
              
                ViewBag.end = count;
                ViewBag.prev = 1;
                ViewBag.next = 1;
                ViewBag.length = count;
                ViewBag.current = 1;
            }
            else
            {
                string bn = Session["branchName"].ToString();
                if (string.IsNullOrEmpty(bn))
                {
                    list = await db.StockTransfer.Where(x => x.advanceDate.HasValue == true && x.id_Subscription == idSubcription).OrderByDescending(x => x.id_StockTransfer).ToListAsync();
                }
                else
                {
                    list = await db.StockTransfer.Where(x => x.advanceDate.HasValue == true && x.id_Subscription == idSubcription && x.transferingBranch == bn).OrderByDescending(x => x.id_StockTransfer).ToListAsync();
                }
               
                decimal count = Convert.ToDecimal(list.Count.ToString());
                list = list.Skip(5 * (1 - 1)).Take(5).ToList();
                decimal pages = count / 5;
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
                ViewBag.length = 5;
            }

          
            return View(list);
        }
        [CheckSessionOut]
        public async Task<ActionResult> Transferlist(string pnr, string transferingbranch, string recevingbranch, string airline, string stockid, string creationrange)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            SearchStockModel ssm = new SearchStockModel();
            ssm.pnrNumber = pnr;
            ssm.transferingBranch = transferingbranch;
            ssm.recevingBranch = recevingbranch;
            ssm.airLine = airline;
            ssm.stockId = stockid;
            ssm.creationRange = creationrange;
            List<StockTransfer> list;
            if(ssm.pnrNumber != null || ssm.transferingBranch != null || ssm.recevingBranch != null || ssm.airLine != null || ssm.stockId != null || ssm.creationRange != null)
            {
                string bn = Session["branchName"].ToString();
                if (string.IsNullOrEmpty(bn))
                {
                    list = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription).OrderByDescending(x => x.id_StockTransfer).ToList());
                }
                else
                {
                    list = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription && x.transferingBranch == bn).OrderBy(x=> x.id_Subscription).ToList());
                }
                
                decimal count = Convert.ToDecimal(list.Count.ToString());
                decimal pages = 1;
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
                ViewBag.end = count;
                ViewBag.prev = 1;
                ViewBag.next = 1;
                ViewBag.length = count;
                ViewBag.current = 1;
            }
            else
            {
                string bn = Session["branchName"].ToString();
                if (string.IsNullOrEmpty(bn))
                {
                    list = await db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription).ToListAsync();
                }
                else
                {
                    list = await db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription  && x.transferingBranch == bn).OrderByDescending(x => x.id_StockTransfer).ToListAsync();
                }

               
                 decimal count = Convert.ToDecimal(list.Count.ToString());
                list = list.Skip(5 * (1 - 1)).Take(5).ToList();
                decimal pages = count / 5;
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
                ViewBag.length = 5;
            }
           
            return View(list);
        }
        [CheckSessionOut]
        public async Task<ActionResult> receive(string pnr, string transferingbranch, string recevingbranch, string airline, string stockid, string creationrange)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            SearchStockModel ssm = new SearchStockModel();
            ssm.pnrNumber = pnr;
            ssm.transferingBranch = transferingbranch;
            ssm.recevingBranch = recevingbranch;
            ssm.airLine = airline;
            ssm.stockId = stockid;
            ssm.creationRange = creationrange;
            List<StockTransfer> list;
            if (ssm.pnrNumber != null || ssm.transferingBranch != null || ssm.recevingBranch != null || ssm.airLine != null || ssm.stockId != null || ssm.creationRange != null)
            {
                string bn = Session["branchName"].ToString();
                if (string.IsNullOrEmpty(bn))
                {
                    list = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription).OrderByDescending(x => x.id_StockTransfer).ToList());
                }
                else
                {
                    list = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription && x.recevingBranch == bn).OrderByDescending(x => x.id_StockTransfer).ToList());
                }

                decimal count = Convert.ToDecimal(list.Count.ToString());
                decimal pages = 1;
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
                ViewBag.end = count;
                ViewBag.prev = 1;
                ViewBag.next = 1;
                ViewBag.length = count;
                ViewBag.current = 1;
            }
            else
            {
                string bn = Session["branchName"].ToString();
                if (string.IsNullOrEmpty(bn))
                {
                    list = await db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription).ToListAsync();
                }
                else
                {
                    list = await db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription && x.recevingBranch == bn).OrderByDescending(x => x.id_StockTransfer).ToListAsync();
                }


                decimal count = Convert.ToDecimal(list.Count.ToString());
                list = list.Skip(5 * (1 - 1)).Take(5).ToList();
                decimal pages = count / 5;
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
                ViewBag.length = 5;
            }

            return View(list);
        }


        [HttpGet]
        [CheckSessionOut]
        public async Task<ActionResult> GetStock(string length, string pageNum)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var pageSize = int.Parse(length);
            var numPage = int.Parse(pageNum);
            var model = new List<StockTransfer>();
            string bn = Session["branchName"].ToString();
            if (string.IsNullOrEmpty(bn))
            {
                model = await db.StockTransfer.Where(x => x.id_Subscription == idSubcription).OrderBy(x => x.id_StockTransfer).Skip(pageSize * (numPage - 1)).Take(pageSize).ToListAsync();
            }
            else
            {
                model = await db.StockTransfer.Where(x => x.id_Subscription == idSubcription && x.transferingBranch ==bn).OrderBy(x => x.id_StockTransfer).Skip(pageSize * (numPage - 1)).Take(pageSize).ToListAsync();
            }

            decimal count = Convert.ToDecimal(db.StockTransfer.ToList().Count.ToString());
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
            ViewBag.prev = numPage - 1;
            ViewBag.next = numPage + 1;
            ViewBag.length = length;
            ViewBag.current = numPage;
            return PartialView("_stlist", model);
        }

        [HttpGet]
        [CheckSessionOut]
        public async Task<ActionResult> nGetStock(string length, string pageNum)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var pageSize = int.Parse(length);
            var numPage = int.Parse(pageNum);
            var model = new List<StockTransfer>();
            string bn = Session["branchName"].ToString();
            if (string.IsNullOrEmpty(bn))
            {
                model = await db.StockTransfer.Where(x => x.id_Subscription == idSubcription).OrderBy(x => x.id_StockTransfer).Skip(pageSize * (numPage - 1)).Take(pageSize).ToListAsync();
            }
            else
            {
                model = await db.StockTransfer.Where(x => x.id_Subscription == idSubcription && x.transferingBranch == bn).OrderBy(x => x.id_StockTransfer ).Skip(pageSize * (numPage - 1)).Take(pageSize).ToListAsync();
            }
            decimal count = Convert.ToDecimal(db.StockTransfer.ToList().Count.ToString());
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
            ViewBag.prev = numPage - 1;
            ViewBag.next = numPage + 1;
            ViewBag.length = length;
            ViewBag.current = numPage;
            return PartialView("_nstlist", model);
        }


        [HttpGet]
        [CheckSessionOut]
        public async Task<ActionResult> rGetStock(string length, string pageNum)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var pageSize = int.Parse(length);
            var numPage = int.Parse(pageNum);
            var model = new List<StockTransfer>();
            string bn = Session["branchName"].ToString();
            if (string.IsNullOrEmpty(bn))
            {
                model = await db.StockTransfer.Where(x => x.id_Subscription == idSubcription).OrderBy(x => x.id_StockTransfer).Skip(pageSize * (numPage - 1)).Take(pageSize).ToListAsync();
            }
            else
            {
                model = await db.StockTransfer.Where(x => x.id_Subscription == idSubcription && x.recevingBranch == bn).OrderBy(x => x.id_StockTransfer).Skip(pageSize * (numPage - 1)).Take(pageSize).ToListAsync();
            }
            decimal count = Convert.ToDecimal(db.StockTransfer.ToList().Count.ToString());
            decimal pages = count / 5;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
            ViewBag.start = pageSize * (numPage - 1) + 1;
            int end = 5;
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
            return PartialView("_srlist", model);
        }
        [CheckSessionOut]
        public async Task<ActionResult> stjson(int stockTransferId)
        {
            if (stockTransferId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            StockTransfer st = await db.StockTransfer.Where(x => x.id_StockTransfer == stockTransferId && x.id_Subscription == idSubcription).FirstOrDefaultAsync();
            if (st == null)
            {
                return HttpNotFound();
            }
            return Json(st, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CheckSessionOut]
        public ActionResult advanceSearch(SearchStockModel ssm)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<StockTransfer> ss = new List<StockTransfer>();
            string bn = Session["branchName"].ToString();
            if (string.IsNullOrEmpty(bn))
            {
                ss = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == true && x.id_Subscription == idSubcription).OrderByDescending(x => x.id_StockTransfer).ToList());
            }
            else
            {
                ss = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == true && x.id_Subscription == idSubcription && x.transferingBranch == bn).OrderByDescending(x => x.id_StockTransfer).ToList());
            }
            ss = ss.Where(x => x.advanceDate.HasValue == true).ToList();
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

            return PartialView("_stlist", ss);
        }
        [CheckSessionOut]
        public ActionResult nadvanceSearch(SearchStockModel ssm)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<StockTransfer> ss = new List<StockTransfer>();
            string bn = Session["branchName"].ToString();
            if (string.IsNullOrEmpty(bn))
            {
                ss = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription).OrderByDescending(x => x.id_StockTransfer).ToList());
            }
            else
            {
                ss = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription && x.transferingBranch == bn).OrderByDescending(x => x.id_StockTransfer).ToList());
            }
            ss = ss.Where(x => x.advanceDate.HasValue == false).ToList();
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

            return PartialView("_nstlist", ss);
        }
        [CheckSessionOut]
        public ActionResult radvanceSearch(SearchStockModel ssm)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<StockTransfer> ss = new List<StockTransfer>();
            string bn = Session["branchName"].ToString();
            if (string.IsNullOrEmpty(bn))
            {
                ss = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription).OrderByDescending(x => x.id_StockTransfer).ToList());
            }
            else
            {
                ss = filterdata(ssm, db.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.id_Subscription == idSubcription && x.recevingBranch == bn).OrderByDescending(x => x.id_StockTransfer).ToList());
            }
            ss = ss.Where(x => x.advanceDate.HasValue == false).ToList();
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

            return PartialView("_srlist", ss);
        }


        private List<StockTransfer> filterdata(SearchStockModel ssm, List<StockTransfer> ss)
        {
           
            if (ssm.airLine != null && ssm.airLine != "")
            {
                ss = ss.Where(x => x.airLine.Contains(ssm.airLine)).ToList();
            }
           
            if (ssm.pnrNumber != null && ssm.pnrNumber != "")
            {
                ss = ss.Where(x => x.pnrNumber.Contains(ssm.pnrNumber)).ToList();
            }
            if (ssm.gdsPnrNumber != null && ssm.gdsPnrNumber != "")
            {
                ss = ss.Where(x => x.gdsPnrNumber.Contains(ssm.gdsPnrNumber)).ToList();
            }
            if (ssm.catalystInvoiceNumber != null && ssm.catalystInvoiceNumber != "")
            {
                ss = ss.Where(x => x.catalystInvoiceNumber.Contains(ssm.catalystInvoiceNumber)).ToList();
            }
            if (ssm.sellingBranch != null && ssm.recevingBranch != "")
            {
                ss = ss.Where(x => x.sellingBranch.Contains(ssm.sellingBranch)).ToList();
            }
            if (ssm.stockId != null && ssm.stockId != "")
            {
                ss = ss.Where(x => x.stockId.Contains(ssm.stockId)).ToList();
            }
            if (ssm.idAgent != null && ssm.idAgent != "")
            {
                ss = ss.Where(x => x.idAgent.Contains(ssm.idAgent)).ToList();
            }
            if (ssm.transferingBranch != null && ssm.transferingBranch != "")
            {
                ss = ss.Where(x => x.transferingBranch.Contains(ssm.transferingBranch)).ToList();
            }
            if (ssm.creationRange != null && ssm.creationRange != "")
            {
                string[] dates = ssm.creationRange.Split('-');
                DateTime sdate = Convert.ToDateTime(dates[0].Trim());
                DateTime edate = Convert.ToDateTime(dates[1].Trim());
                ss = ss.Where(x => x.createAt.Date >= sdate && x.createAt.Date <= edate).ToList();
            }

            if (ssm.advanceRange != null && ssm.advanceRange != "")
            {
                string[] dates = ssm.advanceRange.Split('-');
                DateTime sdate = Convert.ToDateTime(dates[0].Trim());
                DateTime edate = Convert.ToDateTime(dates[1].Trim());
                ss = ss.Where(x => x.advanceDate.Value.Date >= sdate && x.advanceDate.Value.Date <= edate).ToList();
            }
           
            return ss;
        }

        [CheckSessionOut]
        // GET: /Stock/Create
        public async Task<ActionResult> sellingcreate()
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            StockTransfer st = new StockTransfer();
            await Task.Factory.StartNew(() =>
            {


                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<airLine> airline = db1.Airline.Where(x=> x.id_Subscription == idSubcription).ToList();
                    db1.Dispose();
                    st.ListAirline = new List<SelectListItem>();
                    airline.ForEach(x =>
                    {
                        st.ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Agents> agents = db1.Agent.Include(x => x.Person).Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                    st.ListAgents = new List<SelectListItem>();
                    agents.ForEach(x =>
                    {
                        st.ListAgents.Add(new SelectListItem { Text = x.Person.firstName+" "+x.Person.lastName+" ("+x.id_Agent+")", Value = x.id_Agent.ToString() });
                    });

                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<branches> branches = db1.Branch.Where(x => x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                    st.ListBranches = new List<SelectListItem>();
                    branches.ForEach(x =>
                    {
                        st.ListBranches.Add(new SelectListItem { Text = x.branchName, Value = x.branchName.ToString() });
                    });
                }));
               
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Country> Country = db.Country.OrderBy(x => x.Name).ToList(); db1.Dispose();
                    st.ListCountry = new List<SelectListItem>();
                    Country.ForEach(x =>
                    {
                        st.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Stock> Stocks = db1.Stock.Where(x => x.id_Subscription == idSubcription).OrderBy(x => x.id_Stock).ToList(); db1.Dispose();
                    st.ListStockId = new List<SelectListItem>();
                    Stocks.ForEach(x =>
                    {
                        st.ListStockId.Add(new SelectListItem { Text = x.stockName, Value = x.stockName.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();

                    List<pnrLog> pnrAvaliable = new List<pnrLog>();
                    st.ListPNR = new List<SelectListItem>();
                    if (string.IsNullOrEmpty(Session["branchName"].ToString()))
                    {
                        pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubcription && x.pnrStatus == "Avaliable").OrderBy(x => x.pnrLogId).ToList();
                        pnrAvaliable.ForEach(pr =>
                        {

                            
                            st.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber + "(" + pr.branchName + ")", Value = pr.pnrNumber + "," + pr.branchName });
                        });
                    }
                    else
                    {
                        string sb = Session["branchName"].ToString();
                        pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubcription && x.pnrStatus == "Avaliable" && x.branchName == sb).OrderBy(x => x.pnrLogId).ToList();
                        pnrAvaliable.ForEach(pr =>
                        {

                            st.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber, Value = pr.pnrNumber });
                        });

                    }

                 


                    db1.Dispose();

                }));

                Task.WaitAll(tasks.ToArray());



            });
            return View(st);
        }
        [CheckSessionOut]
        public async Task<ActionResult> Transfercreate()
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            StockTransfer st = new StockTransfer();
            await Task.Factory.StartNew(() =>
            {


                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<airLine> airline = db1.Airline.Where(x=> x.id_Subscription == idSubcription).ToList();
                    db1.Dispose();
                    st.ListAirline = new List<SelectListItem>();
                    airline.ForEach(x =>
                    {
                        st.ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
                    });
                }));
               
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<branches> branches = db1.Branch.Where(x=> x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                    st.ListBranches = new List<SelectListItem>();
                    branches.ForEach(x =>
                    {
                        st.ListBranches.Add(new SelectListItem { Text = x.branchName, Value = x.branchName.ToString() });
                    });
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Country> Country = db.Country.OrderBy(x => x.Name).ToList(); db1.Dispose();
                    st.ListCountry = new List<SelectListItem>();
                    Country.ForEach(x =>
                    {
                        st.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Stock> Stocks = db1.Stock.Where(x => x.id_Subscription == idSubcription).OrderBy(x => x.id_Stock).ToList(); db1.Dispose();
                    st.ListStockId = new List<SelectListItem>();
                    Stocks.ForEach(x =>
                    {
                        st.ListStockId.Add(new SelectListItem { Text = x.stockName, Value = x.stockName.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();

                    List<pnrLog> pnrAvaliable = new List<pnrLog>();
                    st.ListPNR = new List<SelectListItem>();
                    if (string.IsNullOrEmpty(Session["branchName"].ToString()))
                    {
                        pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubcription && x.pnrStatus == "Avaliable").OrderBy(x => x.pnrLogId).ToList();
                        pnrAvaliable.ForEach(pr =>
                        {

                            st.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber + "(" + pr.branchName + ")", Value = pr.pnrNumber + "," + pr.branchName });
                        });
                    }
                    else
                    {
                        string sb = Session["branchName"].ToString();
                        pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubcription && x.pnrStatus == "Avaliable" && x.branchName == sb).OrderBy(x => x.pnrLogId).ToList();
                        pnrAvaliable.ForEach(pr =>
                        {
                            st.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber, Value = pr.pnrNumber });
                            
                        });

                    }


                    db1.Dispose();

                }));
                Task.WaitAll(tasks.ToArray());



            });
            return View(st);
        }
        [CheckSessionOut]
        public async Task<ActionResult> TransferEdit(int? id)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            StockTransfer st =  db.StockTransfer.Where(x => x.id_StockTransfer == id && x.id_Subscription == idSubcription).FirstOrDefault();
            await Task.Factory.StartNew(() =>
            {


                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<airLine> airline = db1.Airline.Where(x=> x.id_Subscription == idSubcription).ToList();
                    db1.Dispose();
                    st.ListAirline = new List<SelectListItem>();
                    airline.ForEach(x =>
                    {
                        st.ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
                    });
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<branches> branches = db1.Branch.Where(x=> x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                    st.ListBranches = new List<SelectListItem>();
                    branches.ForEach(x =>
                    {
                        st.ListBranches.Add(new SelectListItem { Text = x.branchName, Value = x.branchName.ToString() });
                    });
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Country> Country = db.Country.OrderBy(x => x.Name).ToList(); db1.Dispose();
                    st.ListCountry = new List<SelectListItem>();
                    Country.ForEach(x =>
                    {
                        st.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Stock> Stocks = db1.Stock.Where(x => x.id_Subscription == idSubcription).OrderBy(x => x.id_Stock).ToList(); db1.Dispose();
                    st.ListStockId = new List<SelectListItem>();
                    Stocks.ForEach(x =>
                    {
                        st.ListStockId.Add(new SelectListItem { Text = x.stockName, Value = x.stockName.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                   
                    st.ListPNR = new List<SelectListItem>();

                    st.ListPNR.Add(new SelectListItem { Text = st.pnrNumber, Value = st.pnrNumber });


                }));
                Task.WaitAll(tasks.ToArray());



            });
            return View(st);
        }
        
        [HttpPost]
        [CheckSessionOut]
        public async Task<string> sellingcreate(StockTransfer stocktransfer)
        {
            
            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (String.IsNullOrEmpty(stocktransfer.pnrNumber))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "pnrNumber", ErrorMessage = "PNR # Cannot be null " });
                    }
                    if (String.IsNullOrEmpty(stocktransfer.airLine))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "airLine", ErrorMessage = "Airline Cannot be null " });
                    }
                    if (String.IsNullOrEmpty(stocktransfer.stockId))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "stockId", ErrorMessage = "StockId Cannot be null " });
                    }
                    if (stocktransfer.pnrNumber.Contains(','))
                    {
                        string[] splitPnr = stocktransfer.pnrNumber.Split(',');
                        stocktransfer.pnrNumber = splitPnr[0];
                    }
                    int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                    pnrLog pl1 = db.pnrLogs.Where(x => x.pnrNumber == stocktransfer.pnrNumber && x.branchName == stocktransfer.sellingBranch && x.idSubscription == idSubcription).SingleOrDefault();
                    if(pl1 == null)
                    {
                        pl1 = new pnrLog();
                    }

                    if (pl1.avaliableSeats >= stocktransfer.noOfSeats)
                    {
                        if (errors.Count == 0)
                        {
                            

                            stocktransfer.createAt = DateTime.Now;
                            stocktransfer.UpdateAt = DateTime.Now;
                            stocktransfer.id_Subscription = idSubcription;
                            using (TransactionScope ts = new TransactionScope())
                            {
                                db.StockTransfer.Add(stocktransfer);
                                db.SaveChanges();
                              
                                if (pl1 != null)
                                {
                                    if ((pl1.avaliableSeats - stocktransfer.noOfSeats) == 0)
                                    {
                                        pl1.pnrStatus = "Sold";
                                        pl1.avaliableSeats = 0;
                                        pl1.sellSeats = pl1.sellSeats + stocktransfer.noOfSeats;

                                        pl1.pnrLock = "";
                                        db.Entry(pl1).State = EntityState.Modified;
                                        db.SaveChanges();
                                        

                                        var st = db.SeatConfirmation.Where(x => (x.pnrNumber == stocktransfer.pnrNumber ||
                                        x.newPnrNumber == stocktransfer.pnrNumber) && x.recevingBranch == stocktransfer.transferingBranch).SingleOrDefault();
                                        if (st != null)
                                        {
                                            st.pnrStatus1 = st.pnrStatus = "Sold";
                                            db.Entry(st).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                    }
                                }                                                            
                                rr.isSuccess = true;
                                rr.Message = "Insert Successfully";
                                errors.Add(rr);
                                ts.Complete();
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
        [HttpPost]
        [CheckSessionOut]
        public async Task<string> Transfercreate( StockTransfer stocktransfer)
        {
            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (String.IsNullOrEmpty(stocktransfer.pnrNumber))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "pnrNumber", ErrorMessage = "PNR # Cannot be null " });
                    }
                    if (String.IsNullOrEmpty(stocktransfer.airLine))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "airLine", ErrorMessage = "Airline Cannot be null " });
                    }
                    if (String.IsNullOrEmpty(stocktransfer.stockId))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "stockId", ErrorMessage = "StockId Cannot be null " });
                    }

                    if(stocktransfer.pnrNumber.Contains(','))
                    {
                        string[] splitPnr = stocktransfer.pnrNumber.Split(',');
                        stocktransfer.pnrNumber = splitPnr[0];
                    }
                    int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                    pnrLog pl1 = db.pnrLogs.Where(x => x.pnrNumber == stocktransfer.pnrNumber && x.branchName == stocktransfer.transferingBranch && x.idSubscription == idSubcription).SingleOrDefault();
                    if(pl1.avaliableSeats >= stocktransfer.noOfSeats)
                    {
                        if (errors.Count == 0)
                        {
                          

                            stocktransfer.createAt = DateTime.Now;
                            stocktransfer.id_Subscription = idSubcription;
                            stocktransfer.UpdateAt = DateTime.Now;
                            using (TransactionScope ts = new TransactionScope())
                            {

                                db.StockTransfer.Add(stocktransfer);
                                db.SaveChanges();

                                pnrLog pl = db.pnrLogs.Where(x => x.pnrNumber == stocktransfer.pnrNumber && x.branchName == stocktransfer.recevingBranch && x.idSubscription== idSubcription).SingleOrDefault();
                                if (pl != null)
                                {
                                   
                                }
                                else
                                {
                                    pl = new pnrLog();
                                    pl.pnrStatus = "Avaliable";
                                    pl.branchName = stocktransfer.recevingBranch;
                                    pl.pnrNumber = stocktransfer.pnrNumber;
                                    pl.idSubscription = stocktransfer.id_Subscription;
                                    pl.pnrLock = "";
                                    pl.avaliableSeats = pl.receiveSeats = stocktransfer.noOfSeats;
                                    db.pnrLogs.Add(pl);
                                    db.SaveChanges();

                                }
                               
                                if ((pl1.avaliableSeats - stocktransfer.noOfSeats) <= 0)
                                {
                                    pl1.pnrStatus = "Sold";
                                    pl1.avaliableSeats = 0;
                                    pl1.transferSeats = pl.transferSeats + stocktransfer.noOfSeats;
                                    pl1.pnrLock = "Locked";
                                    db.Entry(pl1).State = EntityState.Modified;
                                    db.SaveChanges();
                                    

                                    var st = db.SeatConfirmation.Where(x => (x.pnrNumber == stocktransfer.pnrNumber ||
                                    x.newPnrNumber == stocktransfer.pnrNumber) && x.recevingBranch == stocktransfer.transferingBranch).SingleOrDefault();
                                    if(st != null)
                                    {
                                       st.pnrStatus1 = st.pnrStatus = "Sold";
                                        db.Entry(st).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    pl1.pnrStatus = "Avaliable";
                                    pl1.avaliableSeats = pl.avaliableSeats- stocktransfer.noOfSeats;
                                    pl1.transferSeats = pl.transferSeats + stocktransfer.noOfSeats;
                                    pl1.pnrLock = "";
                                    db.Entry(pl1).State = EntityState.Modified;
                                    db.SaveChanges();
                                }



                                rr.isSuccess = true;
                                rr.Message = "Insert Successfully";
                                errors.Add(rr);
                                ts.Complete();
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


            return JsonConvert.SerializeObject(errors);
        }

        // GET: /Stock/Edit/5
        [CheckSessionOut]
        public async Task<ActionResult> sellingEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            StockTransfer stocktransfer =  db.StockTransfer.Where(x => x.id_StockTransfer == id && x.id_Subscription == idSubcription).FirstOrDefault();
            if (stocktransfer == null)
            {
                return HttpNotFound();
            }
            await Task.Factory.StartNew(() =>
            {


                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<airLine> airline = db1.Airline.Where(x=> x.id_Subscription == idSubcription).ToList();
                    db1.Dispose();
                    stocktransfer.ListAirline = new List<SelectListItem>();
                    airline.ForEach(x =>
                    {
                        stocktransfer.ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Agents> agents = db1.Agent.Include(x => x.Person).Where(x=> x.id_Subscription == idSubcription).ToList(); db1.Dispose();
                    stocktransfer.ListAgents = new List<SelectListItem>();
                    agents.ForEach(x =>
                    {
                        stocktransfer.ListAgents.Add(new SelectListItem { Text = x.Person.firstName + " " + x.Person.lastName + " (" + x.id_Agent + ")", Value = x.id_Agent.ToString() });
                    });

                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<branches> branches = db1.Branch.Where(x => x.id_Subscription == idSubcription).ToList();
                    db1.Dispose();
                    stocktransfer.ListBranches = new List<SelectListItem>();
                    branches.ForEach(x =>
                    {
                        stocktransfer.ListBranches.Add(new SelectListItem { Text = x.branchName, Value = x.branchName.ToString() });
                    });
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Country> Country = db.Country.OrderBy(x => x.Name).ToList(); db1.Dispose();
                    stocktransfer.ListCountry = new List<SelectListItem>();
                    Country.ForEach(x =>
                    {
                        stocktransfer.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Stock> Stocks = db1.Stock.Where(x => x.id_Subscription == idSubcription).OrderBy(x => x.id_Stock).ToList(); db1.Dispose();
                    stocktransfer.ListStockId = new List<SelectListItem>();
                    Stocks.ForEach(x =>
                    {
                        stocktransfer.ListStockId.Add(new SelectListItem { Text = x.stockName, Value = x.stockName.ToString() });
                    });
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<pnrLog> pnrAvaliable = new List<pnrLog>();
                    stocktransfer.ListPNR = new List<SelectListItem>();
                    if (string.IsNullOrEmpty(Session["branchName"].ToString()))
                    {
                        pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubcription && x.pnrStatus == "Avaliable").OrderBy(x => x.pnrLogId).ToList();
                        pnrAvaliable.ForEach(pr =>
                        {

                            stocktransfer.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber, Value = pr.pnrNumber });
                        });
                    }
                    else
                    {
                        string sb = Session["branchName"].ToString();
                        pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubcription && x.pnrStatus == "Avaliable" && x.branchName == sb).OrderBy(x => x.pnrLogId).ToList();
                        pnrAvaliable.ForEach(pr =>
                        {

                            stocktransfer.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber + "(" + pr.branchName + ")", Value = pr.pnrNumber + "," + pr.branchName });
                        });

                    }

                }));
                Task.WaitAll(tasks.ToArray());



            });
            return View(stocktransfer);
        }

        [HttpPost]
        [CheckSessionOut]
        public async Task<string> sellingEdit(StockTransfer stocktransfer)
        {

            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (String.IsNullOrEmpty(stocktransfer.pnrNumber))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "pnrNumber", ErrorMessage = "PNR # Cannot be null " });
                    }
                    if (String.IsNullOrEmpty(stocktransfer.airLine))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "airLine", ErrorMessage = "Airline Cannot be null " });
                    }
                    if (String.IsNullOrEmpty(stocktransfer.stockId))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "stockId", ErrorMessage = "StockId Cannot be null " });
                    }




                    if (errors.Count == 0)
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                        var sc = db.StockTransfer.Where(x => x.id_StockTransfer == stocktransfer.id_StockTransfer && x.id_Subscription == idSubcription).FirstOrDefault();
                        if (sc != null)
                        {
                           
                            sc.UpdateAt = DateTime.Now;
                            sc.country = stocktransfer.country;
                            sc.airLine = stocktransfer.airLine;
                            sc.stockId = stocktransfer.stockId;
                            sc.idAgent = stocktransfer.idAgent;
                            sc.sellingBranch = stocktransfer.sellingBranch;
                            sc.noOfSeats = stocktransfer.noOfSeats;
                            sc.cost = stocktransfer.cost;
                            sc.margin = stocktransfer.margin;
                            sc.sellingPrice = stocktransfer.sellingPrice;
                            sc.advanceAmount = stocktransfer.advanceAmount;
                            sc.advanceDate = stocktransfer.advanceDate;
                            sc.gdsPnrNumber = stocktransfer.gdsPnrNumber;
                            sc.catalystInvoiceNumber = stocktransfer.catalystInvoiceNumber;
                            sc.isTickted = stocktransfer.isTickted;
                            sc.isPackage = stocktransfer.isPackage;
                            db.Entry(sc).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            rr.isSuccess = true;
                            rr.Message = "Update Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.Message = "PNR # does not exists";
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
                foreach (var e in errors1)
                {
                    errors.Add(new ResponseRequest() { isSuccess = false, ErrorMessage = e });
                }
            }


            return JsonConvert.SerializeObject(errors); ;
        }

        [HttpPost]
        [CheckSessionOut]

        public async Task<string> transferEdit(StockTransfer stocktransfer)
        {

            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (String.IsNullOrEmpty(stocktransfer.pnrNumber))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "pnrNumber", ErrorMessage = "PNR # Cannot be null " });
                    }
                    if (String.IsNullOrEmpty(stocktransfer.airLine))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "airLine", ErrorMessage = "Airline Cannot be null " });
                    }
                    if (String.IsNullOrEmpty(stocktransfer.stockId))
                    {
                        errors.Add(new ResponseRequest() { isSuccess = false, Element = "stockId", ErrorMessage = "StockId Cannot be null " });
                    }




                    if (errors.Count == 0)
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                        var sc = db.StockTransfer.Where(x => x.id_StockTransfer== stocktransfer.id_StockTransfer && x.id_Subscription == idSubcription).FirstOrDefault();
                        if (sc != null)
                        {

                            sc.UpdateAt = DateTime.Now;
                            sc.country = stocktransfer.country;
                            sc.airLine = stocktransfer.airLine;
                            sc.stockId = stocktransfer.stockId;

                            sc.transferingBranch = stocktransfer.transferingBranch;
                            sc.recevingBranch = stocktransfer.recevingBranch;
                            sc.cost = stocktransfer.cost;
                            sc.margin = stocktransfer.margin;
                            sc.sellingPrice = stocktransfer.sellingPrice;
                          
                            db.Entry(sc).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            rr.isSuccess = true;
                            rr.Message = "Update Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.Message = "PNR # does not exists";
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
                foreach (var e in errors1)
                {
                    errors.Add(new ResponseRequest() { isSuccess = false, ErrorMessage = e });
                }
            }


            return JsonConvert.SerializeObject(errors); ;
        }
        [CheckSessionOut]

        public async Task<ActionResult> sellingdelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            StockTransfer stocktransfer = db.StockTransfer.Where(x => x.id_StockTransfer == id && x.id_Subscription == idSubcription).FirstOrDefault();
            if (stocktransfer == null)
            {
                return HttpNotFound();
            }
            return View(stocktransfer);
        }
        [CheckSessionOut]
        public async Task<ActionResult> transferdelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            StockTransfer stocktransfer = db.StockTransfer.Where(x => x.id_StockTransfer == id && x.id_Subscription == idSubcription).FirstOrDefault();
            
            if (stocktransfer == null)
            {
                return HttpNotFound();
            }
            return View(stocktransfer);
        }
        // POST: /Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [CheckSessionOut]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            StockTransfer stocktransfer =  db.StockTransfer.Where(x => x.id_StockTransfer == id && x.id_Subscription == idSubcription).FirstOrDefault();
         
            db.StockTransfer.Remove(stocktransfer);
            await db.SaveChangesAsync();
            await Task.Factory.StartNew(() =>
            {
                string sb = Session["branchName"].ToString();
                pnrCalculator.isPnrAvaliable(stocktransfer.pnrNumber,sb,"pAvaliable","pSold");
            });
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
    }
}
