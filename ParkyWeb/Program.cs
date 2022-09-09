using Microsoft.AspNetCore.Authentication.Cookies;
using ParkyWeb;
using ParkyWeb.Repository;
using ParkyWeb.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        //If authorization required and user not logined then redirect here
        options.LoginPath = "/Home/Login";
        //If user logined but not authorized then redirect here
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.SlidingExpiration = true;
    });
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.AddScoped<ITrailRepository, TrailRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//Without this line you get this error :---------------------------------------
//Unable to resolve service for type 'System.Net.Http.HttpClient'--------------
builder.Services.AddHttpClient<SD>(options =>
{
    options.BaseAddress = new Uri(SD.APIBaseUrl);
});
//-----------------------------------------------------------------------------

//Activating Session for storing JWT ------------------------------------------
builder.Services.AddSession(options =>
{
    //Set a short timeout for easy testing
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    //Make the session cookie essential
    options.Cookie.IsEssential = true;
});
//-----------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Cross-Origin-Resource-Sharing: ---------------------------------------------------
//Accepting request from one origin to access resources in another origin ----------
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
//----------------------------------------------------------------------------------

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
