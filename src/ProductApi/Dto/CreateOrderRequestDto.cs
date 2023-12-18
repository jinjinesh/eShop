namespace ProductApi.Dto;

public class CreateOrderRequestDto
{
    public string BuyerId { get; set; }

    public List<OrderItemDto> Items { get; set; }
}