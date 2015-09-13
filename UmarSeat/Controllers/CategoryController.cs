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
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Category/
        public async Task<ActionResult> Index()
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            return View(await db.Category.Where(x=> x.id_Subscription  == idSubcription).ToListAsync());
        }

       

        // GET: /Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
     
        public async Task<string> Create(Category category)
        {
            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
               
                category.id_Subscription = Convert.ToInt32(Session["idSubscription"].ToString());
                var cat = db.Category.Where(x => x.categoryName == category.categoryName && x.id_Subscription == category.id_Subscription).FirstOrDefault();
                if(cat == null)
                {
                    db.Category.Add(category);
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

        // GET: /Category/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Category category = await db.Category.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: /Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      
        public async Task<string> Edit( Category category)
        {
            if (ModelState.IsValid)
            {
              
                
            }
            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {

                category.id_Subscription = Convert.ToInt32(Session["idSubscription"].ToString());
                var cat = db.Category.Where(x => x.id_Category == category.id_Category && x.id_Subscription == category.id_Subscription).FirstOrDefault();
                if (cat != null)
                {
                    cat.categoryName = category.categoryName;
                    db.Entry(cat).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    rr.isSuccess = true;
                    rr.Message = "Update Successfully";
                    errors.Add(rr);
                }
                else
                {

                    rr.isSuccess = false;
                    rr.ErrorMessage = "Category does  not exists";
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

        // GET: /Category/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Category category = db.Category.Where(x => x.id_Category == id && x.id_Subscription == idSubcription).FirstOrDefault();
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
      
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Category category = db.Category.Where(x => x.id_Category == id && x.id_Subscription == idSubcription).FirstOrDefault();
            db.Category.Remove(category);
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
