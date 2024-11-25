using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Yarışma.Models;

namespace Yarışma.Controllers
{
    
    public class JudgeController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();

        [Authorize(Roles = "Judge")]
        public IActionResult JudgeProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Kullanıcı kimliği bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            
            var profile = db.JudgeProfils
                .Include(j => j.Judge) 
                .ThenInclude(j=>j.JudgeCategory)
                .FirstOrDefault(p => p.UsedContestantJudgeId == int.Parse(userId));

            if (profile == null)
            {
                TempData["ErrorMessage"] = "Profil bulunamadı.";
                return RedirectToAction("Index", "Home");
            }
            var judge = profile.Judge.FirstOrDefault();
            var JudgeId = profile.Judge.FirstOrDefault()?.Id ?? 0;



            var model = new JudgeProfilViewModel
            {
                Profile = profile,
                JudgeCategories = db.JudgeCategories.ToList(),
                ProjectCategories = db.ProjectCategories.ToList(),
                SelectedJudgeCategoryId = judge?.JudgeCategoryId ?? 0,
                SelectedProjectCategoryId = judge?.ProjectCategoryId ?? 0,
                Univercity = profile.Univercity != null ? profile.Univercity.UniversityName : string.Empty,
                WorkplaceName = profile.WorkplaceName ?? string.Empty
            };


            return View(model);
        }

    
        [HttpPost]
        public async Task<IActionResult> UpdateJudgeProfile(JudgeProfilViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kullanıcı kimliği kontrolü
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
            {
                TempData["ErrorMessage"] = "Kullanıcı kimliği bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            // Hakeme ait profil bilgilerini getir

            var profile = db.JudgeProfils
                .Include(j => j.Judge)  
                .FirstOrDefault(p => p.UsedContestantJudgeId == int.Parse(userId));

            if (profile == null)
            {
                TempData["ErrorMessage"] = "Profil bulunamadı.";
                return RedirectToAction("Index", "Home");
            }
            // Resim dosyasını işleme
            if (viewModel.imageFile != null && viewModel.imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "images");
                Directory.CreateDirectory(uploadsFolder); // Dizin yoksa oluştur

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.imageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.imageFile.CopyToAsync(fileStream);
                }

                // ContestantProfil modelindeki image alanını güncelle
                profile.image = "/images/" + uniqueFileName;

            }
            profile.FullName = viewModel.Profile.FullName;
            profile.Email = viewModel.Profile.Email;
            profile.Phone = viewModel.Profile.Phone;
            profile.Address = viewModel.Profile.Address;

            // Hakem kategorisine göre Üniversite veya İş Yeri Adı güncellemesi
            if (viewModel.SelectedJudgeCategoryId == 1) // Akademisyen Hakem
            {
                // Üniversite bilgisi için kontrol ve atama
                if (profile.Univercity == null)
                {
                    profile.Univercity = new Univercity(); // Eğer null ise yeni bir nesne oluştur
                }
                profile.Univercity.UniversityName = viewModel.Univercity;

                // Sanayici bilgisi boşaltılır
                profile.WorkplaceName = null;
            }
            else if (viewModel.SelectedJudgeCategoryId == 2) // Sanayici Hakem
            {
                // İş yeri bilgisi güncellenir
                profile.WorkplaceName = viewModel.WorkplaceName;

                // Üniversite bilgisi boşaltılır
                profile.Univercity = null;
            }





            //var judge = profile.Judge.FirstOrDefault();
            //if (judge != null)
            //{
            //    judge.JudgeCategoryId = viewModel.SelectedJudgeCategoryId;
            //    judge.ProjectCategoryId = viewModel.SelectedProjectCategoryId;
            //}

            // Veritabanında değişiklikleri kaydet
            await db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";
            return RedirectToAction("JudgeProfile");
        }

    }
}
