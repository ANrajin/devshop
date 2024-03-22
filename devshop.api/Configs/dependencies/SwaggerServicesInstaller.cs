using devshop.api.Cores.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace devshop.api.Configs.dependencies
{
    public class SwaggerServicesInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("devshop-api-v1", new OpenApiInfo
                {
                    Title = "Devshop API",
                    Version = "1",
                    Description = "This is a sample api application",
                    Contact = new OpenApiContact
                    {
                        Name = "AN Rajin",
                        Email = "an.rajin@gmail.com",
                        Url = new Uri("https://anrajin.github.io/me")
                    }
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authentication",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    Description = "Please provide the bearer token to access protected end points.",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, Array.Empty<string>()}
                });
            });
        }
    }
}
