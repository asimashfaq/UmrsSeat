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
    [AuthorizeRoles(Role.Administrator, Role.Managebranches)]
    public class BranchesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Branches/
        
        [CheckSessionOut]
        
        public async Task<ActionResult> Index()
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var branches = db.Branch;
            var list = await branches.Where(x => x.id_Subscription == idSubcription).ToListAsync();
            decimal count = Convert.ToDecimal(list.Count.ToString());
            list = list.OrderByDescending(x => x.id_branch).Skip(5 * (1 - 1)).Take(5).ToList();
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

        // GET: /Branches/Details/5
        
        [HttpGet]
        [CheckSessionOut]
        public async Task<ActionResult> getbranches(string length, string pageNum)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var pageSize = int.Parse(length);
            var numPage = int.Parse(pageNum);
            var branches = db.Branch;
            var list = await branches.Where(x => x.id_Subscription == idSubcription).ToListAsync();
            decimal count = Convert.ToDecimal(list.Count.ToString());
            list = list.OrderByDescending(x => x.id_branch).Skip(pageSize * (numPage - 1)).Take(pageSize).ToList();


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
            return PartialView("_branchlist", list);
        }
       
        [HttpPost]
        [CheckSessionOut]
        public ActionResult advanceSearch(branches branch)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<branches> ss = filterdata(branch, db.Branch.Where(x => x.id_Subscription == idSubcription).ToList());
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

            return PartialView("_branchlist", ss);
        }

        private List<branches> filterdata(branches branch, List<branches> ss)
        {


            if (!string.IsNullOrEmpty(branch.branchName))
            {
                ss = ss.Where(x => x.branchName.Contains(branch.branchName)).ToList();
            }
            if (!string.IsNullOrEmpty(branch.branchCountry))
            {
                ss = ss.Where(x => x.branchCountry.Contains(branch.branchCountry)).ToList();
            }


            return ss;
        }

        // GET: /Branches/Create
     
        [CheckSessionOut]
        public async Task<ActionResult> Create()
        {
            branches br = new branches();
            await Task.Factory.StartNew(() =>
            {

                List<Country> Country = db.Country.OrderBy(x => x.Name).ToList();
                br.ListCountry = new List<SelectListItem>();
                Country.ForEach(x =>
                {
                    br.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                });
            });

         
            return View(br);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetCities(string countryId)
        {

            List<SelectListItem> CityName = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(countryId))
            {
               
                List<City> cities = db.City.Where(x=> x.country == countryId).OrderBy(x=> x.city).ToList();
                cities.ForEach(x =>
                {
                    CityName.Add(new SelectListItem { Text = x.city, Value = x.city.Trim().ToString() });
                });
            }
            return Json(CityName, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetCountries(string searchTerm, int pageSize, int pageNum)
        {
            //Get the paged results and the total count of the results for this query. 

            List<Country> Countries = db.Country.OrderBy(x => x.Name).ToList();
            int countriesCount = Countries.Count;

            //Translate the attendees into a format the select2 dropdown expects
            Select2PagedResult Country = AttendeesToSelect2Format(Countries, countriesCount);

            //Return the data as a jsonp result
            return new JsonpResult
            {
                Data = Country,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        // POST: /Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [CheckSessionOut]

        public async Task<string> Create( branches branches)
        {
           

            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {


                    int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());

                    if (branches.branchName != null)
                    {
                        var bra = db.Branch.Where(x => x.branchName == branches.branchName && x.id_Subscription == idSubcription).FirstOrDefault();
                        if (bra == null)
                        {
                            branches.id_Subscription = idSubcription;
                           
                            branches.CreatedAt = DateTime.Now;
                            db.Branch.Add(branches);
                            await db.SaveChangesAsync();
                            rr.isSuccess = true;
                            rr.Message = "Insert Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.Message = "Branch already exists";
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

        // GET: /Branches/Edit/5
      
        [CheckSessionOut]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            branches branches = db.Branch.Where(x => x.id_Subscription == idSubcription && x.id_branch == id).FirstOrDefault();
            if (branches == null)
            {
                return HttpNotFound();
            }

            await Task.Factory.StartNew(() =>
            {

                List<Country> Country = db.Country.OrderBy(x => x.Name).ToList();
                branches.ListCountry = new List<SelectListItem>();
                Country.ForEach(x =>
                {
                    branches.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                });
            });

            return View(branches);
        }

        // POST: /Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      
       
        [CheckSessionOut]
        public async Task<string> Edit( branches branches)
        {
           

            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {




                    if (branches.id_branch >0 )
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                        var bra = db.Branch.Where(x => x.id_branch == branches.id_branch && x.id_Subscription == idSubcription).FirstOrDefault();
                        if (bra != null)
                        {
                            bra.branchName = branches.branchName;
                            bra.branchCity = branches.branchCity;
                            bra.branchCountry = branches.branchCountry;
                            bra.branchAddress = bra.branchAddress;
                            

                            db.Entry(bra).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            rr.isSuccess = true;
                            rr.Message = "Update Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.Message = "Branch does no exists";
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

        // GET: /Branches/Delete/5
        
        [CheckSessionOut]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            branches branches = db.Branch.Where(x => x.id_Subscription == idSubcription && x.id_branch == id).FirstOrDefault();
            if (branches == null)
            {
                return HttpNotFound();
            }
            return View(branches);
        }

        // POST: /Branches/Delete/5
       
        [HttpPost, ActionName("Delete")]
        [CheckSessionOut]

        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            branches branches =  db.Branch.Where(x => x.id_Subscription == idSubcription && x.id_branch == id).FirstOrDefault();
            db.Branch.Remove(branches);
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

        private Select2PagedResult AttendeesToSelect2Format(List<Country> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2Result>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (Country a in attendees)
            {
                jsonAttendees.Results.Add(new Select2Result { id = a.Id.ToString(), text = a.Name  });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }
    }


    //Extra classes to format the results the way the select2 dropdown wants them
    public class Select2PagedResult
    {
        public int Total { get; set; }
        public List<Select2Result> Results { get; set; }
    }

    public class Select2Result
    {
        public string id { get; set; }
        public string text { get; set; }
    }
}
