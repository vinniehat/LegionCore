using LegionCore.Infrastructure.Helpers.Interfaces;

namespace LegionCore.Infrastructure.Helpers.Services;

public class UtilityService : IService
{

    public List<Type> FilterServicesByType<T>()
    {
        var type = typeof(T);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));

        return types.ToList();
    }
}