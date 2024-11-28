using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class ApproveJudgeController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public IActionResult AproveJudge(int page = 1, int pageSize = 10)
        {
            var totalCount = db.Judges.Count(j => !j.IsApproved && j.Status == false && j.Deleted == true);

            // Onay bekleyen hakemleri getir
            var judges = db.Judges
                .Include(j => j.JudgeProfil)
                  .ThenInclude(p => p.Univercity)
                .Include(j => j.JudgeCategory)
                .Include(j => j.ProjectCategory)
                .Where(j => !j.IsApproved && j.Status == false && j.Deleted == true)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Hakem ve proje kategorilerini getir
            var judgeCategories = db.JudgeCategories.ToList();
            var projectCategories = db.ProjectCategories.ToList();

            // Model oluştur
            var judgeItemViewModels = judges.Select(j => new JudgeItemViewModel
            {
                JudgeId = j.Id,
                FullName = j.JudgeProfil?.FullName,
                Email = j.JudgeProfil?.Email,
                Phone = j.JudgeProfil?.Phone,
                UniversityOrWorkplace = j.JudgeProfil?.Univercity != null
            ? j.JudgeProfil.Univercity.UniversityName // Akademisyen
            : (!string.IsNullOrEmpty(j.JudgeProfil?.WorkplaceName)
                ? j.JudgeProfil.WorkplaceName // Sanayici
                : "Belirtilmemiş"),
                JudgeCategoryName = j.JudgeCategory?.Name,
                ProjectCategoryName = j.ProjectCategory?.Name ?? "Belirtilmemiş",
                IsApproved = j.IsApproved
            }).ToList();

            // ViewModel'i doldur
            var model = new JudgeViewModel
            {
                Judges = judges, // Ham Judge listesi
                JudgeItemViewModels = judgeItemViewModels, // Hazırlanmış ViewModel listesi
                JudgeCategories = db.JudgeCategories.ToList(),
                ProjectCategories = db.ProjectCategories.ToList(),
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };

            return View(model);

        }
           
        
        [HttpPost]
        public IActionResult ApproveJudge(int judgeId)
        {
            var judge = db.Judges
        .Include(j => j.JudgeProfil)
        .Include(j => j.JudgeProfil.UsedContestantJudges)
        .FirstOrDefault(j => j.Id == judgeId);

            if (judge == null)
            {
                TempData["ErrorMessage"] = "Hakem bulunamadı.";
                return RedirectToAction("AproveJudge");
            }

            // Hakem kaydını onayla
            judge.IsApproved = true;
            judge.Status = true;
            judge.Deleted = false;
            judge.IsAssigned = true;

            // Hakemin profilini güncelle
            if (judge.JudgeProfil != null)
            {
                judge.JudgeProfil.Status = true;
                judge.JudgeProfil.Deleted = false;

                // Hakemin kullanıcı kaydını güncelle
                if (judge.JudgeProfil.UsedContestantJudges != null)
                {
                    judge.JudgeProfil.UsedContestantJudges.Status = true; // Kullanıcının durumunu etkinleştir
                    judge.JudgeProfil.UsedContestantJudges.Deleted = false; // Kullanıcıyı etkinleştir
                }
            }

            db.SaveChanges();

            TempData["SuccessMessage"] = "Hakem başarıyla onaylandı.";
            return RedirectToAction("AproveJudge");
        }
    }
    
}
