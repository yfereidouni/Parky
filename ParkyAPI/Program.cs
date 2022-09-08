using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

//SecretKey
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

//JWT Bearer Token config
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

#region SwaggerGenOptions
/*
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("ParkyOpenAPISpecNP",
        new Microsoft.OpenApi.Models.OpenApiInfo()
        {
            Title = "Parky API (National Park)",
            Version = "1",
            Description = "eHubStar Parky API NP",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            {
                Email = "Yasser.Fereidouni@gmail.com",
                Name = "Yasser Fereidouni",
                Url = new Uri("https://github.com/yfereidouni"),
            },
            License = new Microsoft.OpenApi.Models.OpenApiLicense()
            {
                Name = "MIT License",
                Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            }
        });
    options.SwaggerDoc("ParkyOpenAPISpecTrails",
    new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Parky API (Trails)",
        Version = "1",
        Description = "eHubStar Parky API Trails",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Email = "Yasser.Fereidouni@gmail.com",
            Name = "Yasser Fereidouni",
            Url = new Uri("https://github.com/yfereidouni"),
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense()
        {
            Name = "MIT License",
            Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
        }
    });

    // Adding XML Documentation to UI --------------------------------------------------
    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var cmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
    options.IncludeXmlComments(cmlCommentFullPath);
    //----------------------------------------------------------------------------------
});
*/
#endregion

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddCors();

builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.AddScoped<ITrailRepository, TrailRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(ParkyMappings));

//Showing version in Swagger UI -------------------------------------------------
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true; // Load default version
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
});
//--------------------------------------------------------------------------------

var app = builder.Build();

//Auto-Migration ---------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

}
//-------------------------------------------------------------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseSwagger();

var provider = builder.Services.BuildServiceProvider()
        .GetService<IApiVersionDescriptionProvider>();

app.UseSwaggerUI(options =>
{
    foreach (var desc in provider.ApiVersionDescriptions)
        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());

    options.RoutePrefix = "";
});

//app.UseSwaggerUI(options =>
//{
//    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecNP/swagger.json", "Parky API - National Park");
//    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API  - Trails");
//    options.RoutePrefix = "";
//});

app.UseRouting();
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();