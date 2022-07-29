using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using LegionCore.Core.Identity;
using LegionCore.Core.Models.Identity;
using LegionCore.Infrastructure.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LegionCore.Infrastructure.Helpers.Seeders
{
    public class ApplicationUserSeeder : ISeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public int SeedPriority => 100;
        private CsvConfiguration csv_config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        };

        public ApplicationUserSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            Console.WriteLine("Loading users from seeder...");
 
            //     await using var stream = File.Create("../LegionCore.Infrastructure/Helpers/Seeders/SeedData/ApplicationRoles.json");
            //     var data = await JsonSerializer.DeserializeAsync<ApplicationRole[]>(stream);

            try
            {
                using var stream =
                    new StreamReader("../LegionCore.Infrastructure/Helpers/Seeders/SeedData/ApplicationUsers.csv");

                var csv_data = new CsvReader(stream, csv_config);
                
                if (csv_data != null)
                    foreach (var item in csv_data.GetRecords<ApplicationUser>())
                    {
                        Console.WriteLine("Seeding user: " + item.Email + "...");
                        if (await _userManager.FindByEmailAsync(item.Email) != null) continue;
                        Console.WriteLine("User does not exist, creating...");
                        await _userManager.CreateAsync(item);
                    }
                
                Console.WriteLine("Users seeded.");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error seeding users: " + e.Message);
            }
        }
    }
}