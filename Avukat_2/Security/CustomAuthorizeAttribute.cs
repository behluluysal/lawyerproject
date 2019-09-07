using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Avukat_2.Models;

namespace Avukat_2.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(SessionPersister.Username))
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccessDenied", action = "Index" }));
            else
            {
                OurDbContext db = new OurDbContext();

                Admin test = db.userAccounts.Where(u => u.Username == SessionPersister.Username).FirstOrDefault();
                CustomPrincipal mp = new CustomPrincipal(test);
                if (!mp.IsInRole(Roles))
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccessDenied", action = "Index" }));
                //  Account test = db.userAccounts.Where(u => u.Username == SessionPersister.Username).FirstOrDefault();
                //   CustomPrincipal mp = new CustomPrincipal(test);am.find(SessionPersister.Username)
            }
        }
    }
}