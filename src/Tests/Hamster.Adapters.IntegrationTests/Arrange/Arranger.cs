using Autofac;

namespace Hamster.Adapters.IntegrationTests.Arrange
{
    public static class Arranger
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(Adapters.Implementation.DependencyInjection).Assembly);
            builder.RegisterAssemblyModules(typeof(DependencyInjection).Assembly);
            var container = builder.Build();
            return container;
        }
    }
}