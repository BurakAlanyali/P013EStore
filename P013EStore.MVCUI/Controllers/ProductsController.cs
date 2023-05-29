using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Models;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Controllers
{
	public class ProductsController : Controller
	{
		private readonly IProductService _serviceProduct;

		public ProductsController(IProductService serviceProduct)
		{
			_serviceProduct = serviceProduct;
		}
		[Route("tum-urunlerimiz")]// adres çubuğundan tum-urunlerimiz yazınca bu action çalışsın.
		public async Task<IActionResult> Index()
		{
			var model = await _serviceProduct.GetAllAsync(p=>p.IsActive);
			return View(model);
		}
		public async Task<IActionResult> Search(string q) // adres çubuğunda querry string ile data alabilmek için verdiğimiz name property'si
		{
			var model = await _serviceProduct.GetAllAsync(p=>p.IsActive && p.Name.Contains(q) || p.Description.Contains(q));
			return View(model);
		}
		public async Task<IActionResult> Detail(int id)
		{
			var model = new ProductDetailViewModel();
			model.Product= await _serviceProduct.GetProductByIncludeAsync(id);
			model.RelatedProducts = await _serviceProduct.GetAllAsync(p=>p.CategoryId== model.Product.CategoryId && p.Id !=id );
            if (model == null)
			{
				return NotFound();
			}
			return View(model);
		}
	}
}
