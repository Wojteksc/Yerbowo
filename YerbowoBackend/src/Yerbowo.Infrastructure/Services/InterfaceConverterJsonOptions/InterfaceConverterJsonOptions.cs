namespace Yerbowo.Infrastructure.Services.InterfaceConverterJsonOptions;

[ExcludeFromCodeCoverage]
public class InterfaceConverterJsonOptions : IInterfaceConverterJsonOptions
{
    private JsonSerializerOptions _serializerOptions;
    private readonly IAssemblyExecutor _assemblyExecutor;

    public InterfaceConverterJsonOptions(IAssemblyExecutor assemblyExecutor)
    {
        _assemblyExecutor = assemblyExecutor;
    }

    public JsonSerializerOptions GetJsonOptions()
    {
        if (_serializerOptions != null)
        {
            return _serializerOptions;
        }

        _serializerOptions = new JsonSerializerOptions();

        var classesTypes = _assemblyExecutor.GetTypesImplementingInterface<IDomainEvent>();

        List<Type> converterTypes = CreateTypes(classesTypes, typeof(IDomainEvent)).ToList();

        foreach (var converterType in converterTypes)
        {
            _serializerOptions.Converters.Add(Activator.CreateInstance(converterType) as JsonConverter);
        }

        return _serializerOptions;
    }

    public JsonSerializerOptions GetJsonOptions(string className)
    {
        JsonSerializerOptions serializerOptions = new();

        var classType = _assemblyExecutor.GetTypeImplementingInterface<IDomainEvent>(className);

        Type converterType = CreateType(classType, typeof(IDomainEvent));

        serializerOptions.Converters.Add(Activator.CreateInstance(converterType) as JsonConverter);

        return serializerOptions;
    }

    private static IEnumerable<Type> CreateTypes(IReadOnlyList<Type> classesTypes, Type interfaceType)
    {
        foreach (var classType in classesTypes)
        {
            Type converterType = typeof(InterfaceConverter<,>).MakeGenericType(classType, interfaceType);
            yield return converterType;
        }
    }

    private static Type CreateType(Type classType, Type interfaceType)
    {
        Type converterType = typeof(InterfaceConverter<,>).MakeGenericType(classType, interfaceType);
        return converterType;
    }
}