using System;
using Microsoft.Extensions.DependencyInjection;
using TrainProblem.Implementations;
using TrainProblem.Interfaces;

namespace TrainProblem
{
    
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection=new ServiceCollection();
            ConfigureDependencies(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetRequiredService<RouteProcessing>().ProcessRoutingDataFromTerminal();
        }

        private static void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton<RouteProcessing>();
            services.AddTransient<ITrainRouteLogic, TrainRouteLogic>();
        }
    }
}