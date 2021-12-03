using System.Reflection;
using Autofac;
using Autofac.Extras.Moq;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace Hamster.Controllers.UnitTests.Arrange
{
    public static class Arranger
    {
        /// <summary>
        /// Important! AutoMock that is returned is IDisposable object.
        /// So, have to call Dispose or to use "using" statement.
        /// </summary>
        /// <returns></returns>
        public static AutoMock BuildMockContainer()
        {
            var container = AutoMock.GetLoose(Configure);
            return container;
        }

        private static void Configure(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(Controllers.StockController).GetTypeInfo().Assembly)
                .AsSelf()
                .InstancePerLifetimeScope();
            
            builder.RegisterMediatR(typeof(UseCases.DependencyInjection).GetTypeInfo().Assembly);
            builder.RegisterAssemblyModules(typeof(UseCases.DependencyInjection).Assembly);
            builder.RegisterAssemblyModules(typeof(DependencyInjection).Assembly);
        }
    }
}