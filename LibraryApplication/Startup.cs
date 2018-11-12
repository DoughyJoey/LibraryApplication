using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LibraryApplication.Startup))]
namespace LibraryApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
