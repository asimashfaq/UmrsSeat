using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UmarSeat.Helpers
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            // check  sessions here
            if (HttpContext.Current.Session["idSubscription"] == null)
            {
              
                string redirectTo = "~/Account/Login";
                if (!string.IsNullOrEmpty(ctx.Request.RawUrl))
                {
                    redirectTo = string.Format("~/Account/Login?ReturnUrl={0}",
                        HttpUtility.UrlEncode(ctx.Request.RawUrl));
                }
                filterContext.Result = new RedirectResult(redirectTo);
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckSessionOutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Session != null)
            {
                if (context.Session.IsNewSession)
                {
                    string sessionCookie = context.Request.Headers["Cookie"];

                    if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        FormsAuthentication.SignOut();
                        string redirectTo = "~/Account/Login";
                        if (!string.IsNullOrEmpty(context.Request.RawUrl))
                        {
                            redirectTo = string.Format("~/Account/Login?ReturnUrl={0}",
                                HttpUtility.UrlEncode(context.Request.RawUrl));
                        }
                        filterContext.Result = new RedirectResult(redirectTo);

                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}