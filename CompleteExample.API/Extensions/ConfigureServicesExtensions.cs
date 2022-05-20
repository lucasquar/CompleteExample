using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CompleteExample.API.Extensions
{
    public static class ConfigureServicesExtensions
    {
        private const string AssemblyLogicName = "CompleteExample.Logic";
        private const string AssemblyEntitiesName = "CompleteExample.Entities";

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "Eyrus Complete Example", Version = "v1" });
            });
        }

        public static void ConfigureRepositories(this IServiceCollection services)
            => ConfigureServicesByAssembly(services, AssemblyEntitiesName, "Repositories", ServiceLifetime.Scoped);

        public static void ConfigureManagers(this IServiceCollection services)
            => ConfigureServicesByAssembly(services, AssemblyLogicName, "Managers", ServiceLifetime.Scoped);

        private static void ConfigureServicesByAssembly(IServiceCollection services, string assemblyName, string assemblyFolder, ServiceLifetime lifeTime)
        {
            var assembly = Assembly.Load(assemblyName);
            var assemblyFilesName = $"{assemblyName}.{assemblyFolder}";
            var files = assembly.GetTypes().Where(f => f.FullName.StartsWith(assemblyFilesName));
            var classList = files.Where(f => f.IsClass && f.GetInterfaces().Any());
            var interfaceList = files.Where(f => f.IsInterface);
            var serviceDescriptorList = new List<ServiceDescriptor>();
            foreach (var iType in interfaceList)
            {
                var cName = iType.Name[1..];
                var cType = classList.FirstOrDefault(c => c.Name.Equals(cName) && c.GetInterface(iType.Name) != null);
                if (cType != null)
                    serviceDescriptorList.Add(new ServiceDescriptor(iType, cType, lifeTime));
            }

            if (serviceDescriptorList.Any())
                services.TryAddEnumerable(serviceDescriptorList);
        }
    }
}
