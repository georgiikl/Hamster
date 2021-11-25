using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using Autofac;
using Hamster.Controllers;
using Hamster.UseCases.Stocks.Queries.GetFundamental;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Pipeline;
using Xunit;

namespace Hamster.UseCases.IntegrationTests.Stocks.Queries.GetFundamental
{
    public class GetFundamentalQueryHandler
    {
        [Fact]
        public async void Xcc()
        {
            var container = BuildContainer();
            var mediator = container.Resolve<IMediator>();
            //container.Resolve<StockController>();
            var controller = new StockController(mediator);
            var dto = await controller.Fundamental("FIVE");
            Assert.NotNull(dto);
            Assert.Equal(4, dto.RevenueGrowth);
        }
        
        [Fact]
        public async void Xcc2()
        {
            var container = BuildContainer();
            var handler = container.Resolve<IRequestHandler<GetFundamentalQuery, FundamentalDto>>();
            var request = new GetFundamentalQuery{Ticker = "Five"};
            var dto = await handler.Handle(request, CancellationToken.None);
            Assert.NotNull(dto);
            Assert.Equal(4, dto.RevenueGrowth);
        }
        
        private static IMediator BuildMediator()
        {
            var container = BuildContainer();
            var mediator = container.Resolve<IMediator>();
            return mediator;
        }
        
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMediatR(typeof(UseCases.DependencyInjection).GetTypeInfo().Assembly);
            builder.RegisterAssemblyModules(typeof(UseCases.DependencyInjection).Assembly);
            var container = builder.Build();
            return container;
        }
    }
}