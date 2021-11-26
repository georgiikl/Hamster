using System.Reflection;
using System.Threading;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using Xunit;

namespace Hamster.Controllers.IntegrationTests.StockController
{
    public class Fundamental
    {
        [Fact]
        public async void All_fundamental_indicators_should_not_be_null()
        {
            // Arrange
            var controller = BuildContainer().Resolve<Controllers.StockController>();
            
            // Act
            var dto = await controller.Fundamental("FIVE", CancellationToken.None);
            
            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(dto.RevenueGrowth);
            Assert.NotNull(dto.OperatingProfitGrowth);
            Assert.NotNull(dto.NetProfitGrowth);
            Assert.NotNull(dto.GrossMargin);
            Assert.NotNull(dto.OperatingMargin);
            Assert.NotNull(dto.NetProfitMargin);
            Assert.NotNull(dto.Roe);
            Assert.NotNull(dto.Pe);
            Assert.NotNull(dto.Ps);
            Assert.NotNull(dto.EvEbitda);
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