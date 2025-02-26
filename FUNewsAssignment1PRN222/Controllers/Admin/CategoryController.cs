using Business.Interfaces;
using Business.Services;
using Domain.Models;
using FUNewsAssignment1PRN222.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsAssignment1PRN222.Controllers.Admin
{
	[AuthorizeRole("2", "3")]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		// GET: CategoryController
		public ActionResult Index()
		{
			var categories = _categoryService.GetAllCategories();
			return View("~/Views/Admin/Category/Index.cshtml", categories);
		}

		// GET: CategoryController/Details/5
		public ActionResult Details(int id)
		{
			return View("~/Views/Admin/Category/Details.cshtml");
		}

		// GET: CategoryController/Create
		public ActionResult Create()
		{
			return View("~/Views/Admin/Category/Create.cshtml");
		}

		// POST: CategoryController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Category category)
		{
			if (ModelState.IsValid)
			{
				_categoryService.AddCategory(category);
				return RedirectToAction(nameof(Index));
			}
			return View("~/Views/Admin/Category/Detail.cshtml", category);
		}

		// GET: CategoryController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: CategoryController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: CategoryController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: CategoryController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
