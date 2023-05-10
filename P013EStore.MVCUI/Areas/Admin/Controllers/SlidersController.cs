using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Utils;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SlidersController : Controller
	{
		private readonly IService<Slider> _service;

		public SlidersController(IService<Slider> service)
		{
			_service = service;
		}

		// GET: SlidersController
		public ActionResult Index()
		{
			var model = _service.GetAll();
			return View(model);
		}

		// GET: SlidersController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: SlidersController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: SlidersController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(Slider collection, IFormFile? Image)
		{
			try
			{
				if(Image is not null)
				{
					collection.Image= await FileHelper.FileLoaderAsync(Image);
				}
				await _service.AddAsync(collection);
				await _service.SaveAsync();
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: SlidersController/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return BadRequest();
			}
			var model = await _service.FindAsync(id.Value);
			if (model == null)
			{
				return NotFound();
			}

			return View(model);
		}

		// POST: SlidersController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(int id, Slider collection,IFormFile? Image, bool? resmiSil)
		{
			try
			{
				if(resmiSil is not null && resmiSil == true)
				{
					FileHelper.FileRemover(collection.Image);
					collection.Image = "";
				}
				if (Image is not null)
				{
					collection.Image = await FileHelper.FileLoaderAsync(Image);
				}
				_service.Update(collection);
				await _service.SaveAsync();
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: SlidersController/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return BadRequest();
			}
			var model = await _service.FindAsync(id.Value);
			if (model == null)
			{
				return NotFound();
			}

			return View(model);
		}

		// POST: SlidersController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Slider collection)
		{
			try
			{
				_service.Delete(collection);
				_service.Save();
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
