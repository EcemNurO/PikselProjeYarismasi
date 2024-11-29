using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using Microsoft.Build.Evaluation;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class ContestantTableController : Controller
    {
        private readonly CompetitionDbContext db = new CompetitionDbContext();

        public IActionResult Index(string searchQuery, int page = 1, int pageSize = 10)
        {
            var filteredContestants = db.Contestants
         .Include(c => c.Projects)
             .ThenInclude(p => p.ProjectCategory)
         .Include(c => c.contestantProfil)
         .Include(c => c.ContestantCategory)
         .Where(c =>
             string.IsNullOrEmpty(searchQuery) ||
             c.contestantProfil.FullName.Contains(searchQuery) ||
             c.ContestantCategory.Name.Contains(searchQuery) ||
             c.Projects.Name.Contains(searchQuery) ||
             c.Projects.ProjectCategory.Name.Contains(searchQuery))
         .Skip((page - 1) * pageSize)
         .Take(pageSize)
         .ToList();

            var model = new ContestantTableVM
            {
                Contestants = filteredContestants.Select(c => new ContestantViewModel
                {
                    ContestantId = c.Id,
                    ContestantName = c.contestantProfil?.FullName,
                    ContestantCategoryName=c.ContestantCategory?.Name,
                    ProjectName = c.Projects?.Name,
                    ProjectCategoryName = c.Projects?.ProjectCategory?.Name,
                    AssignedAcademicJudgeName = db.ProjectEvaluations
                        .Where(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 1)
                        .Select(pe => pe.Judge.JudgeProfil.FullName)
                        .FirstOrDefault(),
                    AssignedIndustrialJudgeName = db.ProjectEvaluations
                        .Where(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 2)
                        .Select(pe => pe.Judge.JudgeProfil.FullName)
                        .FirstOrDefault(),
                    IsAcademicJudgeAssigned = db.ProjectEvaluations.Any(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 1),
                    IsIndustrialJudgeAssigned = db.ProjectEvaluations.Any(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 2),
                }).ToList(),
                TotalContestants = filteredContestants.Count(),
                PageSize = pageSize,
                CurrentPage = page,
                SearchQuery = searchQuery
            };
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var contestant = db.Contestants
        .Include(c => c.Projects)
            .ThenInclude(p => p.ProjectCategory)
        .Include(c => c.Projects)
            .ThenInclude(p => p.ProjectAnswers)
                .ThenInclude(q => q.Question)
        .Include(c => c.contestantProfil)
        .Include(c => c.ContestantCategory)
        .FirstOrDefault(c => c.Id == id);

            if (contestant == null)
            {
                return NotFound();
            }


            return View(contestant);
        }
        public IActionResult DownloadFile(int projectId)
        {
            var project = db.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null || string.IsNullOrEmpty(project.FilePath))
            {
                TempData["ErrorMessage"] = "Proje dosyası bulunamadı.";
                return RedirectToAction("Index");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", Path.GetFileName(project.FilePath));
            if (!System.IO.File.Exists(filePath))
            {
                TempData["ErrorMessage"] = "Dosya fiziksel olarak mevcut değil.";
                return RedirectToAction("Index");
            }

            return PhysicalFile(filePath, "application/octet-stream", Path.GetFileName(project.FilePath));
        }
        public IActionResult AssignJudges(int id)
        {

            var contestant = db.Contestants
        .Include(c => c.contestantProfil)
        .Include(c => c.Projects)
            .ThenInclude(p => p.ProjectCategory)
        .FirstOrDefault(c => c.Id == id);

            if (contestant == null || contestant.Projects == null)
            {
                TempData["ErrorMessage"] = "Yarışmacıya ait proje bulunamadı.";
                return RedirectToAction("Index");
            }

            var academicJudges = db.Judges
                .Include(j => j.JudgeProfil)
                .Where(j => j.JudgeCategory.Name == "Akademisyen Hakem" &&
                            j.ProjectCategoryId == contestant.Projects.ProjectCategoryId)
                .ToList();

            var industrialJudges = db.Judges
                .Include(j => j.JudgeProfil)
                .Where(j => j.JudgeCategory.Name == "Sanayici Hakem" &&
                            j.ProjectCategoryId == contestant.Projects.ProjectCategoryId)
                .ToList();

            var model = new AssignJudgesVM
            {
                ContestantId = id,
                ContestantName = contestant.contestantProfil.FullName,
                ProjectId = contestant.Projects.Id, // Proje ID'sini ekliyoruz
                ProjectName = contestant.Projects.Name,
                AcademicJudges = academicJudges,
                IndustrialJudges = industrialJudges
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult AssignJudgeToProject(int projectId, int judgeCategoryId, List<int> selectedJudges)
        {


            if (projectId <= 0 || judgeCategoryId <= 0)
            {
                TempData["ErrorMessage"] = "Atama işlemi için gerekli bilgiler eksik.";
                return RedirectToAction("AssignJudges", new { id = projectId });
            }

            // Eğer hiç hakem seçilmediyse bile işlem devam eder
            if (selectedJudges == null || !selectedJudges.Any())
            {
                TempData["Message"] = "Hakem seçilmedi. Ancak işlem tamamlandı.";
                return RedirectToAction("AssignJudges", new { id = projectId });
            }

            foreach (var judgeId in selectedJudges)
            {
                // Aynı proje ve hakem kombinasyonunun daha önce atanıp atanmadığını kontrol et
                var existingAssignment = db.ProjectEvaluations
                    .FirstOrDefault(pe => pe.ProjectId == projectId && pe.JudgeId == judgeId);

                if (existingAssignment == null)
                {
                    // Yeni atama kaydı oluştur
                    var assignment = new ProjectEvaluation
                    {
                        ProjectId = projectId,
                        JudgeId = judgeId,
                        JudgeCategoryId = judgeCategoryId,
                        Status = true,
                        Deleted = false
                    };

                    db.ProjectEvaluations.Add(assignment);
                }
            }

            db.SaveChanges();
            TempData["Message"] = "Hakem atamaları başarıyla kaydedildi.";
            return RedirectToAction("AssignJudges", new { id = projectId });

        }


        public IActionResult ExportToExcel()
        {
            var contestants = db.Contestants
        .Include(c => c.contestantProfil)
        .Include(c => c.ContestantCategory)
        .Include(c => c.Projects)
        .ThenInclude(p => p.ProjectCategory)
        .ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Yarışmacılar");

                // Başlıkları yazıyoruz
                worksheet.Cells[1, 1].Value = "Yarışmacı Adı";
                worksheet.Cells[1, 2].Value = "Yarışmacı Kategorisi";
                worksheet.Cells[1, 3].Value = "Proje Adı";
                worksheet.Cells[1, 4].Value = "Proje Kategorisi";

                // Verileri yazıyoruz
                int row = 2;
                foreach (var contestant in contestants)
                {
                    worksheet.Cells[row, 1].Value = contestant.contestantProfil?.FullName ?? "Bilinmiyor";
                    worksheet.Cells[row, 2].Value = contestant.ContestantCategory?.Name ?? "Bilinmiyor";
                    worksheet.Cells[row, 3].Value = contestant.Projects?.Name ?? "Bilinmiyor"; // Proje adı
                    worksheet.Cells[row, 4].Value = contestant.Projects?.ProjectCategory?.Name ?? "Bilinmiyor"; // Proje kategorisi
                    row++;
                }

                // Stil ayarlamaları
                worksheet.Cells[1, 1, row - 1, 4].AutoFitColumns(); // Tüm sütunları otomatik sığdır
                worksheet.Cells[1, 1, 1, 4].Style.Font.Bold = true; // Başlıkları kalın yap

                // Excel dosyasını byte dizisi olarak alıyoruz
                var fileContents = package.GetAsByteArray();

                // Excel dosyasını indirme olarak sunuyoruz
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Yarismacilar.xlsx");
            }






        }
    }
}


