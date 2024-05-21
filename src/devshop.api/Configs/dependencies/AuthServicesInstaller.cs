using devshop.api.Contexts;
using devshop.api.Cores.Adapters;
using devshop.api.Cores.Contracts;
using devshop.api.Features.Auths.Entities;
using devshop.api.Features.Auths.JWT;
using devshop.api.Features.Auths.Securities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace devshop.api.Configs.dependencies
{
    public class AuthServicesInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthorizationHandler, BooksRequirementHandler>();

            services
                .AddIdentity<ApplicationUser, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManagerAdapter>()
                .AddSignInManager<SigninManagerAdapter>()
                .AddRoleManager<RoleManagerAdapter>()
                .AddDefaultTokenProviders();

            services.AddAuthorizationBuilder()
                .AddPolicy(DevshopPolicies.BooksPolicy, policy =>
                {
                    policy.Requirements.Add(new BooksRequirement());
                });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            //JWT
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            JwtSettings jwtSettings = new();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);

            services.AddSingleton(Options.Create(jwtSettings));

            var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(key),
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthorization();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = ctx =>
                    {
                        ctx.Request.Cookies.TryGetValue("accessToken", out var accessToken);

                        if(!string.IsNullOrEmpty(accessToken))
                            ctx.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
