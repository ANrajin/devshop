using devshop.api.Configs;
using devshop.api.Cores.Contracts;
using devshop.api.Features.Auths;
using devshop.api.Features.Books;
using devshop.api.Features.Courses;

var builder = WebApplication.CreateBuilder(args);

//Register Application Services
builder.Services.AddControllers();
builder.Services.AddServices(builder.Configuration,
    typeof(IServiceInstaller).Assembly);

string allowedOrigins = "_allowedOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins, policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

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

app.UseHttpsRedirection();

app.UseCors(allowedOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapAuthEndPoints();

app.MapGroup("/books")
    .MapBooksApi()
    .WithTags("Books");

app.MapGroup("/courses")
    .MapCoursesEndPoint()
    .WithTags("Courses");

app.Run();
