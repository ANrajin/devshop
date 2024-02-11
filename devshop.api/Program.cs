using devshop.api.Books;
using devshop.api.Contexts;
using devshop.api.Interceptor;
using Microsoft.EntityFrameworkCore;

const string connectionStrName = "DevShopDb";
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString(connectionStrName);

//Dependencies
builder.Services.AddScoped<IApplicationDbContext>(provider => 
    provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<IBooksService, BooksService>();

//Entity Interceptor
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapBookEndPoints();

app.Run();
