    using Aristino.Models;
using Aristino.Repository;
using Aristino.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Aristino.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Product> _product;
        private readonly IMapper _mappers;
        private readonly AristinoDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public HomeController(ILogger<HomeController> logger, IRepository<Product> product, IMapper mapper, AristinoDbContext context,IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _product = product;
            _mappers = mapper;
            _context = context;
            _httpContext = httpContext;
        }

        public async Task<IActionResult> Index()
        {
            var getProducts = _product.GetAll().Include(x => x.Category).Include(x => x.DiscountNavigation).ToListAsync();
            var productListVM = _mappers.Map<Task<List<ProductVM>>>(getProducts);
            var Customerid = 0;
            var session = _httpContext.HttpContext.Session;
            var getAllDiscountBanner = _context.DiscountBanners;
            var getAllComment = _context.Comments.ToList();
            foreach(var item in getAllDiscountBanner.ToList())
            {
                if(DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm")) > DateTime.Parse(item.EndSale) && item.DisableDiscount !=true)
                {
                    _context.Products.Where(x => x.DiscountId == item.DiscountId).ExecuteUpdate(set => set.
                    SetProperty(x => x.DiscountId, a => null).SetProperty(x => x.Discount, 0));
                    _context.DiscountBanners.Where(x => x.DiscountId == item.DiscountId).ExecuteUpdate(set => set.
                    SetProperty(x => x.DisableDiscount, true));
                }
            }
            if(Request.Cookies.ContainsKey("UserLogin"))
            {
                Customerid = Convert.ToInt32(User.Claims.FirstOrDefault(x=>x.Type=="CustomerId").Value);
            }

            var getUserCart = _context.Carts.Where(x => x.CustomerId == Customerid).Include(x=>x.Product).Include(x=>x.Customer);
            var getCategory = _context.Categories;
            var fashionCollection = _context.FashionCollections;
            var getPolicy = _context.AristinoPolicies;
            //Bỏ qua Exception Loop
            JsonSerializerSettings setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            //string CartJSON=JsonConvert.SerializeObject(getUserCart).ToString();
            session.SetString("UserCart", JsonConvert.SerializeObject(getUserCart,setting));
            session.SetString("Category",JsonConvert.SerializeObject(getCategory,setting));
            session.SetString("FashionCollection", JsonConvert.SerializeObject(fashionCollection, setting));
            session.SetString("AristinoPolicy", JsonConvert.SerializeObject(getPolicy,setting));
            ViewBag.DiscountBannerList = _context.DiscountBanners.ToList();
            TempData["NumberOfCart"] = getUserCart.Count();
            ViewBag.Blog=_context.Blogs.Include(x=>x.BlogComments).ToList();
            ViewBag.FashionCollection = _context.FashionCollections.ToList();
            ViewBag.Category = getCategory.ToList();
            TempData["HasCookie"] = Request.Cookies.ContainsKey("UserLogin");
            ViewBag.CategoryOne=getCategory.Where(x=>x.CategoryName == "Áo Blazer").ToList();
			ViewBag.CategoryTwo = getCategory.Where(x => x.CategoryId==19).ToList();
            ViewBag.AllBannerIsActive = getAllDiscountBanner.Where(x => x.DisableDiscount == false).Count();
            ViewBag.CollectionActive = _context.FashionCollections.Where(x => x.CollectionDisable == false).Count();
			return View(await productListVM);
        }
    }
}