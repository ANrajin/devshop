using devshop.api.Cores.Contracts;
using System.Reflection;

namespace devshop.api.Configs
{
    public static class InstallDependencies
    {
        public static IServiceCollection AddServices(this IServiceCollection services,
            IConfiguration configuration,
            params Assembly[] assemblies)
        {
            var serviceInstallers = assemblies
                .SelectMany(x => x.DefinedTypes)
                .Where(IsAssignableToType<IServiceInstaller>)
                .Select(Activator.CreateInstance)
                .Cast<IServiceInstaller>();

            foreach (var serviceInstaller in serviceInstallers)
            {
                serviceInstaller.Install(services, configuration);
            }

            return services;
        }

        private static bool IsAssignableToType<T>(TypeInfo typeInfo)
        {
            return typeof(T).IsAssignableFrom(typeInfo)
                && !typeInfo.IsInterface
                && !typeInfo.IsAbstract;
        }
    }
}
