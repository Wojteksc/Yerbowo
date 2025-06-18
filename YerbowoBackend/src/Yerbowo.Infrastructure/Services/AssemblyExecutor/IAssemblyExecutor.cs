namespace Yerbowo.Infrastructure.Services.Assembly;

public interface IAssemblyExecutor
{
    Type GetTypeImplementingInterface<TInterface>(string className);
    IReadOnlyList<Type> GetTypesImplementingInterface<TInterface>();
}