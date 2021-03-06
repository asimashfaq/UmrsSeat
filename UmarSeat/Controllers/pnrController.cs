﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UmarSeat.Models;
using UmarSeat.Helpers;
using Newtonsoft.Json;

namespace UmarSeat.Controllers
{
    [Authorize]
    [AuthorizeRoles(Role.Administrator, Role.PNRReports)]
    public class pnrController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: pnr
        [CheckSessionOut]
        public ActionResult Index()
        {
            ApplicationDbContext db1 = new ApplicationDbContext();
            int idSubscription = Convert.ToInt32(Session["idSubscription"].ToString());
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
        public ActionResult tree(string pnr)
        {



            ApplicationDbContext db1 = new ApplicationDbContext();
            int idSubscription = Convert.ToInt32(Session["idSubscription"].ToString());
            pnrLog pl = new pnrLog();
            List<pnrLog> pnrAvaliable = new List<pnrLog>();
            pl.ListPNR = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Session["branchName"].ToString()))
            {
                pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubscription).OrderBy(x => x.pnrLogId).ToList();
                pnrAvaliable.ForEach(pr =>
                {


                    pl.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber + "(" + pr.branchName + ")", Value = pr.pnrNumber + "," + pr.branchName });
                });
            }
            else
            {
                string sb = Session["branchName"].ToString();
                pnrAvaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubscription && x.branchName == sb).OrderBy(x => x.pnrLogId).ToList();
                pnrAvaliable.ForEach(pr =>
                {

                    pl.ListPNR.Add(new SelectListItem { Text = pr.pnrNumber, Value = pr.pnrNumber });
                });

            }
            return View(pl);
        }

        public string treedata(string pnr)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            SeatConfirmation sc = db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber == null && x.id_Subscription == idSubcription).FirstOrDefault();
            if (sc == null)
            {

                do
                {
                    sc = db.SeatConfirmation.Where(x => x.newPnrNumber == pnr && x.id_Subscription == idSubcription).FirstOrDefault();
                    if (sc != null)
                    {
                        pnr = sc.pnrNumber;
                    }
                }
                while (sc != null);
                return JsonConvert.SerializeObject(generateTree2(pnr));
            }
            else
            {
                return JsonConvert.SerializeObject(generateTree2(pnr));
            }
            
        }

        private Dictionary<string, object> generateTree2(string pnr)
        {
            int idSubscription = Convert.ToInt32(Session["idSubscription"].ToString());

            Dictionary<string, object> tdata = new Dictionary<string, object>();

            SeatConfirmation sc = db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber == null && x.id_Subscription == idSubscription).FirstOrDefault();
            if (sc == null)
            {
                sc = db.SeatConfirmation.Where(x => x.newPnrNumber == pnr && x.id_Subscription == idSubscription).FirstOrDefault();
            }
            tdata.Add("name", pnr+ " ("+sc.noOfSeats+")");
            tdata.Add("branchName", sc.recevingBranch);
            

            List<Dictionary<string, object>> dd1 = new List<Dictionary<string, object>>();
            List<pnrLog> subbranchs = db.pnrLogs.Where(x => x.pnrNumber == pnr && x.idSubscription == idSubscription).ToList();
            subbranchs.ForEach(sbb =>{

                Dictionary<string, object> d = new Dictionary<string, object>();
                List<Dictionary<string, object>> dd3 = new List<Dictionary<string, object>>();
                List<Dictionary<string, object>> dd4 = new List<Dictionary<string, object>>();
                int total = 0;
                if(sbb.totalSeats != 0)
                {
                    total = sbb.totalSeats;
                }
                else
                {
                    total = sbb.receiveSeats;
                }
                 d.Add("name", sbb.branchName+ " ("+total+")");
                List<SeatConfirmation> children = db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber != null && x.recevingBranch== sbb.branchName && x.id_Subscription == idSubscription).ToList();
                children.ForEach(ch =>
                {
                    Dictionary<string, object> d1 = generateTree2(ch.newPnrNumber);

                    dd4.Add(d1);
                });
                List<Dictionary<string, object>> dd5 = new List<Dictionary<string, object>>();
                List<StockTransfer> transferbrances = db.StockTransfer.Where(x => x.pnrNumber == pnr && x.transferingBranch == sbb.branchName && x.id_Subscription == idSubscription).ToList();
                transferbrances.ForEach(tfb=> {
                    dd5.Add(new Dictionary<string, object>() { { "name",tfb.recevingBranch + " (" + (tfb.noOfSeats) + ")" } });
                });


                if(sbb.groupSplit>0)
                dd3.Add(new Dictionary<string, object>() { { "name", "Split" + " (" + sbb.groupSplit+")" }, { "children",dd4} });
                if (sbb.transferSeats > 0)
                    dd3.Add(new Dictionary<string, object>() { { "name", "Transfer" + " (" +  (sbb.transferSeats ) + ")" }, { "children", dd5 } });
                if (sbb.sellSeats > 0)
                    dd3.Add(new Dictionary<string, object>() { { "name", "Sale" + " (" + sbb.sellSeats + ")" } });
                if (sbb.receiveSeats > 0)
                    dd3.Add(new Dictionary<string, object>() { { "name", "Receive" + " (" + sbb.receiveSeats + ")" } });
                if (sbb.avaliableSeats > 0)
                    dd3.Add(new Dictionary<string, object>() { { "name", "Avaliable" + " (" + sbb.avaliableSeats + ")" } });
                d.Add("children", dd3);

                dd1.Add(d);
            });
            tdata.Add("children", dd1);

            /*
       

            if (children.Count > 0)
            {
                List<Dictionary<string, object>> dd = new List<Dictionary<string, object>>();
                children.ForEach(x =>
                {


                    Dictionary<string, object> d = generateTree(x.newPnrNumber);

                    dd.Add(d);


                });
                tdata.Add("children", dd);
            }
            */


            return tdata;


        }

        private Dictionary<string, object> generateTree(string pnr)
        {
            int idSubscription = Convert.ToInt32(Session["idSubscription"].ToString());

            Dictionary<string, object> tdata = new Dictionary<string, object>();

            SeatConfirmation sc = db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber == null && x.id_Subscription == idSubscription).FirstOrDefault();
            if(sc == null)
            {
                sc = db.SeatConfirmation.Where(x => x.newPnrNumber == pnr && x.id_Subscription == idSubscription).FirstOrDefault();
            }
            tdata.Add("name", pnr + " (" + sc.noOfSeats + ")");
            tdata.Add("branchName", sc.recevingBranch);
            


            List<SeatConfirmation> children = db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber != null && x.id_Subscription == idSubscription).ToList();
            
            if(children.Count>0)
            {
                List<Dictionary<string, object>> dd = new List<Dictionary<string, object>>();
                children.ForEach(x =>
                {


                    Dictionary<string, object> d = generateTree(x.newPnrNumber );

                    dd.Add(d);


                });
                tdata.Add("children", dd);
            }
          

            
            return tdata;


        }

        [CheckSessionOut]
        public ActionResult log()
        {
            List<pnrLog> pnrList2 = new List<pnrLog>();
            List<Task> task = new List<Task>();
            int idSubscription = Convert.ToInt32(Session["idSubscription"].ToString());
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

                if (string.IsNullOrEmpty(br))
                {
                    var seatsavaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubscription && x.pnrStatus == "Expired").Count();
                    ViewBag.totalexpired = seatsavaliable;
                }
                else
                {
                    var seatsavaliable = db1.pnrLogs.Where(x => x.idSubscription == idSubscription && x.pnrStatus == "Expired" && x.branchName == br).Count();
                    ViewBag.totalexpired = seatsavaliable;
                }

            }));


            task.Add(Task.Factory.StartNew(() => {
                ApplicationDbContext db1 = new ApplicationDbContext();
                List<pnrLog> pnrList = new List<pnrLog>();
                if (string.IsNullOrEmpty(br))
                {
                    pnrList = db1.pnrLogs.Where(x => x.createdAt.Year == DateTime.Now.Year && x.idSubscription == idSubscription).OrderByDescending(x => x.pnrLogId).Take(10).ToList();
                }
                else
                {
                    pnrList = db1.pnrLogs.Where(x => x.createdAt.Year == DateTime.Now.Year && x.branchName == br && x.idSubscription == idSubscription).OrderByDescending(x => x.pnrLogId).Take(10).ToList();
                }
                pnrList.ForEach(x => {
                    db = new ApplicationDbContext();
                    x.sc = db.SeatConfirmation.Where(sc => sc.pnrNumber == x.pnrNumber && sc.newPnrNumber == null && sc.recevingBranch == x.branchName && x.createdAt.Month == DateTime.Now.Month && sc.id_Subscription == idSubscription).FirstOrDefault();
                    if (x.sc == null)
                    {
                        x.sc = db.SeatConfirmation.Where(sc => sc.newPnrNumber == x.pnrNumber && sc.recevingBranch == x.branchName && x.createdAt.Month == DateTime.Now.Month && sc.id_Subscription == idSubscription).FirstOrDefault();
                        if (x.sc == null)
                        {
                            x.sc = db.SeatConfirmation.Where(sc => sc.newPnrNumber == x.pnrNumber && sc.id_Subscription == idSubscription).FirstOrDefault();
                            if (x.sc == null)
                            {


                                StockTransfer skt = db.StockTransfer.Where(sx => sx.pnrNumber == x.pnrNumber && sx.recevingBranch == x.branchName && sx.id_Subscription == idSubscription).FirstOrDefault();
                                if (skt != null)
                                {
                                    x.sc = db.SeatConfirmation.Where(scx => (scx.pnrNumber == x.pnrNumber || scx.newPnrNumber == x.pnrNumber) && (scx.recevingBranch == skt.transferingBranch || scx.recevingBranch == skt.recevingBranch) && scx.id_Subscription == idSubscription).FirstOrDefault();
                                    x.sc.noOfSeats = skt.noOfSeats;
                                    x.sc.cost = skt.sellingPrice;
                                    x.sc.recevingBranch = x.branchName;
                                    pnrList2.Add(x);

                                }
                                else
                                {

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
             List<pnrLog> pnrList2 = new List<pnrLog>();
            if (string.IsNullOrEmpty(bn))
            {
                model = db.pnrLogs.Where(x => x.idSubscription == idSubcription).OrderByDescending(x => x.pnrLogId).ToList();
                model = model.Skip(pageSize * (numPage - 1)).Take(pageSize).ToList();
            }
            else
            {
                model =  db.pnrLogs.Where(x => x.idSubscription == idSubcription ).OrderByDescending(x => x.pnrLogId).ToList();
                model = model.Skip(pageSize * (numPage - 1)).Take(pageSize).ToList();
            }

            model.ForEach(x => {
                db = new ApplicationDbContext();
                x.sc = db.SeatConfirmation.Where(sc => sc.pnrNumber == x.pnrNumber && sc.newPnrNumber == null && sc.recevingBranch == x.branchName && x.createdAt.Month == DateTime.Now.Month && sc.id_Subscription == idSubcription).FirstOrDefault();
                if (x.sc == null)
                {
                    x.sc = db.SeatConfirmation.Where(sc => sc.newPnrNumber == x.pnrNumber && sc.recevingBranch == x.branchName && x.createdAt.Month == DateTime.Now.Month && sc.id_Subscription == idSubcription).FirstOrDefault();
                    if (x.sc == null)
                    {
                        x.sc = db.SeatConfirmation.Where(sc => sc.newPnrNumber == x.pnrNumber && sc.id_Subscription == idSubcription).FirstOrDefault();
                        if (x.sc == null)
                        {


                            StockTransfer skt = db.StockTransfer.Where(sx => sx.pnrNumber == x.pnrNumber && sx.recevingBranch == x.branchName && sx.id_Subscription == idSubcription).FirstOrDefault();
                            if (skt != null)
                            {
                                x.sc = db.SeatConfirmation.Where(scx => (scx.pnrNumber == x.pnrNumber || scx.newPnrNumber == x.pnrNumber) && (scx.recevingBranch == skt.transferingBranch || scx.recevingBranch == skt.recevingBranch) && scx.id_Subscription == idSubcription).FirstOrDefault();
                                x.sc.noOfSeats = skt.noOfSeats;
                                x.sc.cost = skt.sellingPrice;
                                x.sc.recevingBranch = x.branchName;
                                pnrList2.Add(x);

                            }
                            else
                            {

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
            decimal count = 0;
            if (string.IsNullOrEmpty(bn))
            {
                count = db.pnrLogs.Where(x => x.createdAt.Year == DateTime.Now.Year && x.idSubscription == idSubcription).ToList().Count;
            }
            else
            {
                count = db.pnrLogs.Where(x => x.createdAt.Year == DateTime.Now.Year && x.branchName == bn && x.idSubscription == idSubcription).ToList().Count;
            }
           
            decimal pages = count / 10;
            ViewBag.pages = (int)Math.Ceiling(pages); 
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
            return PartialView("_loglist", pnrList2);
        }

    }
}