using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace Hamster.Web.Extensions
{
    internal static class ObjectFactoryExtensions
    {
        internal static void ConfigureContainer(this ContainerBuilder builder)
        {
            var assembles = new[]
            {
                typeof(DependencyInjection).Assembly,
                typeof(UseCases.DependencyInjection).Assembly,
                typeof(Adapters.Implementation.DependencyInjection).Assembly
            };
            builder.RegisterAssemblyModules(assembles);
            builder.RegisterMediatR(typeof(Hamster.UseCases.DependencyInjection).Assembly);
        }
    }
}