using Grpc.Core;
using GrpcGreeter.Models;
using GrpcGreeter.Product;
using static GrpcGreeter.Product.ProductService;

namespace GrpcGreeter.Services
{
    using Product = Models.Product;
    public class ProductService : ProductServiceBase
    {
        private static List<Product> _products = GetProducts();
        private readonly ILogger<ProductService> _logger;
        public ProductService(ILogger<ProductService> logger)
        {
            _logger = logger;
        }

        public override Task<GrpcGreeter.Product.StatusMessage> Addproduct(GrpcGreeter.Product.Product request, ServerCallContext context)
        {
            //var price = $"{request.Price.Units}.{request.Price.Nanos}";
            //_products.Add(new Product { Id = Guid.NewGuid(), Name = request.Name, Price = decimal.Parse(price)});
            _products.Add(new Product { Id = Guid.NewGuid(), Name = request.Name, Price = (decimal)request.Price });

            return Task.FromResult(CreateSuccessResponse(_products));
        }

        private static List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1", Price = (DecimalValue)12.34M },
                new Product { Id = Guid.NewGuid(), Name = "Product 2", Price = (DecimalValue)45.67M }
            };
        }

        private static Google.Protobuf.Collections.RepeatedField<GrpcGreeter.Product.Product> ProductListMapper(List<Product> products)
        {
            var productList = new Google.Protobuf.Collections.RepeatedField<GrpcGreeter.Product.Product>();
            products.ForEach(p => productList.Add(new GrpcGreeter.Product.Product 
            { 
                Id = p.Id.ToString(), 
                Name = p.Name,
                Price = (DecimalValue)p.Price
            }));

            return productList;
        }

        private static StatusMessage CreateSuccessResponse(List<Product> products)
        {
            var statusMessage = new StatusMessage
            {
                Success = ReadingStatus.Success,
                Message = $"Added new product. Total count: {products.Count}"
            };

            statusMessage.ProductList.AddRange(ProductListMapper(products));
            return statusMessage;
        }
    }
}
