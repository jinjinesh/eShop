namespace ProductApi.Dto;

public class UpdateOrderRequestDto
{
    public int OrderId { get; set; }

    public List<OrderItemDto> Items { get; set; }
}