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
    public class DiscountBannersController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Product> _product;
        private readonly IMapper _mappers;

        public DiscountBannersController(AristinoDbContext context,IRepository<Product> product,IMapper mapper)
        {
            _context = context;
            _product= product;
            _mappers= mapper;
        }

        // GET: DiscountBanners
        public async Task<IActionResult> Discount(int id)
        {
            var product=_context.Products.Where(x=>x.DiscountId==id).Include(x=>x.DiscountNavigation).Include(x=>x.Category).ToList();
            var productVM=_mappers.Map<List<ProductVM>>(product);
            ViewBag.CategoryList = _context.Categories.ToList();
            ViewBag.ColorList=_context.Colors.ToList();
            ViewBag.SizeList=_context.Sizes.ToList();
            ViewBag.AllProductDiscountCount = product.Count();
            ViewBag.DiscountBackground=_context.DiscountBanners.Where(x=>x.DiscountId==id).ToList();
			return View(productVM);
        }
    }
}
