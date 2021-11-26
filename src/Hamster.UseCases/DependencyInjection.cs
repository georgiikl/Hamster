using System;
using Autofac;

using Hamster.UseCases.Stocks.Queries.GetFundamental;

namespace Hamster.UseCases
{
    public class DependencyInjection : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterType(builder, typeof(AlphaVantageAdapter));;
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