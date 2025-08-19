using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UserManagement.Data;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("AzureSqlDb");


builder.Services.AddCors();
builder.Services
    .AddDataAccess(connectionString!)
    .AddDomainServices()
    .AddMarkdown()
    .AddControllersWithViews();
builder.Services.AddControllers();

    
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()    // logs to console
    //.WriteTo.MSSqlServer(connectionString, "Logs") // optional DB logging
    .CreateLogger();

var app = builder.Build();

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseMarkdown();

app.Services.MigrateDb();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
