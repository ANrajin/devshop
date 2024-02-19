using System.Text;
using devshop.api.Auths.Entities;
using devshop.api.Contexts;
using devshop.api.Cores.Adapters;
using devshop.api.Cores.UnitOfWorks;
using devshop.api.Cores.Utilities;
using devshop.api.Features.Auths;
using devshop.api.Features.Auths.JWT;
using devshop.api.Features.Auths.Securities;
using devshop.api.Features.Auths.Services;
using devshop.api.Features.Books;
using devshop.api.Features.Books.Repositories;
using devshop.api.Features.Books.Requests;
using devshop.api.Features.Books.Services;
using devshop.api.Features.Samples;
using devshop.api.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

const string connectionStrName = "DevShopDb";
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString(connectionStrName);

//Dependencies
builder.Services.AddScoped<IApplicationDbContext>(provider => 
    provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<IUnitOfWorks, UnitOfWorks>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddScoped<IUserManagerAdapter, UserManagerAdapter>();
builder.Services.AddScoped<IUserManagerService, UserManagerService>();
builder.Services.AddScoped<ISigninManagerAdapter, SigninManagerAdapter>();
builder.Services.AddScoped<ISigninManagerService, SigninManagerService>();

builder.Services.AddScoped<BooksRequestHandler>();

//Entity Interceptors
builder.Services.AddSingleton<EntityInterceptor>();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var entityInterceptor = sp.GetService<EntityInterceptor>()!;
    
    options.UseSqlServer(connectionString)
        .AddInterceptors(entityInterceptor);
});

//Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddControllers();

builder.Services.AddSingleton<IAuthorizationHandler, BooksRequirementHandler>();

//Auth
builder.Services
    .AddIdentity<ApplicationUser, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<UserManagerAdapter>()
    .AddSignInManager<SigninManagerAdapter>()
    .AddRoleManager<RoleManagerAdapter>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(DevshopPolicies.BooksPolicy, policy =>
    {
        policy.Requirements.Add(new BooksRequirement());
    });
});

builder.Services.Configure<IdentityOptions>(options =>
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
JwtSettings jwtSettings = new();
builder.Configuration.Bind(JwtSettings.SectionName, jwtSettings);
builder.Services.AddSingleton(Options.Create(jwtSettings));

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

builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = tokenValidationParameters;
});

//Swagger
builder.Services.AddSwaggerGen(options =>
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => { options.RouteTemplate = "swagger/{documentName}/swagger.json"; });
        
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = string.Empty;
        options.SwaggerEndpoint("/swagger/devshop-api-v1/swagger.json", "api-v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndPoints();
//app.MapControllers();
app.MapBookEndPoints();
app.MapSamplesEndPoints();

app.Run();
