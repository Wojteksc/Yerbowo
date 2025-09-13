namespace Yerbowo.Architecture.Tests;

public class ArchitectureTests
{
    private const string ApiNamespace = "Yerbowo.Api";
    private const string ApplicationNamespace = "Yerbowo.Application";
    private const string DomainNamespace = "Yerbowo.Domain";
    private const string InfrastructureNamespace = "Yerbowo.Infrastructure";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = typeof(Domain.AssemblyReference).Assembly;

        var otherProjetcts = new[]
        {
            ApiNamespace,
            ApplicationNamespace,
            InfrastructureNamespace
        };

        var testResult = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjetcts)
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = typeof(Application.AssemblyReference).Assembly;

        var otherProjetcts = new[]
        {
            ApiNamespace,
            ApplicationNamespace,
            InfrastructureNamespace
        };

        var testResult = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjetcts)
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = typeof(Infrastructure.AssemblyReference).Assembly;

        var otherProjetcts = new[]
        {
            DomainNamespace,
            ApiNamespace,
            InfrastructureNamespace
        };

        var testResult = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjetcts)
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_Should_HaveDependencyOnDomain()
    {
        var assembly = typeof(Application.AssemblyReference).Assembly;

        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_HaveDependencyOnRequestDispatcher()
    {
        var assembly = typeof(Api.AssemblyReference).Assembly;

        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Should()
            .HaveDependencyOn(DomainNamespace + ".RequestProcessing")
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }
}