using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
            => ConfigureServicesByAssembly(services, AssemblyEntitiesName, "Repositories");

        public static void ConfigureManagers(this IServiceCollection services)
            => ConfigureServicesByAssembly(services, AssemblyLogicName, "Managers");

        private static void ConfigureServicesByAssembly(IServiceCollection services, string assemblyName, string assemblyFolder)
        {
            var assembly = Assembly.Load(assemblyName);
            var assemblyFilesName = $"{assemblyName}.{assemblyFolder}";
            var files = assembly.GetTypes().Where(f => f.FullName.StartsWith(assemblyFilesName));
            var classList = files.Where(f => f.IsClass && f.GetInterfaces().Any());
            var interfaceList = files.Where(f => f.IsInterface);
            foreach (var iFile in interfaceList)
            {
                var className = iFile.Name[1..];
                var classFile = classList.FirstOrDefault(c => c.Name.Equals(className) && c.GetInterface(iFile.Name) != null);
                if (classFile != null)
                    services.AddScoped(iFile, classFile);
            }
        }
    }
}
