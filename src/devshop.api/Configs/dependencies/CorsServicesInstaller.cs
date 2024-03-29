using devshop.api.Cores.Contracts;

namespace devshop.api.Configs.dependencies
{
    public class CorsServicesInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            var corsName = configuration.GetSection("CORS:Name").Value!;

            services.AddCors(options =>
            {
                options.AddPolicy(name: corsName, policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }
}
