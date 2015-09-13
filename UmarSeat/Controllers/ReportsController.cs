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
        public async Task<ActionResult> airline()
        {
            return View(await db.SeatConfirmation.ToListAsync());
        }
        public async Task<ActionResult> stockId()
        {
            return View(await db.SeatConfirmation.ToListAsync());
        }
        public async Task<ActionResult> outbounddate()
        {
            return View(await db.SeatConfirmation.ToListAsync());
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
	}
}