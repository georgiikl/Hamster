using System.Reflection;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using Xunit;

namespace Hamster.Controllers.IntegrationTests.StockController
{
    public class Fundamental
    {
        [Fact]
        public async void Xcc()
        {
            var container = BuildContainer();
            var controller = container.Resolve<Controllers.StockController>();
            var dto = await controller.Fundamental("FIVE");
            Assert.NotNull(dto);
            Assert.Equal(4, dto.RevenueGrowth);
        }
        
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterAssemblyTypes(typeof(Controllers.StockController).GetTypeInfo().Assembly)
                .AsSelf()
                .InstancePerLifetimeScope();
            
            builder.RegisterMediatR(typeof(UseCases.DependencyInjection).GetTypeInfo().Assembly);
            builder.RegisterAssemblyModules(typeof(UseCases.DependencyInjection).Assembly);
            var container = builder.Build();
            return container;
        }
    }
}