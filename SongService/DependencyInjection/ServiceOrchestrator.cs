namespace SongService.DependencyInjection;

public static class ServiceManager
{
    public static void RegisterServices(this IServiceCollection services)
    {
        Type singletonService = typeof(SingletonServiceAttribute);
        Type transientService = typeof(TransientServiceAttribute);
        Type scopedService = typeof(ScopedServiceAttribute);


        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => !p.IsInterface &&
                        (p.IsDefined(scopedService, true) ||
                         p.IsDefined(transientService, true) ||
                         p.IsDefined(singletonService, true)))
            .Select(s => new
            {
                Service = s.GetInterface($"I{s.Name}"),
                Implementation = s
            }).Where(x => x.Service != null);


        foreach (var type in types)
        {
            if (type.Implementation.IsDefined(scopedService, false))
            {
                services.AddScoped(type.Service!, type.Implementation);
            }

            if (type.Implementation.IsDefined(transientService, false))
            {
                services.AddTransient(type.Service!, type.Implementation);
            }

            if (type.Implementation.IsDefined(singletonService, false))
            {
                services.AddSingleton(type.Service!, type.Implementation);
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class ScopedServiceAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class)]
public class SingletonServiceAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class)]
public class TransientServiceAttribute : Attribute { }