using Autofac;
using Livraria.Core;
using Livraria.Core.Domain.Commands;
using Livraria.Core.Domain.Queries;

namespace Livraria.ApiHost.Autofac;

public class CoreModule : Module
{
    protected override void Load(ContainerBuilder b)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(c => c.FullName!.Contains("Livraria")).ToArray();

        b.RegisterAssemblyTypes(assemblies)
            .Where(t => typeof(ITransientDependency).IsAssignableFrom(t))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        b.RegisterAssemblyTypes(assemblies)
            .Where(t => typeof(ISingletonDependency).IsAssignableFrom(t))
            .AsImplementedInterfaces()
            .SingleInstance();
        
        b.RegisterAssemblyTypes(assemblies)
            .AsClosedTypesOf(typeof(ICommandHandler<,>))
            .AsSelf()
            .InstancePerLifetimeScope();
        
        b.RegisterAssemblyTypes(assemblies)
            .AsClosedTypesOf(typeof(IQueryHandler<,>))
            .AsSelf()
            .InstancePerLifetimeScope();

    }
}