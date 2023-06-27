using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Application.Abstractions;
using Tracker.Application.Services;
using Tracker.Domain.Dtos;
using Tracker.Domain.Provider;
using Tracker.Domain.Settings;

namespace Tracker.Application.Registrars
{
    public class ApplicationLayerRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddScoped<IdentityService>();
        }

    }
    public class DbRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<TrackerSettings, TrackerSettings>();
            builder.Services.AddScoped<ICollectionProvider, CollectionProvider>();
            builder.Services.Configure<TrackerSettings>(builder.Configuration.GetSection(nameof(TrackerSettings)));


        }
    }
    public class IdentityRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var cs = builder.Configuration.GetRequiredSection("TrackerSettings").Get<TrackerSettings>();
            builder.Services.AddIdentity<UsersDto, MongoRoleDto>()
                  .AddMongoDbStores<UsersDto, MongoRoleDto, Guid>
                  (
                      cs.ConnectionString, cs.DatabaseName
                  );
            var jwtSettings = new JwtSettings();
            builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);

            var jwtSection = builder.Configuration.GetSection(nameof(JwtSettings));
            builder.Services.Configure<JwtSettings>(jwtSection);

            builder.Services
                .AddAuthentication(a =>
                {
                    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt =>
                {
                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudiences = jwtSettings.Audiences,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                    jwt.Audience = jwtSettings.Audiences[0];
                    jwt.ClaimsIssuer = jwtSettings.Issuer;
                });

            builder.Services.AddAuthorization();


        }
    }
    public class SwaggerRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(SwaggerOptions()); ;


        }
        private Action<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions> SwaggerOptions()
        {
            var scheme = GetJwtSecurityScheme();
            return option =>
            {
                option.SwaggerDoc("v1", CreateVersionInfo());

                option.AddSecurityDefinition(scheme.Reference.Id, scheme);
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {scheme, new string[0]}
                });
                option.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            };
        }
        private OpenApiInfo CreateVersionInfo(/*ApiVersionDescription description=null*/)
        {
            var info = new OpenApiInfo
            {
                Title = "CwkSocial",
                Version = "v1"// description.ApiVersion.ToString()
            };
            return info;
        }
        private OpenApiSecurityScheme GetJwtSecurityScheme()
        {
            return new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Provide a JWT Bearer",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
        }
    }
}
