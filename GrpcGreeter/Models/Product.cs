namespace GrpcGreeter.Models
{
    public class Product
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
    }
}
