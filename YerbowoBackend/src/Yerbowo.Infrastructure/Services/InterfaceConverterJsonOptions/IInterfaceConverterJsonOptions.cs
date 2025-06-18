namespace Yerbowo.Infrastructure.Services.InterfaceConverterJsonOptions;

public interface IInterfaceConverterJsonOptions
{
    JsonSerializerOptions GetJsonOptions(string className);
    JsonSerializerOptions GetJsonOptions();
}