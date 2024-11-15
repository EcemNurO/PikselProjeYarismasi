using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class AccountAdminController : Controller
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
            if (ModelState.IsValid)
            {
                var User = db.Users.FirstOrDefault(u => u.Email == model.Email
                                                        && u.Password == model.Password
                                                        && u.Status
                                                        && u.Deleted == false
                                                        );

                if (User == null)
                {
                    ViewBag.Message = "Böyle Bir Kullanıcı Bulunamadı";
                    return View(model);
                }
                var claims = new List<Claim>
                {
                    new Claim (ClaimTypes.NameIdentifier, User.Id.ToString()),
                    new Claim(ClaimTypes.Name, User.FullName),
                    new Claim (ClaimTypes.Email, User.Email),


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
