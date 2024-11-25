using Microsoft.AspNetCore.Mvc;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    public class UnivercityController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public async Task<IActionResult> Index()
        {
            var Univercity = db.univercities.ToList();
            return View(Univercity);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Univercity model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    db.univercities.Add(model);
                    db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
        }
        public async Task<IActionResult> Edit(int? id)
        {

            var Univercity = db.univercities.Find(id);
            if (Univercity == null)
            {
                return RedirectToAction(nameof(Index));

            }

            return View(Univercity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectCategory model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var editUnivercity = db.univercities.Find(model.Id);
                    if (editUnivercity== null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    editUnivercity.UniversityName = model.Name;
                    db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch
            {

                return View(model);
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            var Univercity = db.univercities.Find(id);
            if (Univercity == null)
            {
                return RedirectToAction(nameof(Index));

            }

            return View(Univercity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var Univercity = db.univercities.Find(id);
                if (Univercity == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                db.univercities.Remove(Univercity);
                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
