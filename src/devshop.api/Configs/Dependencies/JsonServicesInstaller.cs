using devshop.api.Cores.Contracts;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace devshop.api.Configs.Dependencies;

public class JsonServicesInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.WriteIndented = false;
            options.SerializerOptions.Encoder = JavaScriptEncoder.Default;
            options.SerializerOptions.AllowTrailingCommas = true;
            options.SerializerOptions.MaxDepth = 3;
            options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        });
    }
}

//reference: https://code-maze.com/aspnetcore-set-global-default-json-serialization-options
