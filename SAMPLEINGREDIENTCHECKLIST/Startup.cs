using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SAMPLEINGREDIENTCHECKLIST.Startup))]
namespace SAMPLEINGREDIENTCHECKLIST
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
