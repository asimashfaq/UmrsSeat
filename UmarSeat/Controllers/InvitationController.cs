using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UmarSeat.Models;
using Microsoft.AspNet.Identity;

namespace UmarSeat.Controllers
{
    public class InvitationController : AccountController
    {
        //
        // GET: /Invitation/
         [AllowAnonymous]
        public ActionResult Request(int Id)
        {
            RegisterViewModel rvm = new RegisterViewModel();
             
            rvm.PersonInfo = new person();
            rvm.idSubscription = Id;
            ViewBag.display = false;
            return View(rvm);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
         public async Task<ActionResult> Request(int Id, RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.PersonInfo.createdAt = model.PersonInfo.updatedAt = DateTime.Now;


                var user = new ApplicationUser() { UserName = model.UserName, id_Subscription = model.idSubscription, AccountStatus = AccountStatus.Pending, userRole="None" };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                   
                    string UserId = user.Id;
                    ApplicationDbContext db = new ApplicationDbContext();

                    await Task.Factory.StartNew(() =>
                    {
                        model.PersonInfo.userId = UserId;

                        db.Persons.Add(model.PersonInfo);
                        db.SaveChanges();
                    });
                    model = new RegisterViewModel();
                    ViewBag.success = true;
                    ViewBag.message = "Your account had been created and is pending  for admin approval";
                }
                else
                {
                    ViewBag.success = false;
                    ViewBag.message = "Unable to create your account";
                }
               
            }

            ViewBag.display = true;
            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}