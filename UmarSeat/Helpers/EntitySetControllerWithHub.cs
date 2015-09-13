using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.OData;
using System.Web.Mvc;
using UmarSeat.Models;

namespace UmarSeat.Helpers
{
    public class EntitySetControllerWithHub<THub>: EntitySetController<Employee,int>
    where THub: IHub {
        Lazy<IHubContext> hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<THub>()
            );

        protected IHubContext Hub
        {
            get { return hub.Value; }
        }
    }

    public class ControllerWithHub<Thub> : Controller where Thub : IHub
    {
        Lazy<IHubContext> hub = new Lazy<IHubContext>(
            ()=> GlobalHost.ConnectionManager.GetHubContext<Thub>()
            );
        protected IHubContext Hub
        {
            get { return hub.Value; }
        }
    }
    
}