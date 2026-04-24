namespace PharmacyOrderingWebsite.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public Medicine? Medicine { get; set; }

        public int Stock { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}