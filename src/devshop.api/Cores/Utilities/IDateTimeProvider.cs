namespace devshop.api.Cores.Utilities;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}