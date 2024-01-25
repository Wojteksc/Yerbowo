namespace Yerbowo.Domain.Primitives;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsRemoved { get; set; }
}