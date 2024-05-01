namespace devshop.api.Features.Timer.Services;

public interface ICounterService
{
    int StartValue { get; }
    Task CountdownDelay(CancellationToken cancellationToken);
}
