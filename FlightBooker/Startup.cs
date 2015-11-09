using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FlightBooker.Startup))]
namespace FlightBooker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
