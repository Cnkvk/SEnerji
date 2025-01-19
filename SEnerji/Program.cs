using DataBaseLayer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; // Giriþ yapýlmamýþsa yönlendirme yapýlacak sayfa
        options.AccessDeniedPath = "/Home";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Çerez süresi
        options.SlidingExpiration = true; // Kullaným süresine göre süreyi uzatma
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PersonelOnly", policy => policy.RequireClaim("Rol", "Personel"));
    options.AddPolicy("CustomerOnly", policy => policy.RequireClaim("Rol", "Customer"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
