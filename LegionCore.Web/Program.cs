using System.Text;
using LegionCore.Core.Identity;
using LegionCore.Core.Models.Identity;
using LegionCore.Infrastructure.Data;
using LegionCore.Infrastructure.Helpers.Interfaces;
using LegionCore.Infrastructure.Helpers.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


//# Initialize Builder

var builder = WebApplication.CreateBuilder(args);
//# Set custom path for appssettings.json

var appSettingsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "../LegionCore.Infrastructure/Data/appsettings.json");
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile(appSettingsDirectory, optional: true, reloadOnChange: true);
});

//# Setup SQL Connection

var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//# Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection." + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//# Configure Identity

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequiredLength = 1;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

//# Add JWT Authentication Service
builder.Services.AddAuthentication()
    .AddCookie()
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
        };
    });


//# Add DI // Seeders

builder.Services.Scan(scan => scan
    .FromAssemblyOf<IService>()
    .AddClasses()
    .AsSelf()
    .AsImplementedInterfaces()
    .WithTransientLifetime());


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


var app = builder.Build();

//# Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


//# Running the Seeders

var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
using (var scope = scopedFactory.CreateScope())
{
    var seederService = scope.ServiceProvider.GetService<ApplicationSeederService>();
    seederService.SeedAsync();

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();