namespace PharmacyOrderingWebsite.Helpers
{
    public class FileUploadHelper
    {
        public async Task<string> UploadAsync(IFormFile file)
        {
            try
            {
                var folderPath = "Uploads";

                //CREATE FOLDER if not exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                Console.WriteLine($"[FILE] Uploaded: {path}");

                return path;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FILE ERROR] {ex.Message}");
                throw;
            }
        }
    }
}