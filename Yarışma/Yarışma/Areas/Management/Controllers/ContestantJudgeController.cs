//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Yarışma.Models;
//using System.Linq;

//namespace Yarışma.Areas.Management.Controllers
//{
//    [Area("Management")]
//    public class ContestantJudgeController : Controller
//    {
//        private readonly CompetitionDbContext db = new CompetitionDbContext();

//        public IActionResult Index()
//        {
//            var projects = db.Projects
//                .Include(p => p.Contestant)
//                .ThenInclude(p=> p.ContestantJudges)
//                .ThenInclude(cj => cj.Judge)
//                .ToList();

//            return View(projects);
//        }

//        [HttpGet]
//        public IActionResult AssignJudge(int projectId)
//        {
//            var project = db.Projects.Find(projectId);
//            if (project == null)
//            {
//                return NotFound();
//            }

//            ViewBag.ProjectId = projectId;
//            ViewBag.Judges = db.Judges.ToList();
//            return View();
//        }

//        [HttpPost]
//        public IActionResult AssignJudge(int projectId, int judgeId)
//        {
//            var existingAssignment = db.ContestantJudges
//                .FirstOrDefault(cj => cj.ProjectId == projectId && cj.JudgeId == judgeId);

//            if (existingAssignment == null)
//            {
//                var assignment = new ContestantJudge
//                {
//                    ProjectId = projectId,
//                    JudgeId = judgeId,
//                    IsAssigned = true
//                };
//                db.ContestantJudges.Add(assignment);
//                db.SaveChanges();
//            }

//            return RedirectToAction("Index");
//        }

//        [HttpPost]
//        public IActionResult RemoveAssignment(int assignmentId)
//        {
//            var assignment = db.ContestantJudges.Find(assignmentId);
//            if (assignment != null)
//            {
//                db.ContestantJudges.Remove(assignment);
//                db.SaveChanges();
//            }
//            return RedirectToAction("Index");
//        }

//        public IActionResult Details(int id)
//        {
//            var project = db.Projects
//                .Include(p => p.Contestant)
//                .ThenInclude(p => p.ContestantJudges)
//                .ThenInclude(cj => cj.Judge)
//                .FirstOrDefault(p => p.Id == id);

//            if (project == null)
//            {
//                return NotFound();
//            }

//            return View(project);
//        }
//    }
//}
