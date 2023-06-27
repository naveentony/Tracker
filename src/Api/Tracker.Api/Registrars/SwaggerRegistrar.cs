using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Tracker.Application.Abstractions;

namespace Tracker.Api.Registrars
{
    public class SwaggerRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(SwaggerOptions());;


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
