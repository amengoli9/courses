using NetArchTest.Rules;
using Xunit;

namespace ArchitectureTests;

/// <summary>
/// FITNESS FUNCTIONS per l'Architettura Esagonale
/// Questi test verificano che le regole architetturali siano rispettate
/// Se la struttura viola le regole, i test falliscono
/// Nota: L'architettura esagonale ha solo 3 layer (Domain, Infrastructure, Api)
/// mentre la Clean Architecture ne ha 4 (Domain, Application, Infrastructure, Presentation)
/// </summary>
public class HexagonalArchitectureTests
{
    private const string DomainNamespace = "HexagonalArchitecture.Domain";
    private const string InfrastructureNamespace = "HexagonalArchitecture.Infrastructure";
    private const string ApiNamespace = "HexagonalArchitecture.Api";

    [Fact]
    public void Domain_Should_Not_Depend_On_Infrastructure()
    {
        // REGOLA: Il Domain non deve dipendere dall'Infrastructure
        var result = Types.InAssembly(typeof(HexagonalArchitecture.Domain.Order).Assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, "Domain non deve dipendere da Infrastructure!");
    }

    [Fact]
    public void Domain_Should_Not_Depend_On_Api()
    {
        // REGOLA: Il Domain non deve dipendere dall'Api
        var result = Types.InAssembly(typeof(HexagonalArchitecture.Domain.Order).Assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOn(ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, "Domain non deve dipendere da Api!");
    }

    [Fact]
    public void Infrastructure_Should_Depend_On_Domain()
    {
        // REGOLA: Infrastructure DEVE dipendere da Domain (implementa le Ports)
        var result = Types.InAssembly(typeof(HexagonalArchitecture.Infrastructure.Adapters.InMemoryOrderRepository).Assembly)
            .That()
            .ResideInNamespace(InfrastructureNamespace)
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, "Infrastructure deve dipendere da Domain per implementare le Ports!");
    }

    [Fact]
    public void Ports_Should_Be_Interfaces()
    {
        // REGOLA: Le Ports devono essere interfacce
        var result = Types.InAssembly(typeof(HexagonalArchitecture.Domain.Order).Assembly)
            .That()
            .ResideInNamespace($"{DomainNamespace}.Ports")
            .Should()
            .BeInterfaces()
            .GetResult();

        Assert.True(result.IsSuccessful, "Le Ports devono essere interfacce!");
    }

    [Fact]
    public void Adapters_Should_Implement_Ports()
    {
        // REGOLA: Gli Adapters devono implementare le Ports (interfacce del Domain)
        var adapters = Types.InAssembly(typeof(HexagonalArchitecture.Infrastructure.Adapters.InMemoryOrderRepository).Assembly)
            .That()
            .ResideInNamespace($"{InfrastructureNamespace}.Adapters")
            .GetTypes();

        // Verifica che almeno un adapter implementi un'interfaccia dal namespace Ports
        var hasPortImplementation = adapters.Any(type =>
            type.GetInterfaces().Any(i =>
                i.Namespace != null && i.Namespace.Contains("Ports")));

        Assert.True(hasPortImplementation, "Gli Adapters devono implementare le Ports!");
    }

    [Fact]
    public void Domain_Entities_Should_Not_Depend_On_External_Libraries()
    {
        // REGOLA: Le entità del domain dovrebbero essere POCO (Plain Old CLR Objects)
        // Non dovrebbero dipendere da librerie esterne come Entity Framework, ecc.
        var result = Types.InAssembly(typeof(HexagonalArchitecture.Domain.Order).Assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .And()
            .AreClasses()
            .ShouldNot()
            .HaveDependencyOnAll("Microsoft.EntityFrameworkCore", "Newtonsoft.Json", "System.Data")
            .GetResult();

        Assert.True(result.IsSuccessful, "Le entità del Domain non devono dipendere da librerie esterne!");
    }
}
