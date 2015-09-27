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
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using UmarSeat.Helpers;

namespace UmarSeat.Controllers
{
    [Authorize]
    public class UserRoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /UserRole/
        [CheckSessionOut]
        public async Task<ActionResult> Index()
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            return View(await db.UserRole.Where(x=> x.id_Subscription ==  idSubcription || x.id_Subscription== 0).ToListAsync());
        }


        // GET: /UserRole/Create
        [CheckSessionOut]
        public ActionResult Create()
        {
            List<IdentityRole> roles = db.Database.SqlQuery<IdentityRole>("SELECT Name ,Id FROM [dbo].[AspNetRoles] where Name!= 'SuperAdmin' order by  RIGHT(RTRIM(Name), 5) ").ToList<IdentityRole>();
            List<Dictionary<string,string>> roleslist = new List<Dictionary<string,string>>();
            int i = 0;
            Dictionary<string, string> r1 = new Dictionary<string, string>();
            r1["Create"] = "";
            r1["Read"] = "";
            r1["Update"] = "";
            r1["Delete"] = "";
            foreach(IdentityRole role in roles)
            {
                

                if(role.Name.Contains("Create")||role.Name.Contains("Read")||role.Name.Contains("Update")||role.Name.Contains("Delete"))
                {
                    if (role.Name.Contains("Create")) { r1["Create"] = role.Name.ToString(); }
                    if (role.Name.Contains("Read")) { r1["Read"] = role.Name; }
                    if (role.Name.Contains("Update")) { r1["Update"] = role.Name; }
                    if (role.Name.Contains("Delete")) { r1["Delete"] = role.Name; }
                    i++;
                    if (i == 4)
                    {
                        i = 0;
                        roleslist.Add(r1);
                        r1 = new Dictionary<string, string>();
                        r1["Create"] = "";
                        r1["Read"] = "";
                        r1["Update"] = "";
                        r1["Delete"] = "";
                    }
                }
                else
                {
                    r1["other"] = role.Name.ToString();

                    roleslist.Add(r1);
                    r1 = new Dictionary<string, string>();
                    r1["Create"] = "";
                    r1["Read"] = "";
                    r1["Update"] = "";
                    r1["Delete"] = "";
                }
              
            }
            ViewBag.roles = roleslist;
            return View();
        }

      


        // POST: /UserRole/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckSessionOut]
        public async Task<ActionResult> Create([Bind(Include="id_UserRroles,userRolesType,userRolesName")] UserRoles userroles)
        {
            var rp = Request.Form["rp"];
            String[] roles = rp.Split(',');
            if (ModelState.IsValid)
            {
                string sroles = "";
                foreach(string s in roles.Where(x=> x != "false"))
                {
                    if(s!= "false")
                    {
                       
                        sroles = sroles + s + ",";
                      
                    }

                }
                int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                userroles.id_Subscription = idSubcription;
                userroles.userRolesName = sroles.TrimEnd(',');
                db.UserRole.Add(userroles);
                await db.SaveChangesAsync();
               
                return RedirectToAction("Index");
            }

            return View(userroles);
        }

        // GET: /UserRole/Edit/5
        [CheckSessionOut]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRoles userroles = await db.UserRole.FindAsync(id);
            if (userroles == null)
            {
                return HttpNotFound();
            }
            if (userroles.id_Subscription == 0)
            {
                return HttpNotFound();
            }


            List<IdentityRole> roles = db.Database.SqlQuery<IdentityRole>("SELECT Name ,Id FROM [dbo].[AspNetRoles] where Name!= 'SuperAdmin' order by  RIGHT(RTRIM(Name), 5) ").ToList<IdentityRole>();
            List<Dictionary<string, string>> roleslist = new List<Dictionary<string, string>>();
            int i = 0;
            Dictionary<string, string> r1 = new Dictionary<string, string>();
            r1["Create"] = "";
            r1["Read"] = "";
            r1["Update"] = "";
            r1["Delete"] = "";
            foreach (IdentityRole role in roles)
            {


                if (role.Name.Contains("Create") || role.Name.Contains("Read") || role.Name.Contains("Update") || role.Name.Contains("Delete"))
                {
                    if (role.Name.Contains("Create")) { r1["Create"] = role.Name.ToString(); }
                    if (role.Name.Contains("Read")) { r1["Read"] = role.Name; }
                    if (role.Name.Contains("Update")) { r1["Update"] = role.Name; }
                    if (role.Name.Contains("Delete")) { r1["Delete"] = role.Name; }
                    i++;
                    if (i == 4)
                    {
                        i = 0;
                        roleslist.Add(r1);
                        r1 = new Dictionary<string, string>();
                        r1["Create"] = "";
                        r1["Read"] = "";
                        r1["Update"] = "";
                        r1["Delete"] = "";
                    }
                }
                else
                {
                    r1["other"] = role.Name.ToString();

                    roleslist.Add(r1);
                    r1 = new Dictionary<string, string>();
                    r1["Create"] = "";
                    r1["Read"] = "";
                    r1["Update"] = "";
                    r1["Delete"] = "";
                }

            }
            ViewBag.roles = roleslist;
            return View(userroles);
        }

        // POST: /UserRole/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckSessionOut]
        public async Task<ActionResult> Edit([Bind(Include="id_UserRroles,userRolesType,userRolesName")] UserRoles userroles)
        {
             userroles = await db.UserRole.FindAsync(userroles.id_UserRroles);
            if (userroles.id_Subscription == 0)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.Entry(userroles).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userroles);
        }

        // GET: /UserRole/Delete/5
     

        // POST: /UserRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [CheckSessionOut]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UserRoles userroles = await db.UserRole.FindAsync(id);

         
            if (userroles.id_Subscription == 0)
            {
                return HttpNotFound();
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<string> roles = userroles.userRolesName.Split(',').ToList();
            List<ApplicationUser> users = db.Users.Where(x => x.userRole == userroles.userRolesType && x.id_Subscription == idSubcription).ToList();

            if(users.Count>0)
            {
                foreach(ApplicationUser user in users)
                {
                    //  var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                    var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));


                    foreach (string role in roles)
                        {
                            um.RemoveFromRole(user.Id, role);
                        }
                    user.userRole = "None";
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }


            db.UserRole.Remove(userroles);
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
