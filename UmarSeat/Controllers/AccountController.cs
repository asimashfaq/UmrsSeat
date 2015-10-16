using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using UmarSeat.Models;
using System.Data.Entity;
using UmarSeat.Helpers;
using Microsoft.Owin.Security.OAuth;
using UmarSeat.providers;
using Microsoft.Owin;
using System.Net.Http;
using System.Text;

namespace UmarSeat.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

      

        internal async Task<string> GetBearerToken(string siteUrl, string Username, string Password)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(siteUrl);
            client.DefaultRequestHeaders.Accept.Clear();

            HttpContent requestContent = new StringContent("grant_type=password&username=" + Username + "&password=" + Password, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage responseMessage = await client.PostAsync("Token", requestContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                TokenResponseModel response = await responseMessage.Content.ReadAsAsync<TokenResponseModel>();
                return response.access_token;
            }

            return "";
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            string dominanem = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                   

                    await SignInAsync(user, model.RememberMe);
                    Session["idSubscription"] = user.id_Subscription;
                    Session["branchName"] = user.PersonInfo.First().branchName;
                    Session["userId"] = user.Id;
                    List<Task> tt = new List<Task>();
                    tt.Add(Task.Factory.StartNew(() => {
                        if (user.requiredLogout == true)
                        {
                            ApplicationDbContext db = new ApplicationDbContext();
                            var u = db.Users.Where(x => x.Id == user.Id && x.requiredLogout == true).SingleOrDefault();
                            if (u != null)
                            {
                                u.requiredLogout = false;
                                db.Entry(u).State = System.Data.Entity.EntityState.Modified;
                                 db.SaveChanges();
                            }

                            db.Dispose();
                        }

                    }));
                    tt.Add(Task.Factory.StartNew(() => {
                        if (Session["menulinks"] == null)
                        {
                            menuList ml = new menuList();
                            Session["menulinks"] = ml.getLinks(user.Roles.ToList());
                        }
                    }));
                    Session["access_token"] = await GetBearerToken(dominanem, model.UserName, model.Password);


                    Task.WaitAll(tt.ToArray());
                    
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public async Task<ActionResult> Register()
        {
            

            await Task.Factory.StartNew(() =>
            {
                ApplicationDbContext db = new ApplicationDbContext();
                ViewData["subscriptionPlans"] = new SelectList((from s in db.SubscriptionPlan.ToList()
                                                                select new
                                                                {
                                                                    id_SubscriptionPlan = s.id_SubscriptionPlan,
                                                                    nameSubscriptionPlan = s.nameSubscriptionPlan + " " + s.duration + " " + s.subscriptionDurationType + " $" + s.subscriptionPrice
                                                                }), "id_SubscriptionPlan", "nameSubscriptionPlan");
            });
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
      
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            string dominanem = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
            if (ModelState.IsValid)
            {
                model.PersonInfo.createdAt = model.PersonInfo.updatedAt = DateTime.Now;

                ApplicationDbContext db = new ApplicationDbContext();

                var isUser = db.Users.Where(x => x.UserName == model.UserName || x.PersonInfo.FirstOrDefault().email == model.PersonInfo.email).FirstOrDefault();
                if(isUser == null)
                {
                    try
                    {
                        int id_SubscriptionPlan = Convert.ToInt32(Request.Form["subscriptionPlans"].ToString());
                        var user = new ApplicationUser() { UserName = model.UserName, id_SubscriptionPlan = id_SubscriptionPlan, AccountStatus = AccountStatus.Active, userRole = "Super User" };
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {


                            string UserId = user.Id;

                            List<Task> tt = new List<Task>();
                            tt.Add(Task.Factory.StartNew(() => {
                                model.PersonInfo.userId = UserId;
                                model.PersonInfo.branchName = "";
                                ApplicationDbContext db1 = new ApplicationDbContext();
                                db1.Persons.Add(model.PersonInfo);
                                db1.SaveChanges();

                                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                                um.AddToRole(UserId, Role.Administrator);
                              
                            }));
                            tt.Add(Task.Factory.StartNew(() => {
                                ApplicationDbContext db1 = new ApplicationDbContext();
                                subscriptionPlan sp = db1.SubscriptionPlan.Where(x => x.id_SubscriptionPlan == id_SubscriptionPlan).SingleOrDefault();
                                if (sp != null)
                                {
                                    DateTime startDate = DateTime.Now;
                                    DateTime endDate = DateTime.Now;
                                    Subscription sub = new Subscription { startDate = startDate, cDate = DateTime.Now, UserId = UserId };
                                    if (sp.subscriptionDurationType == DurationType.Day || sp.subscriptionDurationType == DurationType.Days)
                                    {
                                        endDate = endDate.AddDays(sp.duration);
                                    }
                                    else if (sp.subscriptionDurationType == DurationType.Month || sp.subscriptionDurationType == DurationType.Months)
                                    {
                                        endDate = endDate.AddMonths(sp.duration);
                                    }
                                    else if (sp.subscriptionDurationType == DurationType.Year || sp.subscriptionDurationType == DurationType.Years)
                                    {
                                        endDate = endDate.AddYears(sp.duration);
                                    }
                                    sub.endDate = endDate;
                                    sub.SubscriptionStatus = SubscriptionStatus.Active;
                                    db1.Subscription.Add(sub);
                                    db1.SaveChanges();

                                    user = db1.Users.Where(x => x.Id == UserId).SingleOrDefault();
                                    if (user != null)
                                    {
                                        user.id_Subscription = sub.id_Subscription;
                                        db1.Entry(user).State = EntityState.Modified;
                                        db1.SaveChanges();
                                    }

                                 
                                }



                               
                            }));
                        
                          

                            await SignInAsync(user, isPersistent: false);
                            Task.WaitAll(tt.ToArray());

                            Session["idSubscription"] = user.id_Subscription;
                            Session["branchName"] = user.PersonInfo.First().branchName;
                            Session["userId"] = user.Id;
                            if (Session["menulinks"] == null)
                            {
                                menuList ml = new menuList();
                                Session["menulinks"] = ml.getLinks(user.Roles.ToList());
                            }
                            Session["access_token"] = await GetBearerToken(dominanem, model.UserName, model.Password);
                            

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                        //ModelState.AddModelError("UE", "Kindly Select Subscription Plan");
                    }
                }
                else
                {
                    ModelState.AddModelError("UE", "Username or email already exists");
                }

              
            }
            await Task.Factory.StartNew(() =>
            {
                ApplicationDbContext db = new ApplicationDbContext();
                ViewData["subscriptionPlans"] = new SelectList((from s in db.SubscriptionPlan.ToList()
                                                                select new
                                                                {
                                                                    id_SubscriptionPlan = s.id_SubscriptionPlan,
                                                                    nameSubscriptionPlan = s.nameSubscriptionPlan + " " + s.duration + " " + s.subscriptionDurationType + " $" + s.subscriptionPrice
                                                                }), "id_SubscriptionPlan", "nameSubscriptionPlan");
            });

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}