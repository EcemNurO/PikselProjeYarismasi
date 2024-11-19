using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Yarışma.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;

public class ContestController : Controller
{
    CompetitionDbContext db = new CompetitionDbContext();



    [Authorize(Roles = "Contestant")]
    public IActionResult Profile(int id)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            TempData["ErrorMessage"] = "Kullanıcı kimliği bulunamadı.";
            return RedirectToAction("Login", "Account");
        }

        var profile = db.ContestantProfils.Include(p => p.Contestants)
                                       .FirstOrDefault(p => p.usedContestantJudgeId == int.Parse(userId));
        if (profile == null)
        {
            TempData["ErrorMessage"] = "Profil bulunamadı.";
            return RedirectToAction("Index", "Home");
        }
        var contestant = profile.Contestants.FirstOrDefault();

        var contestantId = profile.Contestants.FirstOrDefault()?.Id ?? 0;


        var selectedProjectCategoryId = db.Projects
            .Where(p => p.ContestantId == contestantId)
            .Select(p => p.ProjectCategoryId)
            .FirstOrDefault();
    
        var model = new ProfileViewModel
        {
            Profile = profile , 
            ContestantCategories = db.ContestantCategories.ToList(),
            ProjectCategories = db.ProjectCategories.ToList(),
            SelectedContestantCategoryId = profile.Contestants.FirstOrDefault()?.ContestantCategoryId ?? 0,
            SelectedProjectCategoryId = db.Projects.FirstOrDefault(p => p.ContestantId == profile.Id)?.ProjectCategoryId ?? 0
        };
        return View(model);



    }

       [Authorize]
       [HttpPost]
         public async Task<IActionResult> UpdateProfile(ProfileViewModel viewModel)
 {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
        {
            TempData["ErrorMessage"] = "Kullanıcı kimliği bulunamadı.";
            return RedirectToAction("Login", "Account");
        }

 
        var profile = db.ContestantProfils.Include(p => p.Contestants)
        .FirstOrDefault(p => p.usedContestantJudgeId == parsedUserId);
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

        // Diğer alanları güncelle
        profile.FullName = viewModel.Profile.FullName;
        profile.Email = viewModel.Profile.Email;
        profile.Age = viewModel.Profile.Age;
        profile.Phone = viewModel.Profile.Phone;
        profile.Univercity = viewModel.Profile.Univercity;
        profile.Address = viewModel.Profile.Address;



        //// Yarışmacı kategorisi güncelleme
        //var contestant = profile.Contestants.FirstOrDefault();
        //if (contestant != null)
        //{
        //    contestant.ContestantCategoryId = viewModel.SelectedContestantCategoryId;
        //}

        //// Proje kategorisi güncelleme
        //var project = db.Projects.FirstOrDefault(p => p.ContestantId == contestant.Id);
        //if (project != null)
        //{
        //    project.ProjectCategoryId = viewModel.SelectedProjectCategoryId;
        //}

        // Veritabanında değişiklikleri kaydet
        await db.SaveChangesAsync();
        TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";
        return RedirectToAction("Profile");
     }
}
