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
                SelectedProjectCategoryId = judge?.ProjectCategoryId ?? 0
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
                // Profil bilgilerini güncelle
                profile.FullName = viewModel.Profile.FullName;
            profile.Email = viewModel.Profile.Email;
            profile.Phone = viewModel.Profile.Phone;
            profile.Address = viewModel.Profile.Address;
          



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
