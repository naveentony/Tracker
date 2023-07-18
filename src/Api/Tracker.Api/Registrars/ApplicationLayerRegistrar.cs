
using System.Reflection;
using Tracker.Application.Abstractions;
using Tracker.Application.Services;
using Tracker.Domain.Provider;
using Tracker.Features.Account.Clients;
using Tracker.Features.Account.Identity;
using Tracker.Features.Alerts;
using Tracker.Features.Device.DeviceTypes;
using Tracker.Features.Manage.AssignVehicles;

namespace Tracker.Api.Registrars
{
    public class ApplicationLayerRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddScoped<IdentityService>();
            builder.Services.AddScoped<AlertsService>();
            builder.Services.AddScoped<AssignVehicleService>();
            builder.Services.AddScoped<ClientService>();
            builder.Services.AddScoped<CollectionProvider>();
            builder.Services.AddScoped<DeviceTypesService>();
            builder.Services.AddScoped<UserService>();
            
        }
    }
}
