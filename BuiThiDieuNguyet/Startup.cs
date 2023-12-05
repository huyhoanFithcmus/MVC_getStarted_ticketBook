using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BuiThiDieuNguyet.Startup))]
namespace BuiThiDieuNguyet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
