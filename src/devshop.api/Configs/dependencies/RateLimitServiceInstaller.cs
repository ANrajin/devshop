using devshop.api.Cores.Contracts;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace devshop.api.Configs.dependencies
{
    public class RateLimitServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            RateLimitOptions rateLimitOptions = new();
            configuration.Bind(RateLimitOptions.SectionName, rateLimitOptions);

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.AddFixedWindowLimiter(policyName: "fixed", options =>
                {
                    options.PermitLimit = rateLimitOptions.PermitLimit;
                    options.QueueLimit = rateLimitOptions.QueueLimit;
                    options.Window = TimeSpan.FromSeconds(rateLimitOptions.TimeWindow);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
            });
        }
    }
}
