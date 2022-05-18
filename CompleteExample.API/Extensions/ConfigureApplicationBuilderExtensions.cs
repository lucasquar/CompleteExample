using Microsoft.AspNetCore.Builder;

namespace CompleteExample.API.Extensions
{
    public static class ConfigureApplicationBuilderExtensions
    {
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eyrus Complete Example v1");
            });
        }
    }
}
