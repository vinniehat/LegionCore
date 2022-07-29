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
    public class ApplicationUserRoleSeeder : ISeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public int SeedPriority => 100;
        private CsvConfiguration csv_config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        };

        public ApplicationUserRoleSeeder(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            Console.WriteLine("Loading user roles from seeder...");
 
            //     await using var stream = File.Create("../LegionCore.Infrastructure/Helpers/Seeders/SeedData/ApplicationRoles.json");
            //     var data = await JsonSerializer.DeserializeAsync<ApplicationRole[]>(stream);

            try
            {
                using var stream =
                    new StreamReader("../LegionCore.Infrastructure/Helpers/Seeders/SeedData/ApplicationUserRoles.csv");

                var csv_data = new CsvReader(stream, csv_config);
                
                if (csv_data != null)
                    foreach (var item in csv_data.GetRecords<ApplicationUserRole>())
                    {
                        Console.WriteLine($"Finding user with email {item.ApplicationUserEmail} and role {item.ApplicationRoleName}...");

                        var user = await _userManager.FindByEmailAsync(item.ApplicationUserEmail);
                        var role = await _roleManager.FindByNameAsync(item.ApplicationRoleName);
                        if (user == null)
                        {
                            Console.WriteLine("User does not exist, skipping...");
                            continue;
                        }
                        Console.WriteLine("User found, checking for role...");
                        if (role == null)
                        {
                            Console.WriteLine("Role does not exist, skipping...");
                            continue;
                        }
                        Console.WriteLine("Role found, checking for user role...");
                        if (_userManager.IsInRoleAsync(user, role.ToString()) == Task.FromResult(true))
                        {
                            Console.WriteLine("User is already in role, skipping...");
                            continue;
                        }

                        await _userManager.AddToRoleAsync(user, role.ToString());
                        Console.WriteLine("Seeding user role: " + item.ApplicationUserEmail + " complete...");
                    }
                
                Console.WriteLine("User Roles seeded.");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error seeding user roles: " + e.Message);
            }
        }
    }
}