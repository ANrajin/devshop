namespace devshop.api.Commons;

public abstract class BaseEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}