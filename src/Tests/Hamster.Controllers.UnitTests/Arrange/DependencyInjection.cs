using System;
using Autofac;
using Autofac.Extras.Moq;

namespace Hamster.Controllers.UnitTests.Arrange
{
    public class DependencyInjection : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //RegisterType(builder, typeof(AlphaVantageAdapter));
            var mockAlphaVantageAdapter = AlphaVantageAdapter.GetMock();
            builder.RegisterMock(mockAlphaVantageAdapter);
        }

        private static void RegisterType(ContainerBuilder builder, Type type)
        {
            builder
                .RegisterType(type)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}