using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UmarSeat.Helpers;
using UmarSeat.Models;

namespace UmarSeat.Controllers
{
    public class StockIdController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: /StockId/
        [CheckSessionOut]
        public ActionResult Index()
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var stocks = db.Stock.Where(x => x.id_Subscription == idSubcription);
            var list =  stocks.ToList();
            decimal count = Convert.ToDecimal(list.Count.ToString());
            list = list.OrderByDescending(x => x.id_Stock).Skip(5 * (1 - 1)).Take(5).ToList();
            decimal pages = count / 5;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
            int start = 0;
            if (count > 0)
            {
                start = 1;
            }
            ViewBag.start = start;
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
            return View(list);
        }

        
              [HttpGet]
        [CheckSessionOut]
        public ActionResult getstocks(string length, string pageNum)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var pageSize = int.Parse(length);
            var numPage = int.Parse(pageNum);
            var stockId = db.Stock.Where(x => x.id_Subscription == idSubcription);
            var list =  stockId.ToList();
            decimal count = Convert.ToDecimal(list.Count.ToString());
            list =  list.OrderByDescending(x => x.id_Stock).Skip(pageSize * (numPage - 1)).Take(pageSize).ToList();
            
          
            decimal pages = count / 5;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
          
            int start = 0;
             if(count>0)
             {
                 start = pageSize * (numPage - 1)+1;
             }
                    ViewBag.start =start ;
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
            return PartialView("_stocksIdlist", list);
        }
        [HttpPost]
        [CheckSessionOut]
        public ActionResult advanceSearch(Stock st)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<Stock> ss = filterdata(st, db.Stock.Where(x => x.id_Subscription == idSubcription).ToList());
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

            return PartialView("_stocksIdlist", ss);
        }

        private List<Stock> filterdata(Stock st, List<Stock> ss)
        {

           
            if (!string.IsNullOrEmpty(st.stockName))
            {
                ss = ss.Where(x => x.stockName.Contains(st.stockName)).ToList();
            }
         
            
            return ss;
        }

        [CheckSessionOut]
        public ActionResult Create()
        {
            Stock st = new Stock();
            return View(st);
        }

        //
        // POST: /StockId/Create
        [HttpPost]
        [CheckSessionOut]
        public String Create(Stock stockId)
        {

            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {




                    if (stockId.stockName != null)
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                        var sc = db.Stock.Where(x => x.stockName.ToLower() == stockId.stockName.ToLower() && idSubcription == x.id_Subscription).FirstOrDefault();
                        if (sc == null)
                        {

                            stockId.id_Subscription = idSubcription;
                            db.Stock.Add(stockId);
                            db.SaveChanges();
                            rr.isSuccess = true;
                            rr.Message = "Insert Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.Message = "StockId already exists";
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

        //
        // GET: /StockId/Edit/5    [CheckSessionOut]
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Stock stockId = db.Stock.Where(x => x.id_Stock == id && x.id_Subscription == idSubcription).SingleOrDefault();
            if (stockId == null)
            {
                return HttpNotFound();
            }
            return View(stockId);

        }

        //
       
        [HttpPost]
        [CheckSessionOut]
        public string Edit(Stock stockId)
        {
            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {




                    if (stockId.stockName != null)
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                        var sc = db.Stock.Where(x => x.id_Stock== stockId.id_Stock && idSubcription == x.id_Subscription).FirstOrDefault();
                        if (sc != null)
                        {

                            sc.stockName = stockId.stockName;
                            db.Entry(stockId).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            rr.isSuccess = true;
                            rr.Message = "Update Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.Message = "StockId doesn't exists";
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

        //
        // GET: /StockId/Delete/5
        [CheckSessionOut]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Stock stockId = db.Stock.Where(x => x.id_Stock == id && x.id_Subscription == idSubcription).SingleOrDefault();
            if (stockId == null)
            {
                return HttpNotFound();
            }
            return View(stockId);
        }

        //
        // POST: /StockId/Delete/5
        [CheckSessionOut]
        [HttpPost][ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Stock stockId = db.Stock.Where(x => x.id_Stock == id && x.id_Subscription == idSubcription).SingleOrDefault();

            db.Stock.Remove(stockId);
             db.SaveChanges();
            return null;
        }
    }
}
