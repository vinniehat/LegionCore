namespace LegionCore.Infrastructure.Helpers.Interfaces;

public interface ISeeder
{
    int SeedPriority { get; }

    Task SeedAsync();
}