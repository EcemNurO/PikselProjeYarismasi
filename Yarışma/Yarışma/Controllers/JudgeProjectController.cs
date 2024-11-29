using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Yarışma.Models;

namespace Yarışma.Controllers
{
    //[Authorize(Roles = "Judge")]
    public class JudgeProjectController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();


        public IActionResult AssignedProjects()
        {



            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Kullanıcı kimliği bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            // Hakem bilgisi al
            var judge = db.Judges
                .Include(j => j.JudgeProfil)
                .Include(j=>j.JudgeCategory)
                .FirstOrDefault(j => j.JudgeProfil.UsedContestantJudgeId == int.Parse(userId));

            if (judge == null)
            {
                TempData["ErrorMessage"] = "Hakem bilgisi bulunamadı.";
                return RedirectToAction("Index", "Home");
            }

            // Hakeme atanmış projeleri al
            var projectIds = db.ProjectEvaluations
                 .Include(pe => pe.Judge)
                 .Include(pe=>pe.JudgeCategory)
                .Where(pe => pe.JudgeId == judge.Id && pe.JudgeCategoryId == judge.JudgeCategoryId)
                .Select(pe => pe.ProjectId)
                .ToList();

            // Tüm projeleri ve ilişkili sorularla cevapları getir
            var projects = db.Projects
                .Include(p=>p.Contestant)
                .Where(p => projectIds.Contains(p.Id))
                .Select(p => new AssignedProjectViewModel
                {
                    ProjectId = p.Id,
                    ProjectName = p.Name,
                    ProjectCategory = p.ProjectCategory != null ? p.ProjectCategory.Name : "Kategori Yok",
                    ContestantCategoryId = p.Contestant.ContestantCategory.Name, // Yarışmacının kategorisini alın
                    FilePath = p.FilePath,
                    Questions = db.ProjectQuestions
                        .Select(q => new QuestionWithAnswerViewModel
                        {
                            ProjectQuestionId = q.Id,
                           
                            Question = q.Description,
                            Answer = db.ProjectAnswers
                                .Where(a => a.ProjectQuestionId == q.Id && a.ProjectId == p.Id)
                                .Select(a => a.Text)
                                .FirstOrDefault() ?? "Henüz cevaplanmamış"
                        })
                        .ToList()
                })
                .ToList();

            if (!projects.Any())
            {
                ViewBag.Message = "Şu an tarafınıza atanmış herhangi bir proje bulunmamaktadır.";
            }

            return View(projects);
        }

        public IActionResult DownloadFile(int projectId)
        {
            var project = db.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null || string.IsNullOrEmpty(project.FilePath))
            {
                return NotFound("Dosya bulunamadı.");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", Path.GetFileName(project.FilePath));
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Dosya fiziksel olarak bulunamadı.");
            }

            return PhysicalFile(filePath, "application/octet-stream", Path.GetFileName(project.FilePath));
        }

        [HttpGet]
        public IActionResult EvaluateProject(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // UserId üzerinden JudgeProfilId'yi alın
            var judge = db.Judges
       .Include(j => j.JudgeProfil)
       .FirstOrDefault(j => j.JudgeProfil.UsedContestantJudgeId == userId);

            if (judge == null)
            {
                TempData["ErrorMessage"] = "Hakem bilgisi bulunamadı.";
                return RedirectToAction("AssignedProjects");
            }



            // Örnek: JudgeId ile proje değerlendirmesi alın
            var projectEvaluation = db.ProjectEvaluations
         .Include(pe => pe.Project)
         .ThenInclude(p => p.ProjectCategory)
         .FirstOrDefault(pe => pe.ProjectId == id && pe.JudgeId == judge.Id);


            if (projectEvaluation == null)
            {
                TempData["ErrorMessage"] = "Değerlendirilecek proje bulunamadı.";
                return RedirectToAction("AssignedProjects");
            }

            var scoreProject = db.ScoreProjects
         .FirstOrDefault(sp => sp.ProjectEvaluationId == projectEvaluation.Id);

            // ViewModel oluştur ve bilgileri yükle
            var model = new ProjectEvaluationViewModel
            {
                ProjectId = projectEvaluation.Project.Id,
                ProjectName = projectEvaluation.Project.Name,
                ProjectCategory = projectEvaluation.Project.ProjectCategory.Name,
                FilePath = projectEvaluation.Project.FilePath,
                Score = scoreProject?.Score ?? null, // Daha önce kaydedilen skor yoksa null
                Comments = scoreProject?.Comments ?? string.Empty // Daha önce kaydedilen yorum yoksa boş
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult EvaluateProject(ProjectEvaluationViewModel model)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var judge = db.Judges
                .Include(j => j.JudgeProfil)
                .FirstOrDefault(j => j.JudgeProfil.UsedContestantJudgeId == userId);

            if (judge == null)
            {
                TempData["ErrorMessage"] = "Hakem bilgisi bulunamadı.";
                return RedirectToAction("AssignedProjects");
            }

            var evaluation = db.ProjectEvaluations
                .FirstOrDefault(pe => pe.ProjectId == model.ProjectId && pe.JudgeId == judge.Id);

            if (evaluation == null)
            {
                TempData["ErrorMessage"] = "Değerlendirilecek proje bulunamadı.";
                return RedirectToAction("AssignedProjects");
            }

            var scoreProject = db.ScoreProjects
                .FirstOrDefault(sp => sp.ProjectEvaluationId == evaluation.Id);

            if (scoreProject == null)
            {
                scoreProject = new ScoreProject
                {
                    ProjectEvaluationId = evaluation.Id,
                    Score = model.Score,
                    Comments = model.Comments
                };

                db.ScoreProjects.Add(scoreProject);
            }
            else
            {
                scoreProject.Score = model.Score;
                scoreProject.Comments = model.Comments;
            }

            try
            {
                db.SaveChanges();
                TempData["Message"] = "Değerlendirme başarıyla kaydedildi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Kaydetme sırasında bir hata oluştu: {ex.Message}";
            }

            return RedirectToAction("AssignedProjects");
        }







    }
}
