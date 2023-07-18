using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tracker.Domain.Dtos;
using Tracker.Domain.Settings;

namespace Tracker.Application
{
    public static class ApplicationIdentity
    {
        public static IServiceCollection Identity(this WebApplicationBuilder builder)
        {
            var cs = builder.Configuration.GetRequiredSection("TrackerSettings").Get<TrackerSettings>();

            builder.Services.AddIdentity<UsersDto, MongoRoleDto>()
                    .AddMongoDbStores<UsersDto, MongoRoleDto, Guid>
                    (
                        cs.ConnectionString, cs.DatabaseName
                    );
            return builder.Services;
        }
    }
}
