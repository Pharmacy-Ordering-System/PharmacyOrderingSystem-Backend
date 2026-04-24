using Microsoft.EntityFrameworkCore;
using PharmacyOrderingWebsite.Enums;
using PharmacyOrderingWebsite.Helpers;
using PharmacyOrderingWebsite.Models;
namespace PharmacyOrderingWebsite.Services
{
    public class PrescriptionService
    {
        private readonly AppDbContext _context;
        private readonly FileUploadHelper _fileHelper;

        public PrescriptionService(AppDbContext context, FileUploadHelper fileHelper)
        {
            _context = context;
            _fileHelper = fileHelper;
        }

        public async Task<Prescription> Upload(int userId, IFormFile file)
        {
            try
            {
                Console.WriteLine("[PRESCRIPTION] Upload started");

                var path = await _fileHelper.UploadAsync(file);

                var prescription = new Prescription
                {
                    UserId = userId,
                    FilePath = path,
                    Status = PrescriptionStatus.Pending
                };

                _context.Prescriptions.Add(prescription);
                await _context.SaveChangesAsync();

                Console.WriteLine("[PRESCRIPTION] Uploaded successfully");

                return prescription;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PRESCRIPTION ERROR] {ex.Message}");
                throw;
            }
        }

        public async Task<Prescription> Validate(int id, PrescriptionStatus status)
        {
            try
            {
                var prescription = await _context.Prescriptions.FindAsync(id);

                if (prescription == null)
                    throw new Exception("Prescription not found");

                prescription.Status = status;
                await _context.SaveChangesAsync();

                Console.WriteLine("[PRESCRIPTION] Validation updated");

                return prescription;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PRESCRIPTION ERROR] {ex.Message}");
                throw;
            }
        }

        public async Task<List<Prescription>> GetAll()
        {
            return await _context.Prescriptions.ToListAsync();
        }
    }
}