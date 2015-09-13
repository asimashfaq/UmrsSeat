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

namespace UmarSeat.Controllers
{
    [Authorize]
    public class SectorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Sector/
        public ActionResult Index()
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<Sector> lsecotr = db.Database.SqlQuery<Sector>(String.Format("SELECT dbo.Countries.id, dbo.Countries.name, dbo.Sectors.id_Sector, dbo.Sectors.sectorName, dbo.Sectors.id_Subscription, dbo.Sectors.airline, dbo.Sectors.category, dbo.Countries.name AS country FROM     dbo.Countries RIGHT OUTER JOIN  dbo.Sectors ON dbo.Countries.id = dbo.Sectors.country where dbo.Sectors.id_Subscription = {0} order by dbo.Sectors.id_Sector DESC",idSubcription)).ToList<Sector>();
            return View(lsecotr);
        }

        public string getSectors(Sector sector)
        {
            
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Dictionary<string, List<Sector>> sdata = new Dictionary<string, List<Sector>>();
            List<Sector> sectors = db.Sector.Where(x => x.id_Subscription == idSubcription).ToList();
            List<Sector> outSectors = new List<Sector>() ;
            List<Sector> inSectors = new List<Sector>() ;
            if(sectors.Count>0)
            {
                if(!String.IsNullOrEmpty(sector.country))
                {
                    outSectors = sectors.Where(x => x.country == sector.country && (x.category == "Both" || x.category == "Outbound Sector")).ToList();
                }
                if (!String.IsNullOrEmpty(sector.airline))
                {
                    outSectors = sectors.Where(x => x.airline == sector.airline && (x.category == "Both" || x.category == "Outbound Sector")).ToList();
                }
                if (!String.IsNullOrEmpty(sector.country))
                {
                    inSectors = sectors.Where(x => x.country == sector.country && (x.category == "Both" || x.category == "Inbound Sector")).ToList();
                }
                if (!String.IsNullOrEmpty(sector.airline))
                {
                    inSectors = sectors.Where(x => x.airline == sector.airline && (x.category == "Both" || x.category == "Inbound Sector")).ToList();
                }
                sdata.Add("outbound", outSectors);
                sdata.Add("inbound", inSectors);
              
            }

            return JsonConvert.SerializeObject(sdata);
        }
       
        public ActionResult Create(string country, string airline)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Sector st = new Sector();

            st.airline = airline;
            st.country = country;

                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<airLine> airlinelist = db1.Airline.Where(x => x.id_Subscription == idSubcription).ToList();
                    
                    db1.Dispose();
                    st.ListAirline = new List<SelectListItem>();
                    airlinelist.ForEach(x =>
                    {
                        st.ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
                    });
                }));
               

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<Country> Country = db.Country.OrderBy(x => x.Name).ToList(); db1.Dispose();
                    Country con = Country.Where(x => x.Name == st.country).SingleOrDefault();
                    if(con != null)
                    {
                        st.country = con.Id;
                    }
                    
                    st.ListCountry = new List<SelectListItem>();
                    Country.ForEach(x =>
                    {
                        st.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                    });
                }));
                

                Task.WaitAll(tasks.ToArray());



           
            return View(st);
        }

        // POST: /Sector/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<string> Create(Sector sector)
        {
            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                sector.id_Subscription = Convert.ToInt32(Session["idSubscription"].ToString());
                var se = db.Sector.Where(x => x.sectorName == sector.sectorName && x.id_Subscription == sector.id_Subscription).FirstOrDefault();
                if (se == null)
                {
                    db.Sector.Add(sector);
                    await db.SaveChangesAsync();
                    rr.isSuccess = true;
                    rr.Message = "Insert Successfully";
                    errors.Add(rr);
                }
                else
                {

                    rr.isSuccess = false;
                    rr.ErrorMessage = "Already exists";
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


            return JsonConvert.SerializeObject(errors); 
        }

        // GET: /Sector/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Sector sector = await db.Sector.Where(x => x.id_Sector == id && x.id_Subscription == idSubcription).FirstOrDefaultAsync();
            if (sector == null)
            {
                return HttpNotFound();
            }

           

            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(() =>
            {
                ApplicationDbContext db1 = new ApplicationDbContext();
                List<airLine> airline = db1.Airline.Where(x => x.id_Subscription == idSubcription).ToList();
                db1.Dispose();
                sector.ListAirline = new List<SelectListItem>();
                airline.ForEach(x =>
                {
                    sector.ListAirline.Add(new SelectListItem { Text = x.airlineName, Value = x.airlineName.ToString() });
                });
            }));


            tasks.Add(Task.Factory.StartNew(() =>
            {
                ApplicationDbContext db1 = new ApplicationDbContext();
                List<Country> Country = db.Country.OrderBy(x => x.Name).ToList(); db1.Dispose();
                sector.ListCountry = new List<SelectListItem>();
                Country.ForEach(x =>
                {
                    sector.ListCountry.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                });
            }));


            Task.WaitAll(tasks.ToArray());

            return View(sector);
        }

        // POST: /Sector/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
     
        public async Task<string> Edit( Sector sector)
        {
            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                sector.id_Subscription = Convert.ToInt32(Session["idSubscription"].ToString());
                var se = db.Sector.Where(x => x.id_Sector == sector.id_Sector && x.id_Subscription == sector.id_Subscription).FirstOrDefault();
                if (se != null)
                {
                    se.sectorName = sector.sectorName;
                    se.country = sector.country;
                    se.airline = sector.airline;
                    se.category = sector.category;
                    db.Entry(se).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    rr.isSuccess = true;
                    rr.Message = "Update Successfully";
                    errors.Add(rr);
                }
                else
                {

                    rr.isSuccess = false;
                    rr.ErrorMessage = "Sector does not exists";
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


            return JsonConvert.SerializeObject(errors); 
        }

        // GET: /Sector/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Sector sector = await db.Sector.Where(x => x.id_Sector == id && x.id_Subscription == idSubcription).FirstOrDefaultAsync();
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        // POST: /Sector/Delete/5
        [HttpPost, ActionName("Delete")]
      
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Sector sector =await db.Sector.Where(x=> x.id_Sector == id && x.id_Subscription == idSubcription).FirstOrDefaultAsync();
            db.Sector.Remove(sector);
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
