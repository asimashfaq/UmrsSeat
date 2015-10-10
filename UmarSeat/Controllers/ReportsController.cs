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
using UmarSeat.Helpers;
using Newtonsoft.Json;

namespace UmarSeat.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Reports/
        public async Task<ActionResult> SeatConfirmation()
        {
            return View(await db.SeatConfirmation.ToListAsync());
        }
        public async Task<ActionResult> pnr()
        {
            return View(await db.SeatConfirmation.ToListAsync());
        }
        public async Task<string> airline()
        {
            string arilienNmae = "G9";
            int id_Subscription = 1005;
            DateTime startdate = new DateTime(2016,1,1);
            DateTime enddate = new DateTime(2016, 1, 31);
            Dictionary<string, object> totaldata = new Dictionary<string, object>();
            var sectors = db.Sector.Where(x => x.airline == arilienNmae && x.category == "Both" || x.category == "Outbound Sector").GroupBy(s=> s.sectorName).ToList();
            
            sectors.ToList().ForEach(x =>
            {
               
               
                string query = @"SELECT  sum(noOfSeats) As TotalSeats ,airLine,outBoundSector, outBoundDate,id_Subscription  FROM [SeatConfirmations] where newPnrNumber is null  and outBoundDate >= '" + startdate.ToString("yyyy-MM-dd") + "' and   outBoundDate <='" + enddate.ToString("yyyy-MM-dd") + "' and id_Subscription = " + id_Subscription + " and airLine = '" + arilienNmae + "' and outBoundSector =  '" + x.Key + "'  group by airLine, outBoundSector, outBoundDate, id_Subscription order by outBoundDate";
                List<TotalSeatsAirlineSector> tsal = db.Database.SqlQuery<TotalSeatsAirlineSector>(query).ToList<TotalSeatsAirlineSector>();
                totaldata.Add(x.Key, tsal);
            });


            return JsonConvert.SerializeObject(totaldata);
        }
        public async Task<string> airlinesold( string startDate, string enddate, string airlineName)
        {
            
            int id_Subscription = Convert.ToInt32(Session["idSubscription"].ToString());


            Dictionary<string, object> totaldata = new Dictionary<string, object>();
            var sectors = db.Sector.Where(x => x.airline == airlineName).GroupBy(s => s.sectorName).ToList();

            sectors.ToList().ForEach(x =>
            {


                string query = @"SELECT  sum(noOfSeats) As TotalSeats ,airLine,outBoundSector, outBoundDate,id_Subscription ,pnrNumber, recevingBranch FROM [SeatConfirmations] where newPnrNumber is null  and outBoundDate between '" + startDate + "' and  '" + enddate + "' and id_Subscription = " + id_Subscription + " and airLine = '" + airlineName + "' and outBoundSector =  '" + x.Key + "'  group by airLine, outBoundSector, outBoundDate,pnrNumber, recevingBranch, id_Subscription order by outBoundDate";
                List<SaleSeatsAirlineSector> tsal = db.Database.SqlQuery<SaleSeatsAirlineSector>(query).ToList<SaleSeatsAirlineSector>();
                List<object> tsal1 = new List<object>();
                tsal.ForEach(child =>
                {
                    
                    generateTree4(child.pnrNumber, child.id_Subscription);
                    
                    child.data = pnrs;

                    tsal1.Add(child);
                    
                    pnrs = new List<Dictionary<string, object>>();
                  
                   
                    
                });
                if(tsal1.Count>0)
                totaldata.Add(x.Key, tsal1);
            });


            return JsonConvert.SerializeObject(totaldata);
        }
        public async Task<string> seatsell(string startDate, string enddate, string airlineName)
        {

            int id_Subscription =1002;


            List<object> totaldata = new List<object>();
            DateTime sdata = Convert.ToDateTime(startDate);
            DateTime endDate = Convert.ToDateTime(enddate);
            var sellRecord = db.StockTransfer.Where(x => x.createAt >= sdata && x.createAt<= endDate  && x.airLine == airlineName && x.sellingBranch != null)
                .Select(x=> new { x.pnrNumber,x.sellingBranch,x.noOfSeats,x.sellingPrice,x.idAgent,x.cost,x.margin}).ToList();
            sellRecord.ForEach(x =>
            {
                
                 var sobject = x.ToDictionary();
                var sc = db.SeatConfirmation.Where(s => (s.pnrNumber == x.pnrNumber || s.newPnrNumber == x.pnrNumber) && s.recevingBranch == x.sellingBranch)
                         .Select(_st => new { _st.pnrNumber, _st.newPnrNumber, _st.outBoundSector, _st.inBoundSector, _st.cost }).FirstOrDefault();
                if(sc == null)
                {
                    sc = db.SeatConfirmation.Where(s => (s.pnrNumber == x.pnrNumber || s.newPnrNumber == x.pnrNumber))
                         .Select(_st => new { _st.pnrNumber, _st.newPnrNumber, _st.outBoundSector, _st.inBoundSector, _st.cost }).FirstOrDefault();
                }
                sobject.Add(new KeyValuePair<string, object>("sectorInfo",sc));


                SeatConfirmation psc;
                string pnr = x.pnrNumber;
                do
                {
                    psc = db.SeatConfirmation.Where(px => px.newPnrNumber == pnr && px.id_Subscription == id_Subscription).FirstOrDefault();
                    if (psc != null)
                    {
                        pnr = psc.pnrNumber;
                    }
                }
                while (sc != null);
                sobject.Add(new KeyValuePair<string, object>("BasePnr", pnr));
                var agentInfo = db.Agent.Include(ag => ag.Person).Where(iag => iag.id_Agent == Convert.ToInt32(x.idAgent)).SingleOrDefault();
                sobject.Add(new KeyValuePair<string, object>("agentinfo", agentInfo));
                totaldata.Add(sobject);
            });

            return JsonConvert.SerializeObject(totaldata);
        }
        public async Task<ActionResult> stockId()
        {
            return View(await db.SeatConfirmation.ToListAsync());
        }
        public async Task<ActionResult> outbounddate()
        {
            ApplicationDbContext db1 = new ApplicationDbContext();
            int id_Subscription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<airLine> airline = db1.Airline.Where(x => x.id_Subscription == id_Subscription).ToList();
            db1.Dispose();
            var ListAirline = new List<SelectListItem>();
            airline.ForEach(x =>
            {
               ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
            });

            ViewBag.listAirline = ListAirline;
            ViewBag.selectedValue = ListAirline[0].Value;
            return View();
        }
        public ActionResult stockselling()
        {
            ApplicationDbContext db1 = new ApplicationDbContext();
            int id_Subscription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<airLine> airline = db1.Airline.Where(x => x.id_Subscription == id_Subscription).ToList();
            db1.Dispose();
            var ListAirline = new List<SelectListItem>();
            airline.ForEach(x =>
            {
                ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
            });

            ViewBag.listAirline = ListAirline;
            ViewBag.selectedValue = ListAirline[0].Value;
            return View();
        }

        public ActionResult seatselling()
        {
            ApplicationDbContext db1 = new ApplicationDbContext();
            int id_Subscription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<airLine> airline = db1.Airline.Where(x => x.id_Subscription == id_Subscription).ToList();
            db1.Dispose();
            var ListAirline = new List<SelectListItem>();
            airline.ForEach(x =>
            {
                ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
            });

            ViewBag.listAirline = ListAirline;
            ViewBag.selectedValue = ListAirline[0].Value;
            return View();
        }




        public async Task<ActionResult> outboundsector()
        {
            return View(await db.SeatConfirmation.ToListAsync());
        }
        public async Task<ActionResult> category()
        {
            return View(await db.SeatConfirmation.ToListAsync());
        }
        public async Task<ActionResult> recevingBranch()
        {
            return View(await db.SeatConfirmation.ToListAsync());

        }
        public async Task<ActionResult> duetimelimit()
        {
            return View(await db.SeatConfirmation.ToListAsync());
        }
        public ActionResult pnrGraph()
        {
            List<SeatConfirmation> sc = db.SeatConfirmation.ToList();
            DateTime startdate = sc.First().CreatedAt;
            DateTime endDate = sc.Last().CreatedAt;
            Dictionary<int, object> interval = new Dictionary<int, object>();
            
            Dictionary<string, object> io = new Dictionary<string, object>();
            int dfference = (endDate - startdate).Days;
            List<string> aa = new List<string>();
            aa.Add(UnixTimeStampUTC(startdate).ToString());
            interval.Add(UnixTimeStampUTC(startdate), "0");
            while(dfference>0)
            {
                startdate = startdate.AddDays(1);
                aa.Add(UnixTimeStampUTC(startdate).ToString());
               interval.Add(UnixTimeStampUTC(startdate), "0");
                dfference--;
            }
            foreach (SeatConfirmation s in sc)
            {
                interval[UnixTimeStampUTC(s.CreatedAt)] = s;
            }
            List<double[]> list1 = new List<double[]>();
            List<double[]> list2 = new List<double[]>();
            foreach(KeyValuePair<int,object> kv in interval)
            {
                try
                {
                    SeatConfirmation sco = (SeatConfirmation)kv.Value;
                    double[] ss = new double[2];
                    double[] ss1 = new double[2];
                    double time = Convert.ToInt64(kv.Key.ToString() + "000");
                ss1[0]= ss[0] =time ;
                ss[1] = Convert.ToDouble(sco.cost);
                list1.Add(ss);
                ss1[1] = Convert.ToDouble(sco.noOfSeats);
                list2.Add(ss1);
                }
                catch (Exception)
                {
                    double[] ss = new double[2];
                    double[] ss1 = new double[2];

                    double time = Convert.ToInt64(kv.Key.ToString() + "000");
                    ss1[0] = ss[0] = time;
                    ss[1] = 0;
                    list1.Add(ss);
                    ss1[1] = 0;
                    list2.Add(ss1);
            
                }
               
            }
            io.Add("cost", list1);

            io.Add("noOfSeats", list2);
            List<Dictionary<string, object>> op = new List<Dictionary<string, object>>();
            foreach(var i in io)
            {
                Dictionary<string, object> ob = new Dictionary<string, object>();
                ob.Add("key", i.Key);
                ob.Add("values", i.Value);
                ob.Add("disabled", "true");
                op.Add(ob);
            }
            op.First().Remove("disabled");
            return Json(op, JsonRequestBehavior.AllowGet);
        }
        public int UnixTimeStampUTC(DateTime date)
        {
            return Convert.ToInt32((date - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
        }
        List<Dictionary<string,object>> pnrs = new List<Dictionary<string, object>>();

        private void generateTree4(string pnr, int idSubscription)
        {
            Dictionary<string, object> salesinfo = new Dictionary<string, object>();
            salesinfo.Add("pnr", pnr);
            
            List<SeatConfirmation> sc = new List<Models.SeatConfirmation>();
            do
            {
                sc = db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber != null && x.id_Subscription == idSubscription).OrderByDescending(x=> x.id_SeatConfirmation).ToList();
                if (sc != null)
                {
                    sc.ForEach(x=> {
                        
                        generateTree4(x.newPnrNumber, x.id_Subscription);
                       

                    });
                  
                }
                
                    break;
                
            }
            while (sc != null);
            var stlist = db.StockTransfer.Where(x => x.pnrNumber == pnr && x.id_Subscription == idSubscription && x.sellingBranch != null)
                .Select(x=> new {x.id_StockTransfer, x.pnrNumber,x.noOfSeats,x.createAt,x.id_Subscription}).OrderBy(x=> x.id_StockTransfer).ToList();
          
            salesinfo.Add("sale", stlist);
            if(stlist.Count >0)
            pnrs.Add(salesinfo);
          
        }

        private Dictionary<string, object> generateTree2(string pnr,int idSubscription)
        {
            

            Dictionary<string, object> tdata = new Dictionary<string, object>();

            SeatConfirmation sc = db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber == null && x.id_Subscription == idSubscription).FirstOrDefault();
            if (sc == null)
            {
                sc = db.SeatConfirmation.Where(x => x.newPnrNumber == pnr && x.id_Subscription == idSubscription).FirstOrDefault();
            }
            tdata.Add("name", pnr);
            tdata.Add("branchName", sc.recevingBranch);


            List<Dictionary<string, object>> dd1 = new List<Dictionary<string, object>>();
            List<pnrLog> subbranchs = db.pnrLogs.Where(x => x.pnrNumber == pnr && x.idSubscription == idSubscription).ToList();
            subbranchs.ForEach(sbb => {

                Dictionary<string, object> d = new Dictionary<string, object>();
                List<Dictionary<string, object>> dd3 = new List<Dictionary<string, object>>();
                List<Dictionary<string, object>> dd4 = new List<Dictionary<string, object>>();
                int total = 0;
                if (sbb.totalSeats != 0)
                {
                    total = sbb.totalSeats;
                }
                else
                {
                    total = sbb.receiveSeats;
                }
                d.Add("name", sbb.branchName);
                List<string> pnrs = new List<string>();
                do
                {
                    sc = db.SeatConfirmation.Where(x => x.newPnrNumber == pnr && x.id_Subscription == idSubscription).FirstOrDefault();
                    if (sc != null)
                    {
                        pnr = sc.pnrNumber;
                    }
                }
                while (sc != null);


                List<SeatConfirmation> children = db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber != null && x.recevingBranch == sbb.branchName && x.id_Subscription == idSubscription).ToList();
                children.ForEach(ch =>
                {
                    Dictionary<string, object> d1 = generateTree2(ch.newPnrNumber,idSubscription);
                   
                    dd4.Add(d1);
                });


                


                if (sbb.groupSplit > 0)
                    dd3.Add(new Dictionary<string, object>() { { "Split",  sbb.groupSplit  }, { "children", dd4 } });
                if (sbb.transferSeats > 0)
              //      dd3.Add(new Dictionary<string, object>() { { "Transfer",  (sbb.transferSeats)  }, { "children", dd5 } });
                if (sbb.sellSeats > 0)
                    dd3.Add(new Dictionary<string, object>() { { "Sale",  sbb.sellSeats } });
                d.Add("children", dd3);

                dd1.Add(d);
            });
            tdata.Add("children", dd1);

            


            return tdata;


        }
    }
}