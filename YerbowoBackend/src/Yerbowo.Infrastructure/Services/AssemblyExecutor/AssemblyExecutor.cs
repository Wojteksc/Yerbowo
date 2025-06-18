namespace Yerbowo.Infrastructure.Services.AssemblyExecutor;

[ExcludeFromCodeCoverage]
public class AssemblyExecutor : IAssemblyExecutor
{
    public IReadOnlyList<Type> GetTypesImplementingInterface<TInterface>()
    {
        var interfaceType = typeof(TInterface);
        
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
            .ToList();
    }

    public Type GetTypeImplementingInterface<TInterface>(string className)
    {
        var interfaceType = typeof(TInterface);

        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => interfaceType.IsAssignableFrom(p) 
                && p.IsClass 
                && p.Name == className)
            .Single();
    }
}