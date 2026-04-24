using Microsoft.AspNetCore.Identity;
using PharmacyOrderingWebsite.Helpers;
using PharmacyOrderingWebsite.Models;

namespace PharmacyOrderingWebsite.Data;

public class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var hasher = new PasswordHasher();

            context.Users.AddRange(
                new User
                {
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    PasswordHash = hasher.Hash("admin123"),
                    Role = "Admin"
                },
                new User
                {
                    Name = "User",
                    Email = "user@gmail.com",
                    PasswordHash = hasher.Hash("user123"),
                    Role = "User"
                }
            );

            context.SaveChanges();
        }
    }
}