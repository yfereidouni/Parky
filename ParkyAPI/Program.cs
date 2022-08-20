using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("ParkyOpenAPISpec",
        new Microsoft.OpenApi.Models.OpenApiInfo()
        {
            Title = "Parky API",
            Version = "1",
            Description="eHubStar Parky API",
            Contact= new Microsoft.OpenApi.Models.OpenApiContact()
            {
                Email="Yasser.Fereidouni@gmail.com",
                Name="Yasser Fereidouni",
                Url = new Uri("https://github.com/yfereidouni"),
            },
            License=new Microsoft.OpenApi.Models.OpenApiLicense()
            {
                Name = "MIT License",
                Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            }
        });
    // Adding XML Documentation to UI --------------------------------------------------
    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var cmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
    option.IncludeXmlComments(cmlCommentFullPath);
    //----------------------------------------------------------------------------------
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.AddAutoMapper(typeof(ParkyMappings));

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
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
    options.RoutePrefix = "";
});

app.UseAuthorization();

app.MapControllers();

app.Run();