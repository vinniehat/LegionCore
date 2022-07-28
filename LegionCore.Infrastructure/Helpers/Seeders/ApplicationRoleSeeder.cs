using LegionCore.Core.Models.Identity;
using LegionCore.Infrastructure.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace LegionCore.Infrastructure.Helpers.Seeders
{
    public class ApplicationRoleSeeder : ISeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public int SeedPriority => 100;

        public ApplicationRoleSeeder(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            Console.WriteLine("Loading roles from seeder...");
            using StreamReader r = new StreamReader("./SeedData/ApplicationRoles.json");
            string json = await r.ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<List<ApplicationRole>>(json);
            if (data != null)
                foreach (var item in data)
                {
                    Console.WriteLine("Seeding role: " + item.Name + "...");
                    if (await _roleManager.RoleExistsAsync(item.Name)) continue;
                    Console.WriteLine("Role does not exist, creating...");
                    await _roleManager.CreateAsync(item);
                }

            Console.WriteLine("Roles seeded.");
        }
    }
}