using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class ProjectCategoriesController : Controller
    {
        
       CompetitionDbContext db = new CompetitionDbContext();

        // GET: Management/ProjectCategories
        public async Task<IActionResult> Index()
        {
            var ProjectCategories = db.ProjectCategories.ToList();
            return View(ProjectCategories);
        }

        // GET: Management/ProjectCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
           

            return View();
        }

        // GET: Management/ProjectCategories/Create
        public IActionResult Create()
        {
            return View();
        }

		// POST: Management/ProjectCategories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProjectCategory model)
		{
			try
			{
				if (ModelState.IsValid)
				{

					db.ProjectCategories.Add(model);
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


		// GET: Management/ProjectCategories/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {

			var ProjectCategory = db.ProjectCategories.Find(id);
			if (ProjectCategory == null)
			{
				return RedirectToAction(nameof(Index));

			}

			return View(ProjectCategory);
		}

		// POST: Management/ProjectCategories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ProjectCategory model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var editProjectCat = db.ProjectCategories.Find(model.Id);
					if (editProjectCat == null)
					{
						return RedirectToAction(nameof(Index));
					}
					editProjectCat.Name = model.Name;
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


		// GET: Management/ProjectCategories/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
			var ProjectCategory = db.ProjectCategories.Find(id);
			if (ProjectCategory == null)
			{
				return RedirectToAction(nameof(Index));

			}

			return View(ProjectCategory);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			try
			{
				var ProjectCategory = db.ProjectCategories.Find(id);
				if (ProjectCategory == null)
				{
					return RedirectToAction(nameof(Index));
				}
				db.ProjectCategories.Remove(ProjectCategory);
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

