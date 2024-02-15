namespace devshop.api.Commons.Contracts;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}