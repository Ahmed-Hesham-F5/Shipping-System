namespace ShippingSystem.Interfaces
{
    public interface ITimeStamped
    {
        DateTime CreatedAt { get; }
        DateTime UpdatedAt { get; }
    }
}
