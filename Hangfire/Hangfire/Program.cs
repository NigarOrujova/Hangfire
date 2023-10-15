using Hangfire;
using Hangfire.Controllers;
using Hangfire.DataContext;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(cfg =>
{
    cfg.UseNpgsql(builder.Configuration.GetConnectionString("cString"));
});
builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("cString")));

builder.Services.AddHangfireServer();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = " Hangfire BackgroundJobs Dashboard",
    Authorization = new[]
           {
                new HangfireCustomBasicAuthenticationFilter()
                {
                    Pass = "password",
                    User = "username"
                }
            }
});
app.MapHangfireDashboard();
RecurringJob.AddOrUpdate<InfosController>(s => s.UpdateStatusDaily(), Cron.Minutely);

app.Run();
