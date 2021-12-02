using System;
using Autofac;
using Hamster.Adapters.Implementation.AlphaVantage;

namespace Hamster.Adapters.Implementation
{
    public class DependencyInjection : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterType(builder, typeof(AlphaVantageAdapter));
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