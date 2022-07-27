using LegionCore.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace LegionCore.Infrastructure.Seeders
{
    public class ApplicationRoleSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ApplicationRoleSeeder(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            using StreamReader r = new StreamReader("./SeedData/ApplicationRoles.json");
            string json = await r.ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<List<ApplicationRole>>(json);
            foreach (var item in data)
            {
                if (!await _roleManager.RoleExistsAsync(item.Name))
                {
                    await _roleManager.CreateAsync(item);
                }
            }
        }
    }
}