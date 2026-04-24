
using PharmacyOrderingWebsite.Enums;

namespace PharmacyOrderingWebsite.Models
{
    // Prescription.cs
    public class Prescription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FilePath { get; set; }
        public PrescriptionStatus Status { get; set; } = PrescriptionStatus.Pending;
    }
}