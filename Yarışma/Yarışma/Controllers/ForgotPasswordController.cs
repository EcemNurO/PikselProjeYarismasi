using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Net.Mail;
using Yarışma.Models;
using Yarışma.Services;

namespace Yarışma.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly UserService _userService;
        private readonly ICustomEmailService _emailService;
        public  ForgotPasswordController (UserService userService ,ICustomEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var token = await _userService.CreatePasswordResetTokenAsync(email);

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.ErrorMessage = "E-posta adresi bulunamadı.";
                return View();
            }

            var resetLink = Url.Action("ResetPassword", "ForgotPassword", new { token }, Request.Scheme);
            // E-posta gönderim işlevini burada çağırabilirsiniz.
            await _emailService.SendResetPasswordEmail(email, resetLink);
            ViewBag.Message = "Şifre sıfırlama bağlantısı gönderildi.";
            return View();
        }
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                ViewBag.ErrorMessage = "Geçersiz veya eksik bir token.";
                return View("Error");
            }

            var model = new ResetPasswordModel { Token = token }; // Token'ı formda saklayın
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Token doğrulama
            var user = await _userService.ValidateResetTokenAsync(model.Token);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Geçersiz veya süresi dolmuş token.";
                return View("Error");
            }

            // Yeni şifreyi kaydet
            await _userService.UpdatePasswordAsync(user.Id, model.NewPassword);

            ViewBag.Message = "Şifreniz başarıyla sıfırlandı.";
            return RedirectToAction("Login", "Account");
        }

    }
}
      
