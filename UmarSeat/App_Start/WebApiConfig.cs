using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using UmarSeat.Models;
using System.Web.Http.Tracing;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using UmarSeat.Controllers;

namespace UmarSeat
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.AddODataQueryFilter();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<SeatConfirmation>("SeatConfirmation");
           // config.Routes.MapODataRoute("Odata", "odata", builder.GetEdmModel());
            config.MapODataServiceRoute("Odata", "odata",model: builder.GetEdmModel());
            config.AddODataQueryFilter();
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
