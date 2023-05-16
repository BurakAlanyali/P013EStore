using P013EStore.Data;
using P013EStore.Service.Abstract;
using P013EStore.Service.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies; //Oturum iþlemleri için

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddTransient(typeof(IService<>), typeof(Service<>)); // kendi yazdýðýmýz db iþlemlerini yapan servisi .net core da bu þekilde mvc projesine service olarak tanýtýyoruz ki kullanabilelim.
builder.Services.AddTransient<IProductService, ProductService>(); // Product için yazdýðýmýz service leri kullanabilmek için projeye tanýtýyoruz.

//Uygulama admin paneli için oturum açma ayarlarý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login"; // oturum açmayan kullanýcýlarýn gönderileceði adres
    x.LogoutPath = "/Admin/Logout";
    x.AccessDeniedPath = "/AccessDenied"; // yetkilendirme ile ekrana eriþim hakký olmayan kullanýcýlarýn gönderileceði sayfa
    x.Cookie.Name = "Administator";//Oluþacak cookie ismi
    x.Cookie.MaxAge = TimeSpan.FromDays(1);//oluþacak cookie nin yaþam süresi

}); //Oturum iþlemleri için
//Uygulama admin paneli için yetkilendirme ayarlarý
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", p=>p.RequireClaim("Role","Admin")); // admin paneline giriþ yapma yetkisine sahip olanlarý bu kuralla kontrol edeceðiz.
    x.AddPolicy("UserPolicy", p=>p.RequireClaim("Role","User"));// admin dýþýnda yetkilendirme kullanýrsak bu kuralý kullanabiliriz(siteye üye giriþi yapanlarý ön yüzde bir panele eriþtirme gibi)
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //Dikkat! önce useAuthentication satýrý gelmeli sonra useAuthorization
app.UseAuthorization();

app.MapControllerRoute(
            name: "admin",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
