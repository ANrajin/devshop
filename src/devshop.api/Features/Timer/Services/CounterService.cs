
namespace devshop.api.Features.Timer.Services;

public class CounterService : ICounterService
{
    private const int StartValueCounter = 30;
    private const int MillisecondsDelay = 1000;

    public int StartValue
    {
        get => StartValueCounter;
    }

    public async Task CountdownDelay(CancellationToken cancellationToken)
    {
        await Task.Delay(MillisecondsDelay, cancellationToken);
    }
}
