using Microsoft.Extensions.FileProviders;

namespace CAWebProject.Api.Swagger;

public static class SwaggerUIMiddlewareExtentions
{
    public static IApplicationBuilder UseSwaggerDocumentation(
        this IApplicationBuilder builder, bool useDarkmode = true)
    {
        var swaggerPath = Path.Combine(Directory.GetCurrentDirectory(), "Documentation", "Swagger");
        var stylesPath = Path.Combine(Directory.GetCurrentDirectory(), "Documentation", "Swagger", "Styles");
        
        builder.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(swaggerPath),
            RequestPath = new PathString("/swagger")
        });

        if (useDarkmode)
        {
            builder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(stylesPath),
                RequestPath = new PathString("/swagger/styles")
            });
        }

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

            if (useDarkmode)
            {
                options.InjectStylesheet("/swagger/styles/Base.css");
                options.InjectStylesheet("/swagger/styles/DarkTheme.css");
            }
        });
        
        return builder;
    }
}