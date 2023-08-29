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
using Aristino.Helper;
using Aristino.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
	[Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
	public class AdminDiscountBannersController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<DiscountBanner> _discount;

        public AdminDiscountBannersController(AristinoDbContext context, IMapper mapper, IWebHostEnvironment environment, IRepository<DiscountBanner> discount)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
            _discount = discount;
        }

        // GET: AristinoAdmin/AdminDiscountBanners
        public async Task<IActionResult> Index()
        {
            var aristinoDbContext = _context.DiscountBanners.Include(d => d.Category).ToListAsync();
            var DiscountBannerVM = _mapper.Map<Task<List<DiscountBannerVM>>>(aristinoDbContext);
            return View(await DiscountBannerVM);
        }

        // GET: AristinoAdmin/AdminDiscountBanners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DiscountBanners == null)
            {
                return NotFound();
            }

            var discountBanner = await _context.DiscountBanners
                .Include(d => d.Category)
                .FirstOrDefaultAsync(m => m.DiscountId == id);
            if (discountBanner == null)
            {
                return NotFound();
            }

            return View(discountBanner);
        }

        // GET: AristinoAdmin/AdminDiscountBanners/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: AristinoAdmin/AdminDiscountBanners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountBannerVM discountBannerVM)
        {
            var getCurrentDate = DateTime.Now.ToString("MM/dd/yyyy");
            var getCurrentTime = DateTime.Now.ToString("HH:mm");
            UploadImage upload = new(_environment);
            if (!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
                return Json(new { redirectToUrl = Url.Action("Create", "AdminDiscountBanners") });
            }
            if (_context.DiscountBanners.FirstOrDefault(x => x.DiscountName == discountBannerVM.DiscountName) != null)
            {
                TempData["Error"] = "Tên Banner Này Đã Được Dùng";
                return Json(new { redirectToUrl = Url.Action("Create", "AdminDiscountBanners") });
            }
            if (DateTime.Parse(discountBannerVM.EndDate.ToString("MM/dd/yyyy")) < DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy")))
            {
                TempData["Error"] = "Ngày Kết Thúc Giảm Giá Không Được Nhỏ Hơn Ngày Hiện Tại";
                return Json(new { redirectToUrl = Url.Action("Create", "AdminDiscountBanners") });
            }
            if (DateTime.Parse(discountBannerVM.EndDate.ToString("MM/dd/yyyy")+" "+discountBannerVM.EndTime.ToString("HH:mm")) <= DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm")))
            {
                TempData["Error"] = "Giờ Kết Thúc Giảm Giá Không Được Nhỏ Hơn Hoặc Bằng Giờ Hiện Tại";
                return Json(new { redirectToUrl = Url.Action("Create", "AdminDiscountBanners") });
            }
            if (discountBannerVM.UploadImage != null)
            {
                var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
                var GetImage = await upload.Upload(discountBannerVM.DiscountName, discountBannerVM.UploadImage, ControllerName);
                discountBannerVM.Banner = GetImage.Item1;
            }
            var CategoryName = _context.Categories.AsNoTracking().FirstOrDefault(x => x.CategoryId == discountBannerVM.CategoryId).CategoryName;
            discountBannerVM.DiscountDescription = "Giảm giá " + discountBannerVM.DiscountRate + "% cho mặt hàng " + CategoryName;
            discountBannerVM.StartSale = getCurrentDate + " " + getCurrentTime;
            discountBannerVM.EndSale = discountBannerVM.EndDate.ToString("MM/dd/yyyy") + " " + discountBannerVM.EndTime.ToString("HH:mm");
            discountBannerVM.DisableDiscount = false;
            var discountBanner = _mapper.Map<DiscountBanner>(discountBannerVM);
            await _discount.AddEntity(discountBanner);
            _discount.SaveChanges();
            TempData["Success"] = "Discount Banner Added";
            return Json(new { redirectToUrl = Url.Action("Index", "AdminDiscountBanners") });
        }

        // GET: AristinoAdmin/AdminDiscountBanners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DiscountBanners == null)
            {
                return NotFound();
            }

            var discountBanner = await _context.DiscountBanners.FindAsync(id);
            var discountBannerVM = _mapper.Map<DiscountBannerVM>(discountBanner);
            discountBannerVM.EndDate = DateTime.Parse(DateTime.Parse(discountBannerVM.EndSale).ToString("MM/dd/yyyy"));
            discountBannerVM.EndTime = DateTime.Parse(DateTime.Parse(discountBannerVM.EndSale).ToString("HH:mm"));
            ViewBag.ImageGallery = discountBanner.Banner.Split(",").ToList();
            ViewBag.DiscountName = discountBanner.DiscountName;
            if (discountBanner == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", discountBanner.CategoryId);
            return View(discountBannerVM);
        }

        // POST: AristinoAdmin/AdminDiscountBanners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DiscountBannerVM discountBannerVM)
        {
            UploadImage upload = new(_environment);
            var getAllBannerName = _context.DiscountBanners.Where(x => x.DiscountName == discountBannerVM.DiscountName).AsNoTracking().ToList();
            if (id != discountBannerVM.DiscountId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                TempData["DiscountError"] = String.Join("<br>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Json(new { redirectToUrl = Url.Action("Edit", "AdminDiscountBanners") });
            }
            foreach (var item in getAllBannerName)
            {
                if (discountBannerVM.DiscountId == item.DiscountId)
                    continue;
                TempData["DiscountError"] = "Tên Banner Đã Được Sử Dụng";
                return Json(new { redirectToUrl = Url.Action("Edit", "AdminDiscountBanners") });
            }
            if (DateTime.Parse(discountBannerVM.EndDate.ToString("MM/dd/yyyy")) < DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy")))
            {
                TempData["Error"] = "Ngày Kết Thúc Giảm Giá Không Được Nhỏ Hơn Ngày Hiện Tại";
                return Json(new { redirectToUrl = Url.Action("Edit", "AdminDiscountBanners") });
            }
            if (DateTime.Parse(discountBannerVM.EndDate.ToString("MM/dd/yyyy")+" "+discountBannerVM.EndTime.ToString("HH:mm")) <= DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm")))
            {
                TempData["Error"] = "Giờ Kết Thúc Giảm Giá Không Được Nhỏ Hơn Hoặc Bằng Ngày Và Giờ Hiện Tại";
                return Json(new { redirectToUrl = Url.Action("Edit", "AdminDiscountBanners") });
            }
            if (discountBannerVM.UploadImage != null)
            {
                var CurrentDBDiscountName = _context.DiscountBanners.AsNoTracking().FirstOrDefault(x => x.DiscountId == discountBannerVM.DiscountId).DiscountName;
                var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
                if (upload.CheckDirExist(CurrentDBDiscountName, ControllerName))
                    upload.DeleteUploadFile(CurrentDBDiscountName, ControllerName);
                var getImage = await upload.Upload(discountBannerVM.DiscountName, discountBannerVM.UploadImage, ControllerName);
                discountBannerVM.Banner = getImage.Item1;
            }
            var CategoryName = _context.Categories.AsNoTracking().FirstOrDefault(x => x.CategoryId == discountBannerVM.CategoryId).CategoryName;
            discountBannerVM.DiscountDescription = "Giảm giá " + discountBannerVM.DiscountRate + "% cho mặt hàng " + CategoryName;
            discountBannerVM.EndSale = discountBannerVM.EndDate.ToString("MM/dd/yyyy") + " " + discountBannerVM.EndTime.ToString("HH:mm");
            discountBannerVM.DisableDiscount = false;
            var discount = _mapper.Map<DiscountBanner>(discountBannerVM);
            _discount.UpdateEntity(discount);
            _discount.SaveChanges();
            return Json(new { redirectToUrl = Url.Action("Index", "AdminDiscountBanners") });
        }

        // POST: AristinoAdmin/AdminDiscountBanners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UploadImage upload = new(_environment);
            if (_context.DiscountBanners == null)
            {
                return Problem("Entity set 'AristinoDbContext.DiscountBanners'  is null.");
            }
            var discountBanner = await _context.DiscountBanners.FindAsync(id);
            if (discountBanner != null)
            {
                var controllerName = ControllerContext.ActionDescriptor.ControllerName;
                if (upload.CheckDirExist(controllerName, discountBanner.DiscountName))
                {
                    upload.DeleteUploadFile(discountBanner.DiscountName, controllerName);
                }
                _context.Products.Where(x => x.DiscountId == id).ExecuteUpdate(set => set.
                SetProperty(x => x.DiscountId, x => null).SetProperty(x => x.Discount, 0));
                _context.DiscountBanners.Remove(discountBanner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMultiple(List<int> ProductIDList)
        {
            UploadImage upload = new(_environment);
            var ControllerName=ControllerContext.ActionDescriptor.ControllerName;
            foreach(var item in ProductIDList)
            {
                var getBanner = _context.DiscountBanners.FirstOrDefault(x => x.DiscountId == item);
                if (upload.CheckDirExist(getBanner.DiscountName, ControllerName))
                {
                    upload.DeleteUploadFile(getBanner.DiscountName, ControllerName);
                }
                await _context.Products.Where(x=>x.DiscountId==item).ExecuteUpdateAsync
                    (set=>set.SetProperty(x=>x.DiscountId,x=>null).SetProperty(x=>x.Discount, 0));
                _context.DiscountBanners.Remove(getBanner);
                _context.SaveChanges();
            }
            return Json(new { OK = "ok" });
        }
        public async Task<IActionResult> AddProductToBanner(int CategoryId,int DiscountId)
        {
            var getProduct = _context.Products.Where(x => x.CategoryId == CategoryId && x.DiscountId==null && x.Active==true).ToListAsync();
            var getProductVM = _mapper.Map<Task<List<ProductVM>>>(getProduct);
            ViewBag.CategoryName = _context.Categories.FirstOrDefault(x => x.CategoryId == CategoryId).CategoryName;
            ViewBag.DiscountId = DiscountId;
            return View(await getProductVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductToBanner(List<int> ProductIDList,int DiscountId)
        {
            var getBannerStatus=_context.DiscountBanners.FirstOrDefault(x=>x.DiscountId== DiscountId);
            foreach(var item in ProductIDList)
            {
               await _context.Products.Where(x => x.ProductId == item).ExecuteUpdateAsync(set => set.
               SetProperty(x => x.DiscountId, DiscountId));
                //Nếu Thêm Sản Phẩm Vào Lúc Banner Đang Diễn Ra Thì Cập Nhật Thêm Tỉ Lệ Giảm Giá
                if (!Convert.ToBoolean(getBannerStatus.DisableDiscount))
                {
                    await _context.Products.Where(x => x.ProductId == item).ExecuteUpdateAsync(set => set.
                    SetProperty(x => x.Discount, getBannerStatus.DiscountRate));
                }
            }
            TempData["Success"] = "Đã Thêm Sản Phẩm Thành Công";
            return Json(new { redirectToUrl = Url.Action("DiscountDetail", new {id=DiscountId,CategoryId=getBannerStatus.CategoryId}) });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProductFromBanner(List<int> ProductIDList,int DiscountId)
        {
            foreach(var item in ProductIDList)
            {
                await _context.Products.Where(x => x.ProductId == item).ExecuteUpdateAsync(set => set.SetProperty(x => x.DiscountId, x => null).SetProperty(x=>x.Discount,0));
            }
            TempData["Success"] = "Xóa Khỏi Banner Thành Công";
            return Json(new { redirectToUrl = Url.Action("DiscountDetail", new { id = DiscountId }) });
        }
        private bool DiscountBannerExists(int id)
        {
            return (_context.DiscountBanners?.Any(e => e.DiscountId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> DiscountDetail(int id,int CategoryId)
        {
            var getProductListBaseOnDiscountId = _context.Products.Where(x => x.DiscountId == id).Include(x => x.Category).ToListAsync();
            var getProductListBaseOnDiscountIdVM = _mapper.Map<Task<List<ProductVM>>>(getProductListBaseOnDiscountId);
            ViewBag.DiscountName = _context.DiscountBanners.Find(id).DiscountName;
            ViewBag.CategoryId=CategoryId;
            ViewBag.DiscountId = id;
            return View(await getProductListBaseOnDiscountIdVM);
        }
    }
}
