using devshop.api.Features.Timer.Services;
using Microsoft.Net.Http.Headers;

namespace devshop.api.Features.Timer
{
    public static class TimerEndPoints
    {
        public static void MapTimerEndPoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/counts", async Task (HttpContext context, 
                ICounterService counterService, 
                CancellationToken cancellationToken) =>
            {
                context.Response.Headers.Append(HeaderNames.ContentType, "text/event-stream");
                var count = counterService.StartValue;

                while(count >= 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await counterService.CountdownDelay(cancellationToken);

                    await context.Response.WriteAsync($"data: {count}\n\n", cancellationToken);
                    await context.Response.Body.FlushAsync(cancellationToken);

                    count--;
                }

                await context.Response.CompleteAsync();
            });
        }
    }
}
