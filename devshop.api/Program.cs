using devshop.api.Configs;
using devshop.api.Cores.Contracts;
using devshop.api.Features.Auths;
using devshop.api.Features.Books;

var builder = WebApplication.CreateBuilder(args);

//Register Application Services
builder.Services.AddControllers();
builder.Services.AddServices(builder.Configuration,
    typeof(IServiceInstaller).Assembly);

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapAuthEndPoints();
app.MapBookEndPoints();

app.Run();
