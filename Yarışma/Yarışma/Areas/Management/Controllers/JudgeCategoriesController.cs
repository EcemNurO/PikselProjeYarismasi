using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
	[Area("Management")]
	public class JudgeCategoriesController : Controller

	{
		CompetitionDbContext db = new CompetitionDbContext();



		// GET: Management/JudgeCategories
		public async Task<IActionResult> Index()
		{
			var JudgeCategories = db.JudgeCategories.ToList();
			return View(JudgeCategories);

		}

		// GET: Management/JudgeCategories/Details/5
		public async Task<IActionResult> Details(int? id)
		{

			return View();
		}

		// GET: Management/JudgeCategories/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Management/JudgeCategories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(JudgeCategory model)
		{
			try
			{
				if (ModelState.IsValid)
				{

					db.JudgeCategories.Add(model);
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
		// GET: Management/JudgeCategories/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			var JudgeCategory = db.JudgeCategories.Find(id);
			if (JudgeCategory == null)
			{
				return RedirectToAction(nameof(Index));

			}

			return View(JudgeCategory);

			return View();
		}

		// POST: Management/JudgeCategories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(JudgeCategory model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var EditJudgetCat = db.JudgeCategories.Find(model.Id);
					if (EditJudgetCat == null)
					{
						return RedirectToAction(nameof(Index));
					}
					EditJudgetCat.Name = model.Name;
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
			// GET: Management/JudgeCategories/Delete/5
			public ActionResult Delete(int id)
			{
				try
				{
					var JudgeCategory = db.JudgeCategories.Find(id);
					if ( JudgeCategory== null)
					{
						return RedirectToAction(nameof(Index));
					}
					db.JudgeCategories.Remove(JudgeCategory);
					db.SaveChanges();

					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
			{

				return View();

			}
		}


		}
	
}
