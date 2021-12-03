using System;
using Autofac;

namespace Hamster.Adapters.IntegrationTests.Arrange
{
    public class DependencyInjection : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //RegisterType(builder, typeof(AlphaVantageAdapter));
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