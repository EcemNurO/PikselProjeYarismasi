using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
	[Area("Management")]
	public class AccountController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public IActionResult Login()
        {
            ViewBag.Message = "";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if( ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Email==model.Email
                                               && u.Password== model.Password
                                               && u.Status
                                               && u.Deleted== false);
                if ( user==null)               
                {
                    ViewBag.Message = "Böyle Bir Kullanıcı Bulunamadı";
                    return View(model);
                }
                var claims = new List<Claim>
                {
                    new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim (ClaimTypes.Email, user.Email),
                    //new Claim(ClaimTypes.Role, user.Role),

                };
                var claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    //ne zaman sisteme girmiş
                    IssuedUtc = DateTime.UtcNow,
                    // kaç saat acık kalacak
                    ExpiresUtc = DateTime.UtcNow.AddHours(6),
                     
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Message = "Lütfen Bilgilerini Eksiksiz Doldurun";
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
