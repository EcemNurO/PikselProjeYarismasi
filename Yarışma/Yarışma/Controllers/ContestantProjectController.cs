using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Yarışma.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;
using ProjectModel = Yarışma.Models.Project;


namespace Yarışma.Controllers
{
    public class ContestantProjectController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();


        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }

            throw new Exception("Kullanıcı kimliği alınamadı.");
        }

        [Authorize]
        public IActionResult Project()
        {
            int userId = GetCurrentUserId();

            var contestantProfil = db.ContestantProfils
                .FirstOrDefault(cp => cp.usedContestantJudgeId == userId);
            if (contestantProfil == null)
            {
                throw new Exception("Kullanıcıya ait bir profil bulunamadı.");
            }

            var contestant = db.Contestants
                .Include(c => c.Projects)
                .FirstOrDefault(c => c.ContestantProfilId == contestantProfil.Id);
            if (contestant == null)
            {
                throw new Exception("Kullanıcıya ait bir yarışmacı kaydı bulunamadı.");
            }

            var project = db.Projects
                .Include(p => p.ProjectQuestions) // Eğer ilişkili soruları da yüklemek gerekiyorsa
                .FirstOrDefault(p => p.Id == contestant.Id);

            if (project == null)
            {
                project = new ProjectModel
                {
                    ContestantId = contestant.Id,
                    Name = "Yeni Proje",
                    ProjectQuestions = db.ProjectQuestions.ToList()
                };

                db.Projects.Add(project);
                db.SaveChanges();
            }
            else
            {
                var allQuestions = db.ProjectQuestions.ToList();
                foreach (var question in allQuestions)
                {
                    // Her sorunun Answers koleksiyonunu doldur
                    question.Answers = db.ProjectAnswers
                        .Where(a => a.ProjectId == contestant.Id && a.ProjectQuestionId == question.Id)
                        .ToList();

                    // Eğer proje bu soruyu içermiyorsa ekle
                    //if (!project.ProjectQuestions.Any(q => q.Id == question.Id))
                    //{
                    project.ProjectQuestions.Add(question);
                    //}
                }
                db.SaveChanges();
            }

            return View(project);



        }

       
        [HttpPost]
        public IActionResult SaveProject(int projectId, List<ProjectAnswer> answers, IFormFile file, string deleteFile, string projectName)
        {

            int userId = GetCurrentUserId();

            // Kullanıcının profilini bul
            var contestantProfil = db.ContestantProfils
                .FirstOrDefault(cp => cp.usedContestantJudgeId == userId);
            if (contestantProfil == null)
            {
                throw new Exception("Kullanıcıya ait bir profil bulunamadı.");
            }

            var contestant = db.Contestants
                .FirstOrDefault(c => c.ContestantProfilId == contestantProfil.Id);
            if (contestant == null)
            {
                throw new Exception("Kullanıcıya ait bir yarışmacı kaydı bulunamadı.");
            }

            // Projeyi ContestantId ve ProjectId kullanarak alın
            var project = db.Projects
        .Include(p => p.ProjectQuestions)
        .ThenInclude(q => q.Answers)
        .FirstOrDefault(p => p.Id == projectId && p.ContestantId == contestant.Id);

            if (project == null)
            {
                throw new Exception("Kullanıcıya ait proje bulunamadı.");
            }

            if (!string.IsNullOrEmpty(projectName))
            {
                project.Name = projectName;
                db.Entry(project).State = EntityState.Modified;
            }
            foreach (var answer in answers)
            {
                var existingAnswer = db.ProjectAnswers
                    .FirstOrDefault(a => a.ProjectId == project.Id && a.ProjectQuestionId == answer.ProjectQuestionId);
                if (existingAnswer != null)
                {
                    // Mevcut cevabı güncelle
                   
                    existingAnswer.Text = answer.Text ?? string.Empty;
                    db.Entry(existingAnswer).State = EntityState.Modified;
                }
                else
                {
                    // Yeni cevap ekle
                    db.ProjectAnswers.Add(new ProjectAnswer
                    {
                        ProjectId = project.Id,
                        ProjectQuestionId = answer.ProjectQuestionId,
                        Text = answer.Text ?? string.Empty
                    });
                }
            }

            db.SaveChanges();

            // Dosya işlemleri
            if (!string.IsNullOrEmpty(deleteFile) && deleteFile == "true")
            {
                if (!string.IsNullOrEmpty(project.FilePath))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", project.FilePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    project.FilePath = null;
                }
            }

            if (file != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                project.FilePath = "/files/" + uniqueFileName;
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    project.FileData = memoryStream.ToArray();
                }
            }

            db.SaveChanges();
            TempData["SuccessMessage"] = "Proje cevapları başarıyla kaydedildi.";
            return RedirectToAction("Project");
        }

        // Dosya yükler
        [HttpPost]
        public IActionResult UploadFile(IFormFile file, int projectId)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Lütfen geçerli bir dosya seçin.";
                return RedirectToAction("Project");
            }

            int userId = GetCurrentUserId();

            var project = db.Projects.FirstOrDefault(p => p.Id == projectId && p.ContestantId == userId);
            if (project == null)
            {
                TempData["ErrorMessage"] = "Proje bulunamadı.";
                return RedirectToAction("Project");
            }

            const long MaxFileSize = 10 * 1024 * 1024; // 10 MB
            if (file.Length > MaxFileSize)
            {
                TempData["ErrorMessage"] = "Dosya boyutu 10 MB'yi geçemez.";
                return RedirectToAction("Project");
            }

            var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".jpg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                TempData["ErrorMessage"] = "Sadece PDF, DOCX, XLSX, JPG veya PNG dosyalarına izin verilmektedir.";
                return RedirectToAction("Project");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            project.FilePath = "/files/" + uniqueFileName;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Dosya başarıyla yüklendi.";
            return RedirectToAction("Project");
        }
        [HttpPost]
        public IActionResult DeleteFile()
        {
            int userId = GetCurrentUserId();

            // Kullanıcının mevcut projesini al
            var project = db.Projects.FirstOrDefault(p => p.ContestantId == userId);

            if (project == null || string.IsNullOrEmpty(project.FilePath))
            {
                TempData["ErrorMessage"] = "Silinecek bir dosya bulunamadı.";
                return RedirectToAction("Project");
            }

            // Dosya yolunu belirle ve sunucudan sil
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", project.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Veritabanını güncelle
            project.FilePath = null;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Dosya başarıyla silindi.";
            return RedirectToAction("Project");
        }
    }
}
