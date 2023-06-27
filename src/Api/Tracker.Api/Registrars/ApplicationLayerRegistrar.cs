
using System.Reflection;
using Tracker.Application.Abstractions;
using Tracker.Application.Services;

namespace Tracker.Api.Registrars
{
    public class ApplicationLayerRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddScoped<IdentityService>();

            

        }
        
    }
}
