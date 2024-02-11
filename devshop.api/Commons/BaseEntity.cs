namespace devshop.api.Commons;

public abstract class BaseEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    public DateTime? CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}