﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UmarSeat.Helpers;
using UmarSeat.Models;

namespace UmarSeat.Controllers
{
    [Authorize]
    public class UserManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: /UserManagement/
        [CheckSessionOut]
        public ActionResult Index()
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<ApplicationUser> users = db.Users.Include(x=> x.PersonInfo).Where( x=> x.id_Subscription == idSubcription).ToList();

            return View(users);
        }


        [CheckSessionOut]
        public ActionResult Addroletouser(string id)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            ManageUserRoles mur = new ManageUserRoles();
            
            var user = db.Users.Include(x=> x.PersonInfo).Where(x => x.Id == id).SingleOrDefault();
            if(user != null)
            {
                mur.roleName = user.userRole;
                mur.branchName = user.PersonInfo.First().branchName;
                mur.userId = id;
            }
            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(() =>
            {
                ApplicationDbContext db1 = new ApplicationDbContext();
                List<ApplicationUser> users = db1.Users.Include(x => x.PersonInfo).Where(x => x.Id == id && x.AccountStatus == AccountStatus.Active && x.id_Subscription == idSubcription).ToList();
                mur.listUsers = new List<SelectListItem>();
                users.ForEach(x =>
                {
                    mur.listUsers.Add(new SelectListItem { Text = x.PersonInfo.First().firstName + " " + x.PersonInfo.First().lastName + " (" + x.PersonInfo.First().email + ")", Value = x.Id.ToString() });
                });

            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                ApplicationDbContext db1 = new ApplicationDbContext();
                List<UserRoles> userroles = db1.UserRole.Where(x => x.id_Subscription == idSubcription).GroupBy(x => x.userRolesType).Select(x => x.FirstOrDefault()).ToList();
                mur.listRoles = new List<SelectListItem>();
                userroles.ForEach(x =>
                {
                    mur.listRoles.Add(new SelectListItem { Text = x.userRolesType, Value = x.userRolesType.ToString() });
                });

            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                ApplicationDbContext db1 = new ApplicationDbContext();
                List<branches> branches = db1.Branch.Where(x => x.id_Subscription == idSubcription).ToList();
                mur.listBranches = new List<SelectListItem>();
                mur.listBranches.Add(new SelectListItem { Text = "All Branches", Value = "" });
                branches.ForEach(x =>
                {
                    mur.listBranches.Add(new SelectListItem { Text = x.branchName, Value = x.branchName.ToString() });
                });

            }));
            

            Task.WaitAll(tasks.ToArray());
            if (mur.listRoles.Count > 0)
                return View(mur);
            else
                return View("noRoleFound");
        }

        [HttpPost]
        [CheckSessionOut]
        public string Addroletouser(ManageUserRoles mur)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());

            ResponseRequest rr = new ResponseRequest();
            if (mur != null)
            {
                if (mur.roleName != null && mur.roleName !="Super User")
                {
                    UserRoles userole = db.UserRole.Where(x => x.userRolesType == mur.roleName && x.id_Subscription == idSubcription).FirstOrDefault();
                    List<string> newRoles = userole.userRolesName.Split(',').ToList();
                    ApplicationUser user = db.Users.Where(x => x.Id == mur.userId && x.AccountStatus == AccountStatus.Active && x.id_Subscription == idSubcription).SingleOrDefault();

                  //  var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                    var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    if (user != null)
                    {
                       
                        UserRoles oldRoles = db.UserRole.Where(x => x.userRolesType == user.userRole && x.id_Subscription == idSubcription).FirstOrDefault();
                        if(oldRoles != null)
                        {
                            List<string> roles = oldRoles.userRolesName.Split(',').ToList();

                            IEnumerable<string> oldRoles1 = roles.Except(newRoles);
                            newRoles = newRoles.Except(oldRoles1).ToList();


                            foreach (string role in oldRoles1)
                                    {
                                        um.RemoveFromRole(user.Id, role);
                                    }
                                    user.userRole = "None";
                                    db.Entry(user).State = EntityState.Modified;
                                    db.SaveChanges();
                                
                        }
                        foreach (string role in newRoles)
                        {
                            if (!um.IsInRole(user.Id, role))
                            {
                                um.AddToRole(user.Id, role);
                            }
                        }

                        user.userRole = mur.roleName;
                        user.id_Subscription = idSubcription;
                        user.requiredLogout = true;

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();


                        person p = db.Persons.Where(x => x.userId == mur.userId).SingleOrDefault();
                        if (p != null)
                        {
                            p.branchName = mur.branchName;
                            if(string.IsNullOrEmpty(p.branchName))
                            {
                                p.branchName = "";
                            }
                            db.Entry(p).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        rr.isSuccess = true;
                        rr.Message = "Successfully assigned Permissions!";



                    }
                    else
                    {
                        rr.isSuccess = false;
                        rr.ErrorMessage = "User not found";
                    }



                   

                }
                else
                {
                    rr.isSuccess = false;
                    rr.ErrorMessage = "Role name not found/ InValid Role";
                }
            }
            else
            {
                rr.isSuccess = false;
                rr.ErrorMessage = "No data Recevied";
            }
           
            return JsonConvert.SerializeObject(rr);
        }

        public ActionResult InviteUser()
        {
            return View();
        }

        [HttpPost]
        [CheckSessionOut]
        public string InviteUser(string email)
        {
            ResponseRequest rr = new ResponseRequest();
            try
            {

                string dominanem = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                string body = string.Format("Message from PSSP System" +
               "This email sent by the PSSP system<br />" +
               "<a href='{0}/invitation/Request?id={1}'>Click here to create Account!</b>",dominanem, Session["idSubscription"].ToString());
                SendMessage(email, "Invitation For creating Account", body);
                rr.isSuccess = true;
                rr.Message = "Invitation had been sent successfully!.";
            }
            catch (Exception ex)
            {
                rr.isSuccess = false;
                rr.Message = "Invitation failed. "+ex.ToString()+" "+ex.InnerException.ToString();
            }


            return JsonConvert.SerializeObject(rr); 
        }
        public void SendMessage(string toAddress, string subjectText, string bodyText)
        {
            //create a object to hold the message
            MailMessage newMessage = new MailMessage();

            //Create addresses
            MailAddress senderAddress = new MailAddress("contact@amglobaltechs.com");
            MailAddress recipentAddress = new MailAddress(toAddress);

            //Now create the full message
            newMessage.To.Add(recipentAddress);
            newMessage.From = senderAddress;
            newMessage.Subject = subjectText;
            newMessage.Body = bodyText;

            //Create the SMTP Client object, which do the actual sending
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("contact@amglobaltechs.com", "Asim@123#");
            client.Host = "mail.amglobaltechs.com";
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = false;
            newMessage.IsBodyHtml = true;


            //now send the message
            client.Send(newMessage);
        }
        [HttpPost]
        [CheckSessionOut]
        public ActionResult approved(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                ApplicationUser user = db.Users.Where(x => x.Id == id && x.id_Subscription == idSubcription).FirstOrDefault();
                if(user != null)
                {
                    user.AccountStatus = AccountStatus.Active;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }
            return null;

        }
        [HttpPost]
        [CheckSessionOut]
        public ActionResult blocked(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                Subscription su = db.Subscription.Where(x => x.UserId == id).SingleOrDefault();
                if(su == null)
                {
                    ApplicationUser user = db.Users.Where(x => x.Id == id && x.id_Subscription == idSubcription).FirstOrDefault();
                    if (user != null)
                    {
                        user.AccountStatus = AccountStatus.Blocked;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
               
            }
            else
            {
                return HttpNotFound();
            }
            return null;

        }
        [HttpPost]
        [CheckSessionOut]
        public ActionResult delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                Subscription su = db.Subscription.Where(x => x.UserId == id).SingleOrDefault();
                if(su == null)
                {
                    ApplicationUser user = db.Users.Include(x => x.PersonInfo).Where(x => x.Id == id && x.id_Subscription == idSubcription).FirstOrDefault();
                    if (user != null)
                    {
                        db.Persons.Remove(user.PersonInfo.First());
                        db.Users.Remove(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                
            }
            else
            {
                return HttpNotFound();
            }
            return null;

        }
	}
}