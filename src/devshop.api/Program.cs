using devshop.api.Configs;
using devshop.api.Cores.Contracts;
using devshop.api.Features.Auths;
using devshop.api.Features.Books;
using devshop.api.Features.Courses;
using devshop.api.Features.Timer;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var corsName = builder.Configuration.GetSection("CORS:Name").Value!;

//Register Application Services
builder.Services.AddControllers();
builder.Services.AddServices(builder.Configuration,
    typeof(IServiceInstaller).Assembly);

builder.Host.UseSerilog((ctx, config) =>
{
    config.MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration);
});

try
{
    var app = builder.Build();

    Log.Information("Application built successfully.");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "swagger/{documentName}/swagger.json";
        });

        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = string.Empty;
            options.SwaggerEndpoint("/swagger/devshop-api-v1/swagger.json", "api-v1");
        });
    }

    //Register Framework Middlewares
    app.UseRateLimiter();
    app.UseHttpsRedirection();
    app.UseCors(corsName);
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    //Register Minimal Api Endpoints
    app.MapAuthEndPoints();

    app.MapGroup("/books")
        .MapBooksApi()
        .WithTags("Books")
        .RequireRateLimiting("fixed");

    app.MapGroup("/courses")
        .MapCoursesEndPoint()
        .WithTags("Courses");

    app.MapTimerEndPoints();

    app.Run();
}
catch(Exception ex)
{
    Log.Error("The following {Exception} was thrown during application startup", ex);
}
finally
{
    await Log.CloseAndFlushAsync();
}
