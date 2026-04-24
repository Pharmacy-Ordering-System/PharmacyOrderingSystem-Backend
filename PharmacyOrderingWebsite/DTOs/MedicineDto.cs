namespace PharmacyOrderingWebsite.DTOs
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }
    }
}