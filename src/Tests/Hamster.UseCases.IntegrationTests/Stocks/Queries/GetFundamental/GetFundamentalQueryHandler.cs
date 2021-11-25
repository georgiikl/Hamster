using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using Autofac;
using Hamster.Controllers;
using Hamster.UseCases.Stocks.Queries.GetFundamental;
using MediatR;
using MediatR.Pipeline;
using Xunit;

namespace Hamster.UseCases.IntegrationTests.Stocks.Queries.GetFundamental
{
    public class GetFundamentalQueryHandler
    {
        [Fact]
        public async void Xcc()
        {
            var mediator = BuildMediator();
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
        
        private static IMediator BuildMediator(/*WrappingWriter writer*/)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                /*typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),*/
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(Hamster.UseCases.DependencyInjection).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    // when having a single class implementing several handler types
                    // this call will cause a handler to be called twice
                    // in general you should try to avoid having a class implementing for instance `IRequestHandler<,>` and `INotificationHandler<>`
                    // the other option would be to remove this call
                    // see also https://github.com/jbogard/MediatR/issues/462
                    .AsImplementedInterfaces();
            }

            //builder.RegisterInstance(writer).As<TextWriter>();

            // It appears Autofac returns the last registered types first
            /*builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));*/
            /*builder.RegisterGeneric(typeof(GenericRequestPreProcessor<>)).As(typeof(IRequestPreProcessor<>));
            builder.RegisterGeneric(typeof(GenericRequestPostProcessor<,>)).As(typeof(IRequestPostProcessor<,>));
            builder.RegisterGeneric(typeof(GenericPipelineBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ConstrainedRequestPostProcessor<,>)).As(typeof(IRequestPostProcessor<,>));
            builder.RegisterGeneric(typeof(ConstrainedPingedHandler<>)).As(typeof(INotificationHandler<>));*/

            builder.RegisterAssemblyModules(typeof(UseCases.DependencyInjection).Assembly);
            
            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var container = builder.Build();

            // The below returns:
            //  - RequestPreProcessorBehavior
            //  - RequestPostProcessorBehavior
            //  - GenericPipelineBehavior
            //  - RequestExceptionActionProcessorBehavior
            //  - RequestExceptionProcessorBehavior

            //var behaviors = container
            //    .Resolve<IEnumerable<IPipelineBehavior<Ping, Pong>>>()
            //    .ToList();

            var mediator = container.Resolve<IMediator>();

            return mediator;
        }
        
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterAssemblyTypes(typeof(Hamster.UseCases.DependencyInjection).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                // when having a single class implementing several handler types
                // this call will cause a handler to be called twice
                // in general you should try to avoid having a class implementing for instance `IRequestHandler<,>` and `INotificationHandler<>`
                // the other option would be to remove this call
                // see also https://github.com/jbogard/MediatR/issues/462
                .AsImplementedInterfaces();
            
            builder.RegisterAssemblyModules(typeof(UseCases.DependencyInjection).Assembly);
            
            var container = builder.Build();

            return container;
        }
    }
}