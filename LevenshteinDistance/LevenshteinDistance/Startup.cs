using Microsoft.Owin;
using Owin;
using System.IdentityModel.Tokens.Jwt;

[assembly: OwinStartupAttribute(typeof(LevenshteinDistance.Startup))]
namespace LevenshteinDistance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
        }
    }
}
