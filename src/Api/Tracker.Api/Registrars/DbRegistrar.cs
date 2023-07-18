using Tracker.Application.Abstractions;
using Tracker.Domain.Provider;
using Tracker.Domain.Settings;

namespace Tracker.Api.Registrars
{
    public class DbRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<TrackerSettings, TrackerSettings>();
            builder.Services.AddScoped<ICollectionProvider, CollectionProvider>();
            builder.Services.Configure<TrackerSettings>(builder.Configuration.GetSection(nameof(TrackerSettings)));
        }
    }
}
