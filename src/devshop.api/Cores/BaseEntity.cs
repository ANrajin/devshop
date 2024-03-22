namespace devshop.api.Cores;

public abstract class BaseEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    public DateTime? CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}