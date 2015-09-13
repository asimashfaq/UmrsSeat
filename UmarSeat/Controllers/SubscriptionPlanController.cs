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

namespace UmarSeat.Controllers
{
    [Authorize]
    public class SubscriptionPlanController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SubscriptionPlan/
        public async Task<ActionResult> Index()
        {
            return View(await db.SubscriptionPlan.ToListAsync());
        }

        // GET: /SubscriptionPlan/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscriptionPlan subscriptionplan = await db.SubscriptionPlan.FindAsync(id);
            if (subscriptionplan == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionplan);
        }

        // GET: /SubscriptionPlan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /SubscriptionPlan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="id_SubscriptionPlan,nameSubscriptionPlan,duration,subscriptionDurationType,subscriptionPrice,createdAt,updatedAt")] subscriptionPlan subscriptionplan)
        {
            if (ModelState.IsValid)
            {
                subscriptionplan.createdAt = subscriptionplan.updatedAt = DateTime.Now;
               
                db.SubscriptionPlan.Add(subscriptionplan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(subscriptionplan);
        }

        // GET: /SubscriptionPlan/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscriptionPlan subscriptionplan = await db.SubscriptionPlan.FindAsync(id);
            if (subscriptionplan == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionplan);
        }

        // POST: /SubscriptionPlan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="id_SubscriptionPlan,nameSubscriptionPlan,duration,subscriptionDurationType,subscriptionPrice,createdAt,updatedAt")] subscriptionPlan subscriptionplan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscriptionplan).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subscriptionplan);
        }

        // GET: /SubscriptionPlan/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscriptionPlan subscriptionplan = await db.SubscriptionPlan.FindAsync(id);
            if (subscriptionplan == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionplan);
        }

        // POST: /SubscriptionPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            subscriptionPlan subscriptionplan = await db.SubscriptionPlan.FindAsync(id);
            db.SubscriptionPlan.Remove(subscriptionplan);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
