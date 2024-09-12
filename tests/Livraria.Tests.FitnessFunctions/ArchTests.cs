using Livraria.Core.Domain.Commands;
using Livraria.Core.Domain.Queries;
using Livraria.Core.Infrastructure;
using FluentValidation;
using Shouldly;

namespace Livraria.Tests.FitnessFunctions;

public class ArchTests(ArchTestsFixture fixture) : IClassFixture<ArchTestsFixture>
{
    [Fact]
    public void ShouldPass_WhenAllCommandsHaveValidators()
    {
        // Arrange
        var commandInterfaces = fixture
            .DomainAssembly
            .That()
            .Inherit(typeof(CommandBase))
            .And()
            .ResideInNamespaceContaining("Livraria.Domain.Commands")
            .GetTypes()
            .ToList();

        var abstractValidatorsImplementations = fixture
            .DomainAssembly
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .And()
            .ResideInNamespaceContaining("Livraria.Domain.Commands")
            .And()
            .AreClasses()
            .GetTypes()
            .ToList();

        foreach (var command in commandInterfaces)
        {
            var implementation = abstractValidatorsImplementations
                .Any(c => c.BaseType?.IsGenericType == true &&
                          c.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>) &&
                          c.BaseType.GenericTypeArguments.Contains(command));

            implementation.ShouldBeTrue($"Missing AbstractValidator implementation for {command.Name}.");
        }
    }
    
    [Fact]
    public void ShouldPass_WhenAllQueriesHaveValidators()
    {
        // Arrange
        var commandInterfaces = fixture
            .DomainAssembly
            .That()
            .Inherit(typeof(QueryBase))
            .And()
            .ResideInNamespaceContaining("Livraria.Domain.Queries")
            .GetTypes()
            .ToList();

        var abstractValidatorsImplementations = fixture
            .DomainAssembly
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .And()
            .ResideInNamespaceContaining("Livraria.Domain.Queries")
            .And()
            .AreClasses()
            .GetTypes()
            .ToList();

        foreach (var command in commandInterfaces)
        {
            var implementation = abstractValidatorsImplementations
                .Any(c => c.BaseType?.IsGenericType == true &&
                          c.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>) &&
                          c.BaseType.GenericTypeArguments.Contains(command));

            implementation.ShouldBeTrue($"Missing AbstractValidator implementation for {command.Name}.");
        }
    }

    [Fact]
    public void ShouldPass_WhenCommandsHaveOneImplementation()
    {
        // Arrange
        var commandInterfaces = fixture
            .DomainAssembly
            .That()
            .Inherit(typeof(CommandBase))
            .And()
            .ResideInNamespaceContaining("Livraria.Domain.Commands")
            .GetTypes()
            .ToList();


        var implementedCommands = fixture
            .ServiceAssembly
            .That()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .And()
            .ResideInNamespaceContaining("Livraria.Services.CommandHandlers")
            .GetTypes()
            .ToList();

        // Act & Assert

        foreach (var commandType in commandInterfaces)
        {
            var matchingHandlers = implementedCommands.Where(handlerType =>
            {
                // Get the ICommandHandler<,> interface implemented by the handler
                var commandHandlerInterface = handlerType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType &&
                                         i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

                if (commandHandlerInterface == null)
                    return false; // Type does not implement ICommandHandler

                // Get the generic arguments of the ICommandHandler interface
                var handlerCommandType = commandHandlerInterface.GetGenericArguments()[0];

                // Check if the handler's command type matches the current command type
                return handlerCommandType == commandType;
            }).ToList();

            // Assert that there is at least one handler for each command type
            matchingHandlers.Count.ShouldBe(1,
                $"No handler or more than one found for command type {commandType.Name}.");
        }
    }

    [Fact]
    public void ShouldPass_WhenQueriesHaveOneImplementation()
    {
        // Arrange
        var queryInterfaces = fixture
            .DomainAssembly
            .That()
            .Inherit(typeof(QueryBase))
            .And()
            .ResideInNamespaceContaining("Livraria.Domain.Queries")
            .GetTypes()
            .ToList();


        var implementedQueries = fixture
            .ServiceAssembly
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .And()
            .ResideInNamespaceContaining("Livraria.Services.QueryHandlers")
            .GetTypes()
            .ToList();

        // Act & Assert

        foreach (var queryType in queryInterfaces)
        {
            var matchingHandlers = implementedQueries.Where(handlerType =>
            {
                // Get the ICommandHandler<,> interface implemented by the handler
                var queryHandlerInterface = handlerType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType &&
                                         i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

                if (queryHandlerInterface == null)
                    return false; // Type does not implement ICommandHandler

                // Get the generic arguments of the ICommandHandler interface
                var queryHandlerType = queryHandlerInterface.GetGenericArguments()[0];

                // Check if the handler's command type matches the current command type
                return queryHandlerType == queryType;
            }).ToList();

            // Assert that there is at least one handler for each command type
            matchingHandlers.Count.ShouldBe(1, $"No handler or more than one found for query type {queryType.Name}.");
        }
    }

    [Fact]
    public void ShouldPass_WhenDomainDoestNotReferenceApplication()
    {
        var result = fixture
            .DomainAssembly
            .ShouldNot()
            .HaveDependencyOn("Clicheria.Application")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void ShouldPass_WhenAllRepositoriesHaveImplementations()
    {
        // Arrange
        var repositoryInterfaces = fixture
            .DomainAssembly
            .That()
            .ImplementInterface(typeof(IRepository<>))
            .And()
            .ResideInNamespaceContaining("Livraria.Domain.Repository")
            .GetTypes()
            .ToList();


        var implementedRepositories = fixture
            .InfrastructureAssembly
            .That()
            .ResideInNamespaceContaining("Livraria.Infrastructure.Repository")
            .And()
            .AreClasses()
            .And()
            .ImplementInterface(typeof(IRepository<>))
            .GetTypes()
            .ToList();

        foreach (var repository in repositoryInterfaces)
        {
            var implementation = implementedRepositories
                .Any(c => c.GetInterfaces().Any(c => c == repository));

            implementation.ShouldBeTrue($"Missing repository implementation for {repository.Name}.");
        }
    }
}