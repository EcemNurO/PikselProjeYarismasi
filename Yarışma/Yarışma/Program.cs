using Microsoft.AspNetCore.Authentication.Cookies;
using Yarýþma.Middleware;
using Yarýþma.Models; 
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System.Configuration;
using Yarýþma.Repositories;
using Yarýþma.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TokenRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ICustomEmailService, CustomEmailService>();





builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults
	.AuthenticationScheme)
	.AddCookie(opt =>
	{
		opt.LoginPath = "/Account/Login";
        opt.AccessDeniedPath = "/Account/AccessDenied";
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

app.UseEndpoints(endpoints =>
{
	endpoints.MapAreaControllerRoute(
	name: "Management",
	areaName: "Management",
	pattern: "Management/{controller=Dashboard}/{action=Index}/{id?}"
	);

	endpoints.MapControllerRoute(
	  name: "areas",
	  pattern: "{area:exists}/{controller}/{action}"
	);


	endpoints.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
