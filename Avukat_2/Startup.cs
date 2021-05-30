using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(Avukat_2.Startup))]
namespace Avukat_2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}