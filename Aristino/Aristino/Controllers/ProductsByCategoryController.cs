using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using Aristino.Repository;
using AutoMapper;
using Aristino.ViewModel;

namespace Aristino.Controllers
{
    public class ProductsByCategoryController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Product> _products;
        private readonly IMapper _mapper;

        public ProductsByCategoryController(AristinoDbContext context,IRepository<Product> products,IMapper mapper)
        {
            _context = context;
            _products = products;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetByCategory(int id)
        {
            var getProductByCat=_products.GetAll().Where(x=>x.CategoryId==id).ToListAsync();
            var getProductByCatVM = _mapper.Map<Task<List<ProductVM>>>(getProductByCat);
            ViewBag.ColorList=_context.Colors.ToList();
            ViewBag.SizeList=_context.Sizes.ToList();
            ViewBag.CategoryList=_context.Categories.ToList();
            ViewBag.CategoryImage = _context.Categories.FirstOrDefault(x=>x.CategoryId==id).Avatar.Split(",").ToList();
            ViewBag.CategoryName = _context.Categories.FirstOrDefault(x => x.CategoryId == id).CategoryName;
            ViewBag.ResultCount = getProductByCatVM.Result.Count();
			return View(await getProductByCatVM);
        }
		[HttpPost,ActionName("GetByCategory")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SearchFilter(string color, string size, int CategoryId, string price)
		{
			var minPrice = 100000;
			var priceConvert = Convert.ToInt32(price);
			var productFilter = _context.Products.Where
				(x => x.Color.Contains(color) && x.Size.Contains(size) && x.CategoryId == CategoryId && minPrice <= x.Price && x.Price <= priceConvert).ToListAsync();
			var productFilterVM=_mapper.Map<Task<List<ProductVM>>>(productFilter);
			ViewBag.ColorList = _context.Colors.ToList();
			ViewBag.SizeList = _context.Sizes.ToList();
			ViewBag.CategoryList = _context.Categories.ToList();
			ViewBag.CategoryImage = _context.Categories.FirstOrDefault(x => x.CategoryId == CategoryId).Avatar.Split(",").ToList();
			ViewBag.CategoryName = _context.Categories.FirstOrDefault(x => x.CategoryId == CategoryId).CategoryName;
            ViewBag.ResultCount = productFilterVM.Result.Count();
			return View(await productFilterVM);
		}
	}
}
