using System.Reflection;
using LunarApp.Data.Models;
using LunarApp.Data.Repository;
using LunarApp.Data.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LunarApp.Web.Infrastructure.Extensions
{
    /// <summary>
    /// Provides extension methods for registering repositories and user-defined services into the <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers repositories for all model types in the specified assembly into the <see cref="IServiceCollection"/>.
        /// Automatically generates repository interfaces and their corresponding implementations.
        /// </summary>
        /// <remarks>
        /// This method dynamically generates repository services for all non-abstract and non-interface types in the specified assembly.
        /// It excludes types like <see cref="ApplicationUser"/> from being registered.
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> where the repositories will be registered.</param>
        /// <param name="modelsAssembly">The assembly containing the model types that will have repositories generated.</param>
        public static void RegisterRepositories(this IServiceCollection services, Assembly modelsAssembly)
        {
            Type[] typesToExclude = new Type[] { typeof(ApplicationUser) };
            Type[] modelTypes = modelsAssembly
                .GetTypes()
                .Where(t => !t.IsAbstract &&
                                 !t.IsInterface &&
                                 !t.Name
                                      .ToLower().EndsWith("attribute"))
                .ToArray();

            foreach (Type type in modelTypes)
            {
                if (!typesToExclude.Contains(type))
                {
                    Type repositoryInterface = typeof(IRepository<,>);
                    Type repositoryInstanceType = typeof(BaseRepository<,>);

                    PropertyInfo idPropInfo = type
                        .GetProperties()
                        .Where(p => p.Name.ToLower() == "id")
                        .SingleOrDefault();

                    Type[] constructArgs = new Type[2];
                    constructArgs[0] = type;

                    if (idPropInfo == null)
                    {
                        constructArgs[1] = typeof(object);
                    }
                    else
                    {
                        constructArgs[1] = idPropInfo.PropertyType;
                    }

                    repositoryInterface = repositoryInterface.MakeGenericType(constructArgs);
                    repositoryInstanceType = repositoryInstanceType.MakeGenericType(constructArgs);

                    services.AddScoped(repositoryInterface, repositoryInstanceType);
                }
            }
        }

        /// <summary>
        /// Registers user-defined services from the specified assembly into the <see cref="IServiceCollection"/>.
        /// Automatically maps interfaces to their corresponding implementations.
        /// </summary>
        /// <remarks>
        /// This method dynamically maps services by matching the interface names (prefixed with "I") to the corresponding service implementations
        /// (with names ending in "Service").
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> where the services will be registered.</param>
        /// <param name="serviceAssembly">The assembly containing the service interfaces and implementations.</param>
        /// <exception cref="NullReferenceException">
        /// Thrown if a matching service implementation cannot be found for a service interface.
        /// </exception>
        public static void RegisterUserDefinedServices(this IServiceCollection services, Assembly serviceAssembly)
        {
            Type[] serviceInterfaceTypes = serviceAssembly
                .GetTypes()
                .Where(t => t.IsInterface)
                .ToArray();
            Type[] serviceTypes = serviceAssembly
                .GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract &&
                            t.Name.ToLower().EndsWith("service"))
                .ToArray();

            foreach (Type serviceInterfaceType in serviceInterfaceTypes)
            {
                Type? serviceType = serviceTypes
                    .SingleOrDefault(t => "i" + t.Name.ToLower() == serviceInterfaceType.Name.ToLower());
                if (serviceType == null)
                {
                    throw new NullReferenceException($"Service type could not be obtained for the service {serviceInterfaceType.Name}");
                }

                services.AddScoped(serviceInterfaceType, serviceType);
            }
        }
    }
}
