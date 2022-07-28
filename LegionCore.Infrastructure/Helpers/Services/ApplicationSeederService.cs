using LegionCore.Infrastructure.Helpers.Interfaces;
using Microsoft.Extensions.Logging;

namespace LegionCore.Infrastructure.Helpers.Services;
public class ApplicationSeederService : IService
{
    private readonly ILogger _logger;
    private readonly UtilityService _utils;
    private readonly IServiceProvider _serviceProvider;

    public int SeedPriority => Int32.MaxValue;

    public ApplicationSeederService(IServiceProvider serviceProvider, ILogger<ApplicationSeederService> logger,
        UtilityService utils)
    {
        _logger = logger;
        _utils = utils;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Perform the seeding function by calling any other registered seeders.
    /// </summary>
    /// <returns></returns>
    public async Task SeedAsync()
    {
	    var seeders = new List<ISeeder>();

        // The service registry contains a list of all the automatically-registered
        // services using the Serviced service.  This code creates a list of all of
        // the other registered ISeeders in the assemblies specified in Startup.
        // (see services.AddServiced line)
        // List<Type> seederTypes = _utils.FilterServiceTypes<ISeeder>()
        //     .Where(t => t.Name != GetType().Name)
        //     .ToList();

        List<Type> seederTypes = _utils.FilterServicesByType<ISeeder>();

        _logger.LogInformation("ApplicationSeederService beginning execution...");

        // Iterate through each of the seeders and build a seeder list
        foreach (var stype in seederTypes)
        {
            var seederService = _serviceProvider.GetService(stype);

            if (seederService != null)
            {
                seeders.Add(((ISeeder)seederService));
            }
        }

        seeders = seeders.OrderByDescending(sd => sd.SeedPriority).ToList();

        foreach (var seeder in seeders)
        {
	        var serviceName = seeder.GetType().Name;

	        _logger.LogInformation($"Seeder {serviceName} started at {DateTime.UtcNow}.");

	        await seeder.SeedAsync();

	        _logger.LogInformation($"Seeder {serviceName} completed at {DateTime.UtcNow}.");

        }

        _logger.LogInformation("DBInitalizer completed.");
    }
}