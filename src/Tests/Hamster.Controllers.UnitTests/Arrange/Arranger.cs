using System.Reflection;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace Hamster.Controllers.UnitTests.Arrange
{
    public static class Arranger
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterAssemblyTypes(typeof(Controllers.StockController).GetTypeInfo().Assembly)
                .AsSelf()
                .InstancePerLifetimeScope();
            
            builder.RegisterMediatR(typeof(UseCases.DependencyInjection).GetTypeInfo().Assembly);
            builder.RegisterAssemblyModules(typeof(UseCases.DependencyInjection).Assembly);
            //builder.RegisterAssemblyModules(typeof(Adapters.Implementation.DependencyInjection).Assembly);
            builder.RegisterAssemblyModules(typeof(DependencyInjection).Assembly);
            var container = builder.Build();
            return container;
        }
    }
}