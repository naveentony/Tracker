using Microsoft.AspNetCore.Builder;

namespace Tracker.Application.Abstractions
{
    public interface IRegistrar
    {
        void RegisterServices(WebApplicationBuilder builder);
    }
}
