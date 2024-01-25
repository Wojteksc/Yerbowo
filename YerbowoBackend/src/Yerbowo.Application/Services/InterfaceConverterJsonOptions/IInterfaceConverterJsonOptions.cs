namespace Yerbowo.Application.Services.InterfaceConverterJsonOptions;

public interface IInterfaceConverterJsonOptions
{
    JsonSerializerOptions GetJsonOptions(string className);
    JsonSerializerOptions GetJsonOptions();
}