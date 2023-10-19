using CustomerSearch.Data;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Everest.BackgroundWorker.01.WebApi",
        Description = "WebApi para crear tareas en tareas en segundo plano en el BackgroundWorker.01",
    });
});

builder.Services.AddScoped<DapperDbContext>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
        {
            SchemaName = "Jobs01",
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

builder.Services.AddHangfireServer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();
app.MapHangfireDashboard("/jobs", new DashboardOptions()
{
    DashboardTitle = "Everest Jobs",
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
