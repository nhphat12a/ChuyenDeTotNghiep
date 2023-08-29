using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using AutoMapper;
using Aristino.ViewModel;

namespace Aristino.Controllers
{
    public class FashionCollectionsController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IMapper _mapper;

        public FashionCollectionsController(AristinoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: FashionCollections
        public async Task<IActionResult> Index(int CollectionID)
        {
            var getProduct = _context.Products.Where(x=>x.CollectionId==CollectionID).ToListAsync();
            var getProductVM = _mapper.Map<Task<List<ProductVM>>>(getProduct);
            ViewBag.CollectionDetail = _context.FashionCollections.Where(x => x.CollectionId == CollectionID).ToList();
            return View(await getProductVM);
        }
    }
}
