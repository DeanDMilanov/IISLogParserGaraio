using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IISLogFileParserGaraio.Startup))]
namespace IISLogFileParserGaraio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
