using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BookShopDemo.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BookShopDemoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookShopDemoContext") ?? throw new InvalidOperationException("Connection string 'BookShopDemoContext' not found.")));
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(1); });
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UsersAccounts}/{action=Login}/{id?}");

app.Run();
