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
using Microsoft.AspNet.Identity.EntityFramework;

namespace UmarSeat.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Roles/
        public async Task<ActionResult> Index()
        {
            return View(await db.Roles.ToListAsync());
        }

        // GET: /Roles/Details/5
   

        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult BulkCreate()
        {
            return View();
        }

        // POST: /Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(new IdentityRole() { 
                Name = role.Name
                });
               await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BulkCreate(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(new IdentityRole()
                {
                    Name = "Read"+role.Name
                });
                db.Roles.Add(new IdentityRole()
                {
                    Name = "Create" + role.Name
                });
                db.Roles.Add(new IdentityRole()
                {
                    Name = "Delete" + role.Name
                });
                db.Roles.Add(new IdentityRole()
                {
                    Name = "Update" + role.Name
                });

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: /Roles/Edit/5
        public ActionResult Edit(string rolename)
        {
            if (rolename == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           var thisRole = db.Roles.Where(r => r.Name.Equals(rolename, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
           if (thisRole == null)
            {
                return HttpNotFound();
            }
           return View(thisRole);
        }

        // POST: /Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
               
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: /Roles/Delete/5
        public ActionResult Delete(string rolename)
        {
            if (rolename == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var thisRole = db.Roles.Where(r => r.Name.Equals(rolename, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (thisRole == null)
            {
                return HttpNotFound();
            }
            return View(thisRole);
        }

        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string rolename)
        {
            if (rolename == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var thisRole = db.Roles.Where(r => r.Name.Equals(rolename, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
           
            if (thisRole == null)
            {
                return HttpNotFound();
            }
            db.Roles.Remove(thisRole);
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
