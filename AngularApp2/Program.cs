using AngularApp2;
using Microsoft.AspNetCore.Authentication.Cookies;
using AutoMapper;
using AngularApp2.Models.Entity;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string con = "Host=localhost;Port=5432;Database=Diplomus;Username=postgres;Password=qwerty123456";



builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => //CookieAuthenticationOptions
        {
            options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        });
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DiplomusContext>(options => options.UseNpgsql(con), ServiceLifetime.Transient);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
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
    pattern: "{controller}/{action=Index}/{id?}");
//Scaffold-DbContext "Host=localhost;Port=5432;Database=diplomus;Username=postgres;Password=qwerty123456" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models/Entity -Tables "nutrients" -NoPluralize
app.MapFallbackToFile("index.html");

app.Run();

