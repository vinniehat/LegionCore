using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using LegionCore.Core.Models.Identity;
using LegionCore.Infrastructure.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LegionCore.Infrastructure.Helpers.Seeders
{
    public class ApplicationRoleSeeder : ISeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public int SeedPriority => 100;
        private CsvConfiguration csv_config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        };

        public ApplicationRoleSeeder(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            Console.WriteLine("Loading roles from seeder...");
 
            //     await using var stream = File.Create("../LegionCore.Infrastructure/Helpers/Seeders/SeedData/ApplicationRoles.json");
            //     var data = await JsonSerializer.DeserializeAsync<ApplicationRole[]>(stream);

            try
            {
                using var stream =
                    new StreamReader("../LegionCore.Infrastructure/Helpers/Seeders/SeedData/ApplicationRoles.csv");

                var csv_data = new CsvReader(stream, csv_config);
                
                if (csv_data != null)
                    foreach (var item in csv_data.GetRecords<ApplicationRole>())
                    {
                        Console.WriteLine("Seeding role: " + item.Name + "...");
                        if (await _roleManager.RoleExistsAsync(item.Name)) continue;
                        Console.WriteLine("Role does not exist, creating...");
                        await _roleManager.CreateAsync(item);
                    }
                
                Console.WriteLine("Roles seeded.");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error seeding roles: " + e.Message);
            }
        }
    }
}