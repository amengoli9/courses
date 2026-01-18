using NetArchTest.Rules;
using Xunit;

namespace ArchitectureTests;

/// <summary>
/// FITNESS FUNCTIONS per la Clean Architecture
/// Verifica che la Dependency Rule sia rispettata:
/// Le dipendenze devono puntare verso il centro (Domain)
/// </summary>
public class CleanArchitectureTests
{
    private const string DomainNamespace = "CleanArchitecture.Domain";
    private const string UseCasesNamespace = "CleanArchitecture.UseCases";
    private const string AdaptersNamespace = "CleanArchitecture.Adapters";
    private const string WebApiNamespace = "CleanArchitecture.WebApi";

    [Fact]
    public void Domain_Should_Not_Have_Dependencies_On_Other_Layers()
    {
        // REGOLA: Il Domain (centro) non deve dipendere da nessun altro layer
        var result = Types.InAssembly(typeof(CleanArchitecture.Domain.Entities.TodoTask).Assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOnAny(UseCasesNamespace, AdaptersNamespace, WebApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, "Domain non deve avere dipendenze verso altri layer!");
    }

    [Fact]
    public void UseCases_Should_Only_Depend_On_Domain()
    {
        // REGOLA: UseCases può dipendere solo da Domain
        var result = Types.InAssembly(typeof(CleanArchitecture.UseCases.CreateTask.CreateTaskUseCase).Assembly)
            .That()
            .ResideInNamespace(UseCasesNamespace)
            .ShouldNot()
            .HaveDependencyOnAny(AdaptersNamespace, WebApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, "UseCases deve dipendere solo da Domain!");
    }

    [Fact]
    public void UseCases_Should_Depend_On_Domain()
    {
        // REGOLA: UseCases DEVE dipendere da Domain
        var result = Types.InAssembly(typeof(CleanArchitecture.UseCases.CreateTask.CreateTaskUseCase).Assembly)
            .That()
            .ResideInNamespace(UseCasesNamespace)
            .And()
            .AreClasses()
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, "UseCases deve dipendere da Domain!");
    }

    [Fact]
    public void Adapters_Should_Only_Depend_On_Domain()
    {
        // REGOLA: Adapters può dipendere solo da Domain (non da UseCases o WebApi)
        var result = Types.InAssembly(typeof(CleanArchitecture.Adapters.Persistence.InMemoryTaskRepository).Assembly)
            .That()
            .ResideInNamespace(AdaptersNamespace)
            .ShouldNot()
            .HaveDependencyOnAny(UseCasesNamespace, WebApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, "Adapters deve dipendere solo da Domain!");
    }

    [Fact]
    public void Adapters_Should_Implement_Domain_Interfaces()
    {
        // REGOLA: Gli Adapters devono implementare interfacce del Domain
        var adapters = Types.InAssembly(typeof(CleanArchitecture.Adapters.Persistence.InMemoryTaskRepository).Assembly)
            .That()
            .ResideInNamespace($"{AdaptersNamespace}.Persistence")
            .GetTypes();

        var hasInterfaceImplementation = adapters.Any(type =>
            type.GetInterfaces().Any(i =>
                i.Namespace != null && i.Namespace.StartsWith(DomainNamespace)));

        Assert.True(hasInterfaceImplementation, "Gli Adapters devono implementare interfacce del Domain!");
    }

    [Fact]
    public void Domain_Entities_Should_Be_In_Entities_Namespace()
    {
        // REGOLA: Le entità devono stare nel namespace Entities
        var result = Types.InAssembly(typeof(CleanArchitecture.Domain.Entities.TodoTask).Assembly)
            .That()
            .ResideInNamespace($"{DomainNamespace}.Entities")
            .Should()
            .BeClasses()
            .GetResult();

        Assert.True(result.IsSuccessful, "Le entità devono essere classi nel namespace Entities!");
    }

    [Fact]
    public void Domain_Repositories_Should_Be_Interfaces()
    {
        // REGOLA: I repository nel Domain devono essere interfacce
        var result = Types.InAssembly(typeof(CleanArchitecture.Domain.Entities.TodoTask).Assembly)
            .That()
            .ResideInNamespace($"{DomainNamespace}.Repositories")
            .Should()
            .BeInterfaces()
            .GetResult();

        Assert.True(result.IsSuccessful, "I repository nel Domain devono essere interfacce!");
    }

    [Fact]
    public void UseCases_Should_Have_UseCase_Suffix()
    {
        // REGOLA: Le classi di UseCases dovrebbero terminare con "UseCase"
        var result = Types.InAssembly(typeof(CleanArchitecture.UseCases.CreateTask.CreateTaskUseCase).Assembly)
            .That()
            .ResideInNamespace(UseCasesNamespace)
            .And()
            .AreClasses()
            .And()
            .DoNotHaveNameMatching(".*Request$")
            .And()
            .DoNotHaveNameMatching(".*Response$")
            .And()
            .DoNotHaveNameMatching(".*Dto$")
            .Should()
            .HaveNameEndingWith("UseCase")
            .GetResult();

        Assert.True(result.IsSuccessful, "Le classi di Use Case dovrebbero terminare con 'UseCase'!");
    }

    [Fact]
    public void Domain_Should_Not_Depend_On_External_Frameworks()
    {
        // REGOLA: Domain non deve dipendere da framework esterni
        var result = Types.InAssembly(typeof(CleanArchitecture.Domain.Entities.TodoTask).Assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOnAll(
                "Microsoft.EntityFrameworkCore",
                "Microsoft.AspNetCore",
                "Newtonsoft.Json",
                "System.Data.SqlClient")
            .GetResult();

        Assert.True(result.IsSuccessful, "Domain non deve dipendere da framework esterni!");
    }

    [Fact]
    public void WebApi_Can_Depend_On_All_Layers()
    {
        // REGOLA: Il layer più esterno (WebApi) può dipendere da tutti gli altri
        // Questo è OK perché è il punto di composizione dell'applicazione
        var result = Types.InAssembly(typeof(CleanArchitecture.WebApi.Program).Assembly)
            .That()
            .ResideInNamespace(WebApiNamespace)
            .Should()
            .HaveDependencyOnAny(DomainNamespace, UseCasesNamespace, AdaptersNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, "WebApi dovrebbe dipendere dagli altri layer per fare il wiring!");
    }

    [Fact]
    public void Repository_Implementations_Should_Have_Repository_Suffix()
    {
        // REGOLA: Le implementazioni dei repository dovrebbero terminare con "Repository"
        var result = Types.InAssembly(typeof(CleanArchitecture.Adapters.Persistence.InMemoryTaskRepository).Assembly)
            .That()
            .ResideInNamespace($"{AdaptersNamespace}.Persistence")
            .And()
            .AreClasses()
            .Should()
            .HaveNameEndingWith("Repository")
            .GetResult();

        Assert.True(result.IsSuccessful, "Le implementazioni dei repository dovrebbero terminare con 'Repository'!");
    }
}
