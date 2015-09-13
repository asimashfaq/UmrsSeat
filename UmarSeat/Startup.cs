using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UmarSeat.Startup))]
namespace UmarSeat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
