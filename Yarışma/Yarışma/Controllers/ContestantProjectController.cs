using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Yarışma.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Project = Yarışma.Models.Project;

namespace Yarışma.Controllers
{
    public class ContestantProjectController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();

       

        // Kullanıcı kimliğini alır ve geçerli bir ID olup olmadığını kontrol eder
        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }

            throw new Exception("Kullanıcı kimliği alınamadı.");
        }

        
        public IActionResult Project()
        {
            int userId = GetCurrentUserId();

            // Kullanıcının profilini bul
            var contestantProfil = db.ContestantProfils
                .FirstOrDefault(cp => cp.usedContestantJudgeId == userId);
            if (contestantProfil == null)
            {
                throw new Exception("Kullanıcıya ait bir profil bulunamadı.");
            }

            // Kullanıcının Contestant kaydını bul
            var contestant = db.Contestants
                .Include(c => c.Projects)
                .FirstOrDefault(c => c.ContestantProfilId == contestantProfil.Id);
            if (contestant == null)
            {
                throw new Exception("Kullanıcıya ait bir yarışmacı kaydı bulunamadı.");
            }

            // Yarışmacının mevcut projesini al
            var project = db.Projects
                .Include(p => p.ProjectQuestions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefault(p => p.ContestantId == contestant.Id);

            // Eğer proje yoksa, yeni bir proje oluştur
            if (project == null)
            {
                project = new Project
                {
                    ContestantId = contestant.Id,
                    Name = "Yeni Proje",
                    ProjectQuestions = db.ProjectQuestions.ToList() // Tüm soruları yeni projeye ekle
                };

                db.Projects.Add(project);
                db.SaveChanges();
            }
            else
            {
                // Mevcut projeye eksik soruları ekle
                var allQuestions = db.ProjectQuestions.ToList();

                foreach (var question in allQuestions)
                {
                    if (!project.ProjectQuestions.Any(q => q.Id == question.Id))
                    {
                        // Aynı nesneyi tekrar oluşturmak yerine mevcut nesneyi kullan
                        project.ProjectQuestions.Add(question);
                    }
                }


                db.SaveChanges();
            }

            // Projeyi Görünüme Gönder
            return View(project);
        }

       
        [HttpPost]
        public IActionResult SaveProject(int projectId, List<ProjectAnswer> answers)
        {
            Console.WriteLine("Samet");
            int userId = GetCurrentUserId();

            // Kullanıcının profilini ve yarışmacısını bul
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

            // Projeyi al
            var project = db.Projects
                .FirstOrDefault(p =>  p.ContestantId == contestant.Id);
            if (project == null)
            {
                throw new Exception("Proje bulunamadı.");
            }
            if (answers == null || !answers.Any())
            {
                answers = db.ProjectQuestions.Select(q => new ProjectAnswer
                {
                    ProjectId = project.Id,
                    ProjectQuestionId = q.Id,
                    Text = string.Empty // Boş cevap
                }).ToList();
            }
            // Cevapları kaydet veya güncelle
            foreach (var answer in answers)
            {
                var existingAnswer = db.ProjectAnswers
                    .FirstOrDefault(a => a.ProjectId == project.Id && a.ProjectQuestionId == answer.ProjectQuestionId);

                if (existingAnswer != null)
                {
                    // Mevcut cevabı güncelle
                    existingAnswer.Text = answer.Text ?? string.Empty;
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

            return RedirectToAction("Project");
        }

        // Dosya yükler
        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null)
            {
                TempData["ErrorMessage"] = "Dosya seçilmedi.";
                return RedirectToAction("Project");
            }

            int userId = GetCurrentUserId();
            var project = db.Projects.FirstOrDefault(p => p.ContestantId == userId);

            if (project != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                project.FilePath = "/files/" + file.FileName;
                db.SaveChanges();
            }
            else
            {
                TempData["ErrorMessage"] = "Proje bulunamadı.";
            }

            return RedirectToAction("Project");
        }
    }
}
