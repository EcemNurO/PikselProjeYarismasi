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
    public class ProjectQuestionController : Controller
    {
       CompetitionDbContext db = new CompetitionDbContext();
        // GET: Management/ProjectQuestion
        public async Task<IActionResult> Index()
        {
            var ProjectQuestion =db.ProjectQuestions.ToList();
            return View(ProjectQuestion);
        }

        // GET: Management/ProjectQuestion/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            return View();
        }

        // GET: Management/ProjectQuestion/Create
        public IActionResult Create()
        {
            return View();
        }

		// POST: Management/ProjectQuestion/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProjectQuestion model)
		{
			try
			{
				if (ModelState.IsValid)
				{

					db.ProjectQuestions.Add(model);
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

		// GET: Management/ProjectQuestion/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {

			var ProjectQuestion = db.ProjectQuestions.Find(id);
			if (ProjectQuestion == null)
			{
				return RedirectToAction(nameof(Index));

			}

			return View(ProjectQuestion);
		}

		// POST: Management/ProjectQuestion/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ProjectQuestion model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var EditProjectQues = db.ProjectQuestions.Find(model.Id);
					if (EditProjectQues == null)
					{
						return RedirectToAction(nameof(Index));
					}
					EditProjectQues.Title = model.Title;
					EditProjectQues.Description = model.Description;
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
		// GET: Management/ProjectQuestion/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {


			var ProjectQuestion = db.ProjectQuestions.Find(id);
			if (ProjectQuestion == null)
			{
				return RedirectToAction(nameof(Index));

			}

			return View(ProjectQuestion);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			try
			{
				var ProjectQuestion = db.ProjectQuestions.Find(id);
				if (ProjectQuestion == null)
				{
					return RedirectToAction(nameof(Index));
				}
				db.ProjectQuestions.Remove(ProjectQuestion);
				db.SaveChanges();

				return RedirectToAction(nameof(Index));
			}
			catch (Exception)
			{

				throw;
			}
		}

	}

	// POST: Management/ProjectQuestion/Delete/5



}

