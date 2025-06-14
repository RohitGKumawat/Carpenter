using Carpenter.Models;
using Microsoft.EntityFrameworkCore;
using DinkToPdf;
using DinkToPdf.Contracts;
using Carpenter.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSession();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();


builder.Services.AddDbContext<EverytDB>(options =>
{
    options.UseSqlServer("Server=LAPTOP-5L2MAI1U\\CARPENTERDBSQL;Database=CarpenterDB;Trusted_Connection=True;TrustServerCertificate=True;");
});

// Register DinkToPdf and your service
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<PdfService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailService>();


var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/libwkhtmltox/libwkhtmltox.dll"));


var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
