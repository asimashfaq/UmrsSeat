using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using UmarSeat.Models;
using System.Web.Http.Tracing;
using System.Web.Http.OData.Builder;
using System.Web.OData.Extensions;

namespace UmarSeat
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.EnableQuerySupport();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Employee>("Employees");
            config.Routes.MapODataRoute("Odata", "odata", builder.GetEdmModel());
            
        }
    }
}
