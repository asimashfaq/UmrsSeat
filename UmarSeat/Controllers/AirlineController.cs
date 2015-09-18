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

namespace UmarSeat.Controllers
{
    [Authorize]
    public class AirlineController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Airline/
        [CheckSessionOut]
        public async Task<ActionResult> Index()
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var airline = db.Airline.Where(x=> x.id_Subscription == idSubcription);
            var list = await airline.ToListAsync();
            decimal count = Convert.ToDecimal(list.Count.ToString());
            list = list.OrderByDescending(x => x.id_AirLine).Skip(5 * (1 - 1)).Take(5).ToList();
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
            ViewBag.end = end;
            ViewBag.prev = 1;
            ViewBag.next = 2;
            ViewBag.current = 1;
            ViewBag.length = 5;
            return View(list);
        }
        [HttpGet]
        [CheckSessionOut]
        public async Task<ActionResult> getairlines(string length, string pageNum)
        {
            var pageSize = int.Parse(length);
            var numPage = int.Parse(pageNum);
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var airline = db.Airline.Where(x => x.id_Subscription == idSubcription);
            var list = await airline.ToListAsync();
            decimal count = Convert.ToDecimal(list.Count.ToString());
            list = list.OrderByDescending(x => x.id_AirLine).Skip(pageSize * (numPage - 1)).Take(pageSize).ToList();


            decimal pages = count / 5;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
            ViewBag.start = pageSize * (numPage - 1)+1;
            int end = pageSize * (numPage - 1)  + pageSize; 
            if(end >= count)
            {
                end = (int)Math.Ceiling(count); 
            }
            else
            {
              
            }
            ViewBag.end =end;
            ViewBag.prev = numPage - 1;
            ViewBag.next = numPage + 1;
            ViewBag.length = length;
            ViewBag.current = numPage;
            return PartialView("_airlinelist", list);
        }
        [HttpPost]
        [CheckSessionOut]
        public ActionResult advanceSearch(airLine airline)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<airLine> ss = filterdata(airline, db.Airline.Where(x=> x.id_Subscription== idSubcription).ToList());
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

            return PartialView("_airlinelist", ss);
        }
        [CheckSessionOut]
        private List<airLine> filterdata(airLine airline, List<airLine> ss)
        {


            if (!string.IsNullOrEmpty(airline.airlineName))
            {
                ss = ss.Where(x => x.airlineName.Contains(airline.airlineName)).ToList();
            }
            if (!string.IsNullOrEmpty(airline.Country))
            {
                ss = ss.Where(x => x.Country.Contains(airline.Country)).ToList();
            }


            return ss;
        }

        [CheckSessionOut]
        public async Task<ActionResult> Create()
        {
            airLine ar = new airLine();
            await Task.Factory.StartNew(() =>
            {

                List<Country> Country = db.Country.OrderBy(x => x.Name).ToList();
                ar.ListCountry = new List<SelectListItem>();
                Country.ForEach(x =>
                {
                    ar.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Name.ToString() });
                });

              
            });
            return View(ar);
        }

        // POST: /Airline/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CheckSessionOut]
        public async Task<string> Create( airLine airline)
        {
           
            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {




                    if (airline.airlineName != null)
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                        var air = db.Airline.Where(x => x.airlineName == airline.airlineName && x.id_Subscription== idSubcription).FirstOrDefault();
                        if (air == null)
                        {
                            airline.id_Subscription = idSubcription;

                            airline.createdAt = DateTime.Now;
                            db.Airline.Add(airline);
                            await db.SaveChangesAsync();
                            rr.isSuccess = true;
                            rr.Message = "Insert Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.Message = "Airline already exists";
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

        // GET: /Airline/Edit/5
        [CheckSessionOut]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            airLine airline = await db.Airline.Where(x=> x.id_Subscription== idSubcription && x.id_AirLine == id).FirstOrDefaultAsync();
            if (airline == null)
            {
                return HttpNotFound();
            }
            await Task.Factory.StartNew(() =>
            {

                List<Country> Country = db.Country.OrderBy(x => x.Name).ToList();
                airline.ListCountry = new List<SelectListItem>();
                Country.ForEach(x =>
                {
                    airline.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Name.ToString() });
                });


            });
            return View(airline);
        }

        // POST: /Airline/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CheckSessionOut]
        public async Task<string> Edit(airLine airline)
        {
           

            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {




                    if (airline.id_AirLine > 0 )
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                        var air = db.Airline.Where(x => x.id_AirLine == airline.id_AirLine && idSubcription== x.id_Subscription).FirstOrDefault();
                        if (air != null)
                        {


                            air.airlineName = airline.airlineName;
                            air.Country = airline.Country;

                            db.Entry(air).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            rr.isSuccess = true;
                            rr.Message = "Update Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.Message = "Airline not found";
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

        // GET: /Airline/Delete/5
        [CheckSessionOut]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            airLine airline = await db.Airline.Where(x => x.id_Subscription == idSubcription && x.id_AirLine == id).FirstOrDefaultAsync();
            if (airline == null)
            {
                return HttpNotFound();
            }
            return View(airline);
        }

        // POST: /Airline/Delete/5
        [HttpPost, ActionName("Delete")]
        [CheckSessionOut]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            airLine airline = await db.Airline.Where(x => x.id_Subscription == idSubcription && x.id_AirLine == id).FirstOrDefaultAsync();
            db.Airline.Remove(airline);
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
    }
}
