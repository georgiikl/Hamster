using Autofac;

namespace Hamster.Web.Extensions
{
    internal static class ObjectFactoryExtensions
    {
        internal static void ConfigureContainer(this ContainerBuilder builder)
        {
            var assembles = new[]
            {
                typeof(DependencyInjection).Assembly,
                typeof(UseCases.DependencyInjection).Assembly
            };
            builder.RegisterAssemblyModules(assembles);
        }
    }
}