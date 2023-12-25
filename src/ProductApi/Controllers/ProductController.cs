using Google.Protobuf.Collections;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderService;
using ProductApi.Dto;

namespace ProductApi.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly Order.OrderClient _orderClient;

    public ProductController(IConfiguration configuration)
    {
        var orderServiceUrl = configuration["orderServiceUrl"];
        var channel = GrpcChannel.ForAddress(orderServiceUrl);
        _orderClient = new Order.OrderClient(channel);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAllProducts()
    {
        return GetProducts();
    }

    [HttpGet("{productId}")]
    public ActionResult<Product> GetProduct(int productId)
    {
        var product = GetProducts().FirstOrDefault(x => x.ProductId == productId);
        if (product is null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost("order")]
    public async Task<ActionResult> CreateOrder([FromBody] CreateOrderRequestDto request)
    {
        var products = GetProducts().Where(x => request.Items.Any(i => i.ProductId == x.ProductId));
        var createOrderRequest = new CreateOrderRequest
        {
            BuyerId = request.BuyerId
        };
        createOrderRequest.Items.AddRange(products.Select(x => new ItemDetail
        {
            Id = x.Id,
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            Quantity = request.Items.FirstOrDefault(i => i.ProductId == x.ProductId)!.Quantity,
            UnitPrice = x.UnitPrice
        }));
        var result = await _orderClient.CreateOrderAsync(createOrderRequest);
        return result.OrderId != 0 ? Ok(result.OrderId) : BadRequest();
    }

    [HttpPut("order")]
    public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderRequestDto request)
    {
        var products = GetProducts().Where(x => request.Items.Any(i => i.ProductId == x.ProductId));
        var updateOrderRequest = new UpdateOrderRequest
        {
            OrderId = request.OrderId
        };
        updateOrderRequest.Items.AddRange(products.Select(x => new ItemDetail
        {
            Id = x.Id,
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            Quantity = request.Items.FirstOrDefault(i => i.ProductId == x.ProductId)!.Quantity,
            UnitPrice = x.UnitPrice
        }));
        var result = await _orderClient.UpdateOrderAsync(updateOrderRequest);
        return result.OrderId != 0 ? Ok(result.OrderId) : BadRequest();
    }

    private List<Product> GetProducts()
    {
        return new List<Product>
        {
            new() { Id = 1, ProductId = 1001, ProductName = "Product 1", UnitPrice = 10.99, Quantity = 5 },
            new() { Id = 2, ProductId = 1002, ProductName = "Product 2", UnitPrice = 15.99, Quantity = 3 },
            new() { Id = 3, ProductId = 1003, ProductName = "Product 3", UnitPrice = 7.99, Quantity = 10 },
            new() { Id = 4, ProductId = 1004, ProductName = "Product 4", UnitPrice = 12.99, Quantity = 8 },
            new() { Id = 5, ProductId = 1005, ProductName = "Product 5", UnitPrice = 9.99, Quantity = 15 },
            new() { Id = 6, ProductId = 1006, ProductName = "Product 6", UnitPrice = 6.99, Quantity = 20 },
            new() { Id = 7, ProductId = 1007, ProductName = "Product 7", UnitPrice = 10.99, Quantity = 5 },
            new() { Id = 8, ProductId = 1008, ProductName = "Product 8", UnitPrice = 15.99, Quantity = 3 },
            new() { Id = 9, ProductId = 1009, ProductName = "Product 9", UnitPrice = 7.99, Quantity = 10 },
            new() { Id = 10, ProductId = 1010, ProductName = "Product 10", UnitPrice = 12.99, Quantity = 8 },
            new() { Id = 11, ProductId = 1011, ProductName = "Product 11", UnitPrice = 9.99, Quantity = 15 },
            new() { Id = 12, ProductId = 1012, ProductName = "Product 12", UnitPrice = 6.99, Quantity = 20 },
        };
    }
}