using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using UmarSeat.Models;

namespace UmarSeat.Hubs
{
    [HubName("booking")]
    public class BookingHub : Hub
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static ConcurrentDictionary<string, List<string>> _mapping = new ConcurrentDictionary<string, List<string>>();

        public override System.Threading.Tasks.Task OnConnected()
        {
            _mapping.TryAdd(Context.ConnectionId, new List<string>());
            return base.OnConnected();
        }
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            List<string> ids = new List<string>();
            foreach (var id in _mapping[Context.ConnectionId])
            {
                var seatToLock = db.SeatConfirmation.Where(x=> x.pnrNumber == id).FirstOrDefault();
                if (seatToLock.Locked == true)
                {
                    seatToLock.Locked = false;
                    db.Entry(seatToLock).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ids.Add(id);


                }
            }
            var list = new List<string>();
            _mapping.TryRemove(Context.ConnectionId, out list);
            foreach (string id in ids)
            {
                Clients.All.unlockEmployee(id);
            }
            ids = null;
            return base.OnDisconnected(stopCalled);
        }

        public void Lock(string id)
        {
            var seatToLock = db.SeatConfirmation.Where(x => x.pnrNumber == id).FirstOrDefault();
            if (seatToLock.Locked == false)
            {
                seatToLock.Locked = true;
                db.Entry(seatToLock).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Clients.Others.lockEmployee(id);
                _mapping[Context.ConnectionId].Add(id);

            }
            else
            {

                Clients.Client(Context.ConnectionId).lockFail(id);
            }
        }
        public void unLock(string id)
        {
            var seatToLock = db.SeatConfirmation.Where(x=> x.pnrNumber == id).FirstOrDefault();
            if (seatToLock.Locked == true)
            {
                seatToLock.Locked = false;
                db.Entry(seatToLock).State = System.Data.Entity.EntityState.Modified;
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