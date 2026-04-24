namespace PharmacyOrderingWebsite.DTOs
{
    public class OrderDto
    {
        //  public int UserId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}