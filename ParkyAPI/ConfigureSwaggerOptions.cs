using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace ParkyAPI;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var desc in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(desc.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
            {
                Title = $"Parky API {desc.ApiVersion}",
                Version=desc.ApiVersion.ToString(),
            });
        }

        // Adding XML Documentation to UI --------------------------------------------------
        var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var cmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
        options.IncludeXmlComments(cmlCommentFullPath);
        //----------------------------------------------------------------------------------
    }
}
