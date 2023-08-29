using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using Aristino.ViewModel;
using Aristino.Helper;
using Aristino.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    [Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
    public class AdminProductsController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IWebHostEnvironment _environtment;
        private readonly IRepository<Product> _productsRepository;
        private readonly IMapper _mapper;
        public AdminProductsController(AristinoDbContext context, IWebHostEnvironment environtment, IRepository<Product> productrepository, IMapper mapper)
        {
            _context = context;
            _environtment = environtment;
            _productsRepository = productrepository;
            _mapper = mapper;
        }

        // GET: AristinoAdmin/AdminProducts
        public async Task<IActionResult> Index()
        {
            var ProductList = _productsRepository.GetAll().Include(p => p.Category).Include(x=>x.Collection);
            var ProductListVM = _mapper.Map<Task<List<ProductVM>>>(ProductList.ToListAsync());
            return View(await ProductListVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRatingStar()
        {
            var getAllProduct=_productsRepository.GetAll().ToList();
            var getAllComment = _context.Comments.ToList();
            int OneStar=0,TwoStar=0,ThreeStar=0,FourStar=0,FiveStar=0;
            decimal AverageRate = 0;
            foreach(var product in getAllProduct)
            {
                foreach(var comment in getAllComment.Where(x=>x.ProductId==product.ProductId))
                {
                    switch (comment.StarRating)
                    {
                        case 20:
                            OneStar += 1;
                            break;
                        case 40:
                            TwoStar += 1;
                            break;
                        case 60:
                            ThreeStar+= 1;
                            break;
                        case 80:
                            FourStar+= 1;
                            break;
                        case 100:
                            FiveStar+= 1;
                            break;
                        default:
                            break;
                    }
                }
                if (OneStar != 0 || TwoStar != 0 || ThreeStar != 0 || FourStar != 0 || FiveStar != 0)
                {
                    AverageRate=(1*OneStar+2*TwoStar+3*ThreeStar+4*FourStar+5*FiveStar)/(OneStar+TwoStar+ThreeStar+FourStar+FiveStar);
                    await _context.Products.Where(x => x.ProductId == product.ProductId).ExecuteUpdateAsync(set => set.
                    SetProperty(x => x.StarRate, AverageRate.ToString()));
                }
                else
                {
                    await _context.Products.Where(x => x.ProductId == product.ProductId).ExecuteUpdateAsync(set => set.SetProperty(x => x.StarRate, "0"));
                }
            }
            TempData["Success"] = "Cập Nhật Thành Công";
            return RedirectToAction("Index");
        }
        // GET: AristinoAdmin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorName", "ColorName");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeName", "SizeName");
            ViewData["DiscountBanner"] = new SelectList(_context.DiscountBanners, "DiscountId", "DiscountDescription");
            return View();
        }

        // POST: AristinoAdmin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM productVm, bool ActiveButton)
        {
            (string, string) getImageName = ("", "");
            UploadImage upload = new UploadImage(_environtment);
            if (!ModelState.IsValid)
            {
                TempData["isError"] = "is error";
                TempData["ErrorList"] = string.Join("<br>", ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
                return Json(new { redirectToUrl = Url.Action("Create", "AdminProducts") });
            }
            var getProductName = _context.Products.FirstOrDefault(x => x.ProductName == productVm.ProductName);
            if (getProductName != null)
            {
                TempData["Error"] = "Sản Phẩm Đã Tồn Tại";
                return Json(new { redirectToUrl = Url.Action("Create", "AdminProducts") });
            }
            if (productVm.DiscountId != null && productVm.CategoryId != _context.DiscountBanners.Find(productVm.DiscountId).CategoryId)
            {
                var getDiscountBanner = _context.DiscountBanners.Find(productVm.DiscountId);
                var getCategoryName = _context.Categories.Find(getDiscountBanner.CategoryId).CategoryName;
                TempData["DiscountError"] = "Banner này chỉ giảm giá cho mặt hàng " + getCategoryName;
                return Json(new { redirectToUrl = Url.Action("Create", "AdminProducts") });
            }
            if(productVm.DiscountId != null)
            {
                productVm.Discount = _context.DiscountBanners.FirstOrDefault(x=>x.DiscountId==productVm.DiscountId).DiscountRate;
            }
            else
            {
                productVm.Discount = 0;
            }
            if (upload.CheckFileExtension(productVm.UploadImage))
            {
                TempData["Error"] = "Chỉ Hỗ Trợ Định Dạng Hình: .png,.jpg,.jpeg,.gif";
                return Json(new { redirectToUrl = Url.Action("Create") });
            }
            if (upload.CheckFileSize(productVm.UploadImage))
            {
                TempData["Error"] = "Hình Vượt Quá Giới Hạn, Giới Hạn Tối Đa 30MB";
                return Json(new { redirectToUrl = Url.Action("Create") });
            }
            var getControllerName = ControllerContext.ActionDescriptor.ControllerName;
            if (productVm.UploadImage != null)
            {
                getImageName = await upload.Upload(productVm.ProductName, productVm.UploadImage, getControllerName);
            }
            var MultipleColor = String.Join(",", productVm.ColorList);
            var MultipleSize = String.Join(",", productVm.SizeList);
            productVm.Color = MultipleColor;
            productVm.Size = MultipleSize;
            productVm.ProductImage = getImageName.Item1;
            productVm.ProductGallery = getImageName.Item2;
            productVm.Active = ActiveButton;
            productVm.StarRate = "0";
            productVm.ProductDiscontinue = false;
            var product = _mapper.Map<Product>(productVm);
            await _productsRepository.AddEntity(product);
            _productsRepository.SaveChanges();
            //Trả về nội dung thông báo thành công
            TempData["Success"] = "Product Added";
            return Json(new { redirectToUrl = Url.Action("Index", "AdminProducts") });
        }

        // GET: AristinoAdmin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var product = await _productsRepository.GetByWhereAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productvm = _mapper.Map<ProductVM>(product);
            ViewBag.ProductName = productvm.ProductName;
            ViewBag.ImageGallery = productvm.ProductGallery.Split(",").ToList();
            //ViewBag.SizeList chứa các Size của 1 product cụ thể
            ViewBag.SizeList = productvm.Size.Split(",").ToList();
            ViewBag.ColorList = productvm.Color.Split(",").ToList();
            //Liệt kê toàn bộ size có trong bảng Size
            var SizeLists = _context.Sizes.Select(x => x.SizeName.ToString()).ToList();
            var ColorList = _context.Colors.Select(x => x.ColorName.ToString()).ToList();
            var DiscountBannerList = _context.DiscountBanners.Where(x => x.DiscountId != productvm.DiscountId).ToList();
            var SizeListViewData = new List<SizeVM>();
            var ColorListViewData = new List<ColorVM>();
            var DiscountBannerViewData = new List<DiscountBannerVM>();
            foreach (var item in ViewBag.SizeList)
            {
                if (SizeLists.Contains(item))
                {
                    SizeLists.Remove(item);
                }
            }
            foreach (var item in SizeLists)
            {
                SizeVM sizevm = new();
                sizevm.SizeName = item;
                SizeListViewData.Add(sizevm);
            }
            foreach (var item in ViewBag.ColorList)
            {
                if (ColorList.Contains(item))
                    ColorList.Remove(item);

            }
            foreach (var item in ColorList)
            {
                ColorVM colorvm = new();
                colorvm.ColorName = item;
                ColorListViewData.Add(colorvm);
            }
            if (productvm.DiscountId != null)
            {
                ViewBag.DiscountId = productvm.DiscountId;
                ViewBag.DiscountDescription = _context.DiscountBanners.Find(productvm.DiscountId).DiscountDescription;
                var DiscountBannerListVM = _mapper.Map<List<DiscountBannerVM>>(DiscountBannerList);
                foreach (var item in DiscountBannerListVM)
                {
                    DiscountBannerViewData.Add(item);
                }
            }
            else
            {
                var getAllBanner = _context.DiscountBanners.Include(x => x.Products).ToList();
                var getAllBannerVM = _mapper.Map<List<DiscountBannerVM>>(getAllBanner);
                foreach (var item in getAllBannerVM)
                {
                    DiscountBannerViewData.Add(item);
                }

            }
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", productvm.CategoryId);
            ViewData["ColorId"] = new SelectList(ColorListViewData, "ColorName", "ColorName");
            ViewData["SizeId"] = new SelectList(SizeListViewData, "SizeName", "SizeName");
            ViewData["DiscountBanner"] = new SelectList(DiscountBannerViewData, "DiscountId", "DiscountDescription", productvm.DiscountId);
            ViewBag.CheckDiscontinue = productvm.ProductDiscontinue;
            return View(productvm);
        }

        // POST: AristinoAdmin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductVM productvm, bool ActiveButton)
        {
            UploadImage upload = new(_environtment);
            (string, string) getImageName = ("", "");
            var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            var getProductStatus = _context.Products.AsNoTracking().FirstOrDefault(x => x.ProductId == productvm.ProductId);
            if (id != productvm.ProductId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
                return Json(new { redirectToUrl = Url.Action("Edit", "AdminProducts") });
            }
            if (productvm.DiscountId != null && productvm.CategoryId != _context.DiscountBanners.Find(productvm.DiscountId).CategoryId)
            {
                var getCategoyName = _context.DiscountBanners.Include(x => x.Category).FirstOrDefault(x => x.DiscountId == productvm.DiscountId);
                TempData["DiscountError"] = "Banner này chỉ giảm giá cho mặt hàng " + getCategoyName.Category.CategoryName;
                return Json(new { redirectToUrl = Url.Action("Edit", "AdminProducts") });
            }
            if(productvm.DiscountId!=null)
            {
                productvm.Discount=_context.DiscountBanners.FirstOrDefault(x=>x.DiscountId==productvm.DiscountId).DiscountRate;
            }
            else
            {
                productvm.Discount = 0;
            }
            //var getProductName = _context.Products.FirstOrDefault(x => x.ProductName == productvm.ProductName);
            foreach (var item in _context.Products.Where(x => x.ProductName == productvm.ProductName).AsNoTracking())
            {
                if (productvm.ProductId == item.ProductId)
                    continue;
                TempData["ProductExisted"] = "Sản Phẩm Đã Tồn Tại";
                return Json(new { redirectToUrl = Url.Action("Edit", "AdminProducts") });
            }
            if (upload.CheckFileExtension(productvm.UploadImage))
            {
                TempData["Error"] = "Chỉ Hỗ Trợ Định Dạng Hình: .png,.jpg,.jpeg,.gif";
                return Json(new { redirectToUrl = Url.Action("Edit") });
            }
            if (upload.CheckFileSize(productvm.UploadImage))
            {
                TempData["Error"] = "Hình Vượt Quá Giới Hạn, Giới Hạn Tối Đa 30MB";
                return Json(new { redirectToUrl = Url.Action("Edit") });
            }
            var getOriginalName = _context.Products.AsNoTracking().FirstOrDefault(x => x.ProductId == productvm.ProductId).ProductName;
            if (upload.CheckDirExist(getOriginalName, ControllerName))
            {
                upload.DeleteUploadFile(getOriginalName, ControllerName);
            }
            if (productvm.UploadImage != null)
            {
                getImageName = await upload.Upload(productvm.ProductName, productvm.UploadImage, ControllerName);
            }
            if(!ActiveButton && getProductStatus.Active)
            {
                TempData["Error"] = "Sản Phẩm Đang Bán, Không Thể Chuyển Sang Chuẩn Bị Bán";
                return Json(new { redirectToUrl = Url.Action("Edit") });
            }
            if (!productvm.ProductDiscontinue)
            {
               productvm.Active= true;
            }
            var ColorLString = String.Join(",", productvm.ColorList);
            var SizeString = String.Join(",", productvm.SizeList);
            productvm.ProductImage = getImageName.Item1;
            productvm.ProductGallery = getImageName.Item2;
            productvm.Color = ColorLString;
            productvm.Size = SizeString;
            if(productvm.ProductDiscontinue)
            {
                productvm.Active = false;
                productvm.Quantity = 0;
            }
            var product = _mapper.Map<Product>(productvm);
            await _productsRepository.UpdateEntity(product);
            _productsRepository.SaveChanges();
            return Json(new { redirectToUrl = Url.Action("Index", "AdminProducts") });
        }
        // POST: AristinoAdmin/AdminProducts/Delete/5
        [Authorize(Policy ="AdminOnly")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UploadImage upload = new UploadImage(_environtment);
            if (_context.Products == null)
            {
                return Problem("Entity set 'AristinoDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            var checkProductInOrder = _context.OrdersDetails.FirstOrDefault(x => x.ProductId == id);
            if(checkProductInOrder!=null)
            {
                TempData["Error"] = "Sản Phẩm Đã Phát Sinh Hóa Đơn, Không Thể Xóa, Vui Lòng Chuyển Sang Ngừng Kinh Doanh Nếu Không Kinh Doanh";
                return RedirectToAction("Index");
            }
            if (product != null)
            {
                if (upload.CheckDirExist(product.ProductName, ControllerContext.ActionDescriptor.ControllerName) != false)
                    upload.DeleteUploadFile(product.ProductName, ControllerContext.ActionDescriptor.ControllerName);
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMultipleProducts(List<int> ProductIDList)
        {
            UploadImage upload = new(_environtment);
            var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            foreach (var item in ProductIDList)
            {
                var getProduct = _context.Products.FirstOrDefault(x => x.ProductId == item);
                var checkExistInOrder = _context.OrdersDetails.FirstOrDefault(x => x.ProductId == item);
                if(checkExistInOrder!=null)
                {
                    TempData["Error"] = "Có Sản Phẩm Đã Phát Sinh Hóa Đơn, Không Thể Xóa, Vui Lòng Chuyển Sang Ngừng Kinh Doanh Nếu Không Kinh Doanh";
                    return Json(new {redirectToUrl=Url.Action("Index"), isSuccess = "Failed" });
                }
                if (upload.CheckDirExist(getProduct.ProductName, ControllerName))
                {
                    upload.DeleteUploadFile(getProduct.ProductName, ControllerName);
                }
                _context.Products.Remove(getProduct);
                await _context.SaveChangesAsync();
            }
            return Json(new {Success="Ok"});
        }
        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
