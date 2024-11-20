using DinkToPdf.Contracts;
using DinkToPdf;
using Kariyer.Models;
using Kariyer.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache(); // Oturum için gerekli bellek içi önbelleði ekler

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman aþýmý süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<KariyerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

// Kimlik doðrulama ve yetkilendirme yapýlandýrmasý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Giris";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsArayan", policy =>
        policy.RequireClaim("KullaniciTur", "IsArayan"));

    options.AddPolicy("IsVeren", policy =>
        policy.RequireClaim("KullaniciTur", "IsVeren"));
});

builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
var app = builder.Build();

// Hata iþleme ve HTTPS yönlendirme
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Anasayfa}/{id?}");

app.MapControllerRoute(
    name: "isArayan",
    pattern: "{controller=IsArayan}/{action=Index}/{id?}");



app.Run();
