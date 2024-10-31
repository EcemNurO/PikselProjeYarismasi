using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Login(LoginViewModel model)
        {
            if( !ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Email==model.Email
                && u.Password== model.Password);
                if ( user==null)               
                {
                    ViewBag.Message = "Böyle Bir Kullanıcı Bulunamadı";
                    return View(model);
                }
            }
            ViewBag.Message = "Lütfen Bilgilerini Eksiksiz Doldurun";
            return View(model);
        }
    }
}
