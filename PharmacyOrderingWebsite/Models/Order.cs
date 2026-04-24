using PharmacyOrderingWebsite.Enums;

namespace PharmacyOrderingWebsite.Models
{
    // Order.cs
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Placed;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<OrderItem> Items { get; set; }
    }
}