using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
                Version = desc.ApiVersion.ToString(),
            });
        }

        // Adding Authorize Button for JWT Bearer Token Pop-up --------------------------
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
            "Enter 'Bearer' [space] and then your token in the text input below \r\n\r\n" +
            "Example: \"Bearer 12112312cded\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id="Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
        //---------------------------------------------------------------------------------

        // Adding XML Documentation to UI --------------------------------------------------
        var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var cmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
        options.IncludeXmlComments(cmlCommentFullPath);
        //----------------------------------------------------------------------------------
    }
}
