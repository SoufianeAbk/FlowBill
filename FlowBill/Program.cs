using FlowBill.Data;
using FlowBill.Middleware;
using FlowBill.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/flowbill-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30
    )
    .CreateLogger();

try
{
    Log.Information("Starting FlowBill application");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    // Database Context
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Identity
    builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

    // Custom services
    builder.Services.AddScoped<IFacturatieService, FacturatieService>();
    builder.Services.AddScoped<IBackupService, BackupService>();

    // Add memory cache
    builder.Services.AddMemoryCache();

    // Add session
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    var app = builder.Build();

    // Seed database
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
            await DbInitializer.Initialize(services);
            Log.Information("Database initialized successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while seeding the database");
        }
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    // Custom error handling middleware
    app.UseErrorHandling();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSerilogRequestLogging();

    app.UseRouting();
    app.UseSession();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}