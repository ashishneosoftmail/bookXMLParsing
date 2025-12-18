using BookXMLParsing.Application;
using BookXMLParsing.Persistence;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logPath = Path.Combine(builder.Environment.WebRootPath, "Logs");
// Create Logs folder if it doesn't exist
if (!Directory.Exists(logPath))
{
    Directory.CreateDirectory(logPath);
}

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File(
        path: Path.Combine(logPath, "Log_.txt"),
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,   // Creates a new file daily
        rollOnFileSizeLimit: true,             // Optional: create new file if size exceeds limit
        retainedFileCountLimit: 30             // Optional: keep last 30 log files
    )
    .MinimumLevel.Information()               // Adjust as needed
    .CreateLogger();

builder.Host.UseSerilog(); // Replace default logger with Serilog

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddInterfaceServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BookXMLParsing",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"))
   .ExcludeFromDescription();

app.Run();
