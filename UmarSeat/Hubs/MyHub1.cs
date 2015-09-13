using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using UmarSeat.Models;
using System.Collections.Concurrent;

namespace UmarSeat.Hubs
{
    [HubName("employee")]
    public class EmployeeHub : Hub
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static ConcurrentDictionary<string, List<int>> _mapping = new ConcurrentDictionary<string, List<int>>();

        public override System.Threading.Tasks.Task OnConnected()
        {
            _mapping.TryAdd(Context.ConnectionId, new List<int>());
            return base.OnConnected();
        }
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            List<int> ids = new List<int>();
            foreach (var id  in _mapping[Context.ConnectionId])
            {
                var employeeToLock = db.Employees.Find(id);
                if (employeeToLock.Locked == true)
                {
                    employeeToLock.Locked = false;
                    db.Entry(employeeToLock).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ids.Add(id);
                 
                   
                }
            }
            var list = new List<int>();
            _mapping.TryRemove(Context.ConnectionId, out list);
            foreach(int id in ids)
            {
                Clients.All.unlockEmployee(id);
            }
            ids = null;
            return base.OnDisconnected(stopCalled);
        }

        public void Lock(int id)
        {
            var employeeToLock = db.Employees.Find(id);
            if(employeeToLock.Locked == false)
            {
                employeeToLock.Locked = true;
                db.Entry(employeeToLock).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Clients.Others.lockEmployee(id);
                _mapping[Context.ConnectionId].Add(id);

            }
            else
            {

                Clients.Client(Context.ConnectionId).lockFail(id);
            }
        }
        public void unLock(int id)
        {
            var employeeToLock = db.Employees.Find(id);
            if (employeeToLock.Locked == true)
            {
                employeeToLock.Locked = false;
                db.Entry(employeeToLock).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Clients.Others.unlockEmployee(id);
                _mapping[Context.ConnectionId].Remove(id);
            }
            else
            {

                Clients.Client(Context.ConnectionId).lockFail(id);
            }
        }
    }
}