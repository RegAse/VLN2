using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VLN2.Startup))]
namespace VLN2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Map SignalR
            app.MapSignalR();
        }
    }
}
