using devshop.api.Commons.Contracts;

namespace devshop.api.Commons;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}