using Kt12Profiles.Models;

namespace Kt12Profiles.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        if (!await db.Database.EnsureCreatedAsync()) return;
        db.Users.AddRange(
            new User
            {
                Name = "Даниил Денисенко", Email = "daniil@example.com",
                Profile = new UserProfile { Bio = "Студент группы 4п4.23 NET", City = "Москва" }
            },
            new User
            {
                Name = "Анна Иванова", Email = "anna@example.com",
                Profile = new UserProfile { Bio = "Изучаю Entity Framework Core", City = "Санкт-Петербург" }
            });
        await db.SaveChangesAsync();
    }
}
