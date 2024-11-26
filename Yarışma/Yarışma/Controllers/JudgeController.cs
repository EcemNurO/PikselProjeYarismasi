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
                .Include(j => j.Univercity)
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

            var hasAssignedProject = db.ProjectEvaluations.Any(pe => pe.JudgeId == JudgeId);

            var model = new JudgeProfilViewModel
            {
                Profile = profile,
                JudgeCategories = db.JudgeCategories.ToList(),
                ProjectCategories = db.ProjectCategories.ToList(),
                UniversityList = db.univercities.ToList(),
                SelectedJudgeCategoryId = judge?.JudgeCategoryId ?? 0,
                SelectedProjectCategoryId = judge?.ProjectCategoryId ?? 0,
                Univercity = profile.Univercity?.UniversityName ?? string.Empty,
                WorkplaceName = profile.WorkplaceName ?? string.Empty,
                  HasAssignedProject = hasAssignedProject

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

            var judge = profile.Judge.FirstOrDefault();
            var judgeId = judge?.Id ?? 0;

      
            if (db.ProjectEvaluations.Any(pe => pe.JudgeId == judgeId))
            {
                TempData["ErrorMessage"] = "Hakeme proje atanmış, proje kategorisi değiştirilemez.";
                return RedirectToAction("JudgeProfile");
            }





            profile.FullName = viewModel.Profile.FullName;
            profile.Email = viewModel.Profile.Email;
            profile.Phone = viewModel.Profile.Phone;
            profile.Address = viewModel.Profile.Address;

            if (viewModel.SelectedJudgeCategoryId == 1) 
            {
               
                if (viewModel.Profile.Univercity?.Id > 0)
                {
                    profile.Univercity = db.univercities.FirstOrDefault(u => u.Id == profile.Univercity.Id);
                }
                else
                {
                    profile.Univercity = null;
                }

                profile.WorkplaceName = null; 
            }
            else if (viewModel.SelectedJudgeCategoryId == 2) 
            {
                profile.WorkplaceName = viewModel.WorkplaceName;
                profile.Univercity = null; 
            }

            var judgeToUpdate = profile.Judge.FirstOrDefault();
            if (judgeToUpdate != null && viewModel.SelectedProjectCategoryId > 0)
            {
                judgeToUpdate.ProjectCategoryId = viewModel.SelectedProjectCategoryId;
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
