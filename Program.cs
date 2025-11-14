using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var rootPath = app.Environment.WebRootPath;

if (string.IsNullOrEmpty(rootPath)) rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
// Ensure the "media" folder exists in wwwroot, create it if missing
var videoFilePath = Path.Combine(rootPath, "media");
if (!Directory.Exists(videoFilePath))
{
    Directory.CreateDirectory(videoFilePath);
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(videoFilePath),
    RequestPath = "/media"
});

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapControllers();
app.Run();
