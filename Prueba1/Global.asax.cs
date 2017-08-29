using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Prueba1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            HttpContext.Current.Session["Config"] = "en-US";
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                var configSession = (string) HttpContext.Current.Session["Config"];

                if (configSession == null)
                {
                    configSession = "es-MX";
                    HttpContext.Current.Session.Add("Config", configSession);
                }

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(configSession);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(configSession);
            }
        }
    }
}
