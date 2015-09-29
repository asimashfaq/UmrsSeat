using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using UmarSeat.Models;
namespace UmarSeat.Controllers
{
   
    public class SeatConfirmationController : ODataController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Skip |
                               AllowedQueryOptions.Top | AllowedQueryOptions.Select | AllowedQueryOptions.Filter)]
        [Authorize]
        public IQueryable<SeatConfirmation> Get()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

           
            var identity = User.Identity as ClaimsIdentity;

            var claims = from c in identity.Claims.Where(x=> x.Type=="idSubscription")
                         select new
                         {
                             subject = c.Subject.Name,
                             type = c.Type,
                             value = c.Value
                         };

            int sub = Convert.ToInt32(claims.First().value);
            return db.SeatConfirmation.Where(x=> x.id_Subscription==sub);
        }

    }
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }

        public static IQueryable<Order> CreateOrders()
        {
            List<Order> OrderList = new List<Order>
            {
                new Order {OrderID = 10248, CustomerName = "Taiseer Joudeh", ShipperCity = "Amman", IsShipped = true },
                new Order {OrderID = 10249, CustomerName = "Ahmad Hasan", ShipperCity = "Dubai", IsShipped = false},
                new Order {OrderID = 10250,CustomerName = "Tamer Yaser", ShipperCity = "Jeddah", IsShipped = false },
                new Order {OrderID = 10251,CustomerName = "Lina Majed", ShipperCity = "Abu Dhabi", IsShipped = false},
                new Order {OrderID = 10252,CustomerName = "Yasmeen Rami", ShipperCity = "Kuwait", IsShipped = true}
            };

            return OrderList.AsQueryable();
        }
    }
}