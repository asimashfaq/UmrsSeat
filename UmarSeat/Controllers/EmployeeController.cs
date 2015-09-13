using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using UmarSeat.Helpers;
using UmarSeat.Hubs;
using UmarSeat.Models;

namespace UmarSeat.Controllers
{
    public class EmployeesController : EntitySetControllerWithHub<EmployeeHub>
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [Queryable(PageSize = 2)]
        public override IQueryable<Employee> Get()
        {
            return db.Employees.AsQueryable();
        }
        protected override Employee GetEntityByKey(int key)
        {
            return db.Employees.Find(key);
        }

        public override HttpResponseMessage Patch(int key, Delta<Employee> patch)
        {
            var employeetoPach = GetEntityByKey(key);
            if(patch != null)
            {
                patch.Patch(employeetoPach);
                db.Entry(employeetoPach).State = EntityState.Modified;
                db.SaveChanges();
                var changeProperty = patch.GetChangedPropertyNames().ToList();
                foreach(var ch in changeProperty)
                {
                    object changePropertyValue;
                    patch.TryGetPropertyValue(ch, out changePropertyValue);
                    Hub.Clients.All.updateEmployee(employeetoPach.Id, ch, changePropertyValue);
                }
                
            
            }
            var resp = Request.CreateResponse<Employee>(
              HttpStatusCode.OK,
             employeetoPach
          );
            return resp;
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