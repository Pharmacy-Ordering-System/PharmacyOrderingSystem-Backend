namespace PharmacyOrderingWebsite.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public Inventory? Inventory { get; set; }
    }
}