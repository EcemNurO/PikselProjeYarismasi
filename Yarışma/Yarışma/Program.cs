using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults
	.AuthenticationScheme)
	.AddCookie(opt =>
	{
		opt.LoginPath = "/Management/Account/Login";
	});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
//bu bir yorumdur

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
