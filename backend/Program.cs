using backend.Data;
using backend.Services;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Serilog Config
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Loglari konsolda göster
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Her gun icin log dosyasi
    .Enrich.FromLogContext()    // Ek bilgileri ekle
    .CreateLogger();

builder.Host.UseSerilog();

// DB Service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Zamanlamali background exchangerate service
builder.Services.AddHostedService<ExchangeRateBackgroundService>();

// Dependency Injection (DI)
builder.Services.AddHttpClient<ExchangeRateService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubcategoryService, SubcategoryService>();
builder.Services.AddScoped<IPriceService, PriceService>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Apply Cors Policy
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Global Logging Middleware
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    // Request bilgilerinin loglanmasi
    logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);

    await next(); // next

    // Response bilgilerinin loglanmasi
    logger.LogInformation("Response status: {StatusCode}", context.Response.StatusCode);
});

app.Run();
