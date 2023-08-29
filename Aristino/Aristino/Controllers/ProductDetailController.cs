using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using Aristino.ViewModel;
using AutoMapper;
using Newtonsoft.Json;

namespace Aristino.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public ProductDetailController(AristinoDbContext context,IMapper mapper,IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        // GET: ProductDetail
        public async Task<IActionResult> Index(int id)
        {
            var CustomerId = 0;
            var aristinoDbContext = _context.Products.Where(x => x.ProductId == id).Include(p => p.Category).Include(x=>x.DiscountNavigation);
            var product = _context.Products.Include(x=>x.DiscountNavigation).FirstOrDefault(x => x.ProductId == id);
            var getDiscount = product.DiscountId;
            //Kiểm tra xem banner giảm giá còn thời gian không, nếu không thì xóa banner đó ra khỏi sản phẩm
            if (product.Discount > 0)
            {
                if (DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm")) >= DateTime.Parse(product.DiscountNavigation.EndSale))
                {
                    string isNull = null;
                    _context.DiscountBanners.Where(x => x.DiscountId == getDiscount).ExecuteUpdate(set => set.
                    SetProperty(x => x.DisableDiscount, true));
                    _context.Products.Where(x => x.DiscountId == getDiscount).ExecuteUpdate(set => set.SetProperty(a => a.DiscountId, a => null).SetProperty(x => x.Discount, 0));
                }
            }
            if (Request.Cookies.ContainsKey("UserLogin"))
            {
                CustomerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
                ViewBag.UserComment = _context.Comments.Where(x => x.CustomerId == CustomerId && x.ProductId==product.ProductId).Include(x => x.Customer).ToList();
			}
            ViewBag.ImageGallery = product.ProductGallery.Split(",").ToList();
            var getSizeList = product.Size.Split(",").ToList();
            var getColorList=product.Color.Split(",").ToList();
            var SizeList = new List<SizeVM>();
            var ColorList = new List<ColorVM>();
            foreach(var item in getSizeList)
            {
				SizeVM size = new()
				{
					SizeName = item
				};
				SizeList.Add(size);
            }
            foreach(var item in getColorList)
            {
                ColorVM color = new()
                {
                    ColorName = item
                };
                ColorList.Add(color);
            }

			var getUserCart = _context.Carts.Where(x => x.CustomerId == CustomerId).Include(x => x.Product).Include(x => x.Customer);
            var session = _httpContext.HttpContext.Session;
			JsonSerializerSettings setting = new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			};
			session.SetString("UserCart", JsonConvert.SerializeObject(getUserCart, setting));
            TempData["NumberOfCart"] = _context.Carts.Where(x => x.CustomerId == CustomerId).Count();
			ViewData["SizeList"]=new SelectList(SizeList, "SizeName", "SizeName");
            ViewData["ColorList"] = new SelectList(ColorList, "ColorName", "ColorName");
            ViewBag.RecommendProduct = _context.Products.Where(x=>x.CategoryId==product.CategoryId).ToList();
            if (product.Discount != 0)
            {
                ViewBag.EndSale = aristinoDbContext.Select(x=>x.DiscountNavigation.EndSale).ToList();
            }
            ViewBag.CheckIfSale = _context.Products.FirstOrDefault(x => x.ProductId == id).Discount;
            ViewBag.CommentCount = _context.Comments.Where(x => x.ProductId == id).Count();
            return View(await aristinoDbContext.ToListAsync());
        }
        public async Task<IActionResult> AllProductComment(int productId)
        {
            var getAllComment=_context.Comments.Where(x=>x.ProductId==productId).Include(x=>x.Customer).Include(x=>x.Product).ToList();
            var getAllCommentVM=_mapper.Map<List<CommentVM>>(getAllComment);
            ViewBag.ProductImage = _context.Products.Where(x=>x.ProductId==productId);
            ViewBag.CommentCount = getAllCommentVM.Count();
            return View(getAllCommentVM);
        }
    }
}
