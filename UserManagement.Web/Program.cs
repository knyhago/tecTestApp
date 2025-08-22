using Microsoft.EntityFrameworkCore;
using Serilog;
using UserManagement.Data;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("AzureSqlDb");


builder.Services.AddCors();
builder.Services
    .AddApplicationDb(connectionString!,builder.Environment)
    .AddDomainServices()
    .AddMarkdown()
    .AddControllersWithViews();
builder.Services.AddControllers();

    
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();
app.UseMarkdown();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();

if (context.Database.IsRelational())
{
    context.Database.Migrate();
}
else
{
    DataContextSeed.Seed(context); // ✅ seed InMemory
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
