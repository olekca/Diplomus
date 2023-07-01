using AngularApp2;
using Microsoft.AspNetCore.Authentication.Cookies;
using AutoMapper;
using AngularApp2.Models.Entity;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using AngularApp2.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string con = "Host=localhost;Port=5432;Database=Diplomus;Username=postgres;Password=qwerty123456";



builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => //CookieAuthenticationOptions
        {
            options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        });
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DiplomusContext>(options => options.UseNpgsql(con), ServiceLifetime.Transient);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {

            //you can configure your custom policy
            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});
var app = builder.Build();
app.UseCors();
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
//Scaffold-DbContext "Host=localhost;Port=5432;Database=diplomus;Username=postgres;Password=qwerty123456" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models/Entity -Tables "nutrients","daily_diet", "needs", "needs_nutrients", "products", "products_nutrients", "recipes", "recipes_products","users" -NoPluralize
app.MapFallbackToFile("index.html");

app.Run();

