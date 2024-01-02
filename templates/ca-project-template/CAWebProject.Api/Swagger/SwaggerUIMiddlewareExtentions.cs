using Microsoft.Extensions.FileProviders;

namespace CAWebProject.Api.Swagger;

public static class SwaggerUIMiddlewareExtentions
{
    public static IApplicationBuilder UseSwaggerDocumentation(
        this IApplicationBuilder builder)
    {
        var swaggerPath = Path.Combine(Directory.GetCurrentDirectory(), "Documentation", "Swagger");
        
        builder.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(swaggerPath),
            RequestPath = new PathString("/swagger")
        });
            
        builder.UseSwaggerUI(options =>
        {
            var files = Directory.GetFiles(swaggerPath, "*.swagger.json")
                .Select(file => new FileInfo(file))
                .OrderBy(file => file.Name.Split(".").First());
                
            foreach (var file in files)
            {
                var url = $"/swagger/{file.Name}";
                var name = file.Name.Split(".").First();
                    
                options.SwaggerEndpoint(url, name);
            }
        });
        
        return builder;
    }
}