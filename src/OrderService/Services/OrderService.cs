using Grpc.Core;
using Transporter;

namespace OrderService.Services;

public class OrderService : Order.OrderBase
{
    private readonly ILogger<OrderService> _logger;
    private readonly RabbitMQMessagePublisher _messagePublisherForCreation;
    private readonly RabbitMQMessagePublisher _messagePublisherForUpdation;

    public OrderService(ILogger<OrderService> logger)
    {
        _logger = logger;
        _messagePublisherForCreation =
            new RabbitMQMessagePublisher("localhost", "guest", "guest", "orderCreation", "fanout", "creation");
        _messagePublisherForUpdation =
            new RabbitMQMessagePublisher("localhost", "guest", "guest", "orderUpdation", "topic", "updation");
    }

    public override Task<OrderResultResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Create order request received");
        try
        {
            if (string.IsNullOrWhiteSpace(request.BuyerId))
            {
                context.Status = new Status(StatusCode.InvalidArgument, "Buyer id is not valid");
                return Task.FromResult(new OrderResultResponse());
            }

            if (request.Items.Count == 0)
            {
                context.Status = new Status(StatusCode.InvalidArgument, "Items is not present");
                return Task.FromResult(new OrderResultResponse());
            }
            //logic to create order
           
            var orderId = Random.Shared.Next();
            _messagePublisherForCreation.PublishMessage($"Order created - {orderId}", "creation");
            context.Status = new Status(StatusCode.OK, $"Order created successfully");
            return Task.FromResult(new OrderResultResponse()
            {
                OrderId = orderId
            });
        }
        catch (Exception ex)
        {
            context.Status = new Status(StatusCode.Internal, "Some error occurred while creating order");
            throw;
        }
        finally
        {
            _logger.LogInformation("Create order request completed");
        }
    }

    public override Task<OrderResultResponse> UpdateOrder(UpdateOrderRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Update order request received");
        try
        {
            if (request.OrderId == 0)
            {
                context.Status = new Status(StatusCode.InvalidArgument, "Order id is not valid");
                return Task.FromResult(new OrderResultResponse());
            }

            if (request.Items.Count == 0)
            {
                context.Status = new Status(StatusCode.InvalidArgument, "Items is not present");
                return Task.FromResult(new OrderResultResponse());
            }
            //logic to update order
            //todo raise event in rabbit mq
            _messagePublisherForUpdation.PublishMessage($"Order updated - {request.OrderId}", "updation");
            context.Status = new Status(StatusCode.OK, $"Order updated successfully");
            return Task.FromResult(new OrderResultResponse()
            {
                OrderId = request.OrderId
            });
        }
        catch (Exception ex)
        {
            context.Status = new Status(StatusCode.Internal, "Some error occurred while updating order");
            throw;
        }
        finally
        {
            _logger.LogInformation("Update order request completed");
        }
    }
}