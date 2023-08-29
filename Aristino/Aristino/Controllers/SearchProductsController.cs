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
    public class SearchProductsController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Product> _products;
        private readonly IMapper _mapper;

        public SearchProductsController(AristinoDbContext context,IRepository<Product> product,IMapper mapper)
        {
            _context = context;
            _products = product;
            _mapper = mapper;
        }

        // GET: SearchProducts
        public async Task<IActionResult> Search(string productName)
        {
            var getProductByName=_context.Products.Where(x=>x.ProductName.Contains(productName)).ToListAsync();
            var getProductByNameVM = _mapper.Map <Task<List<ProductVM>>>(getProductByName);
            ViewBag.CategoryList=_context.Categories.ToList();
            ViewBag.ColorList=_context.Colors.ToList();
            ViewBag.SizeList = _context.Sizes.ToList();
            ViewBag.ResultCount = getProductByNameVM.Result.Count();
            return View(await getProductByNameVM);
        }
    }
}
