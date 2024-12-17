using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GestaoEstudantil.Startup))]
namespace GestaoEstudantil
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
