namespace PharmacyOrderingWebsite.Models
{
    // OrderItem.cs
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}