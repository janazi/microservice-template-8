using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceTemplate.Api
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Replace a registered service with different implementation type.
        /// </summary>
        /// <typeparam name="TService">Service type to replace</typeparam>
        /// <typeparam name="TImplementation">New implementation</typeparam>
        /// <param name="services">Service collection</param>
        /// <param name="lifetime">Lifetime that TService was registered under</param>
        /// <returns></returns>
        public static IServiceCollection Replace<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);

            services.Add(descriptorToAdd);

            return services;
        }

        /// <summary>
        /// Replace a registered service with the instance produced by the specific implementation factory
        /// </summary>
        /// <typeparam name="TService">Service type to replace</typeparam>
        /// <param name="services">Service collection</param>
        /// <param name="implementationFactory">Implementation factory for an instance of TService</param>
        /// <param name="lifetime">Lifetime that TService was registered under</param>
        /// <returns></returns>
        public static IServiceCollection Replace<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory, ServiceLifetime lifetime)
            where TService : class
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), implementationFactory, lifetime);

            services.Add(descriptorToAdd);

            return services;
        }

        /// <summary>
        /// Replace a registered service with the specific instance
        /// </summary>
        /// <typeparam name="TService">Service type to replace</typeparam>
        /// <param name="services">Service collection</param>
        /// <param name="implementationInstance">Implementation instance which will be returned each time the service is resolved</param>
        /// <returns></returns>
        public static IServiceCollection Replace<TService>(this IServiceCollection services, TService implementationInstance)
            where TService : class
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), implementationInstance);

            services.Add(descriptorToAdd);

            return services;
        }
    }
}
