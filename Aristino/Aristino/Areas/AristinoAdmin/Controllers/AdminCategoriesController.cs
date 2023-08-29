using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using Aristino.ViewModel;
using Aristino.Repository;
using AutoMapper;
using Aristino.Helper;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    [Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
    public class AdminCategoriesController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Category> _category;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminCategoriesController(AristinoDbContext context, IRepository<Category> category, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _category = category;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: AristinoAdmin/AdminCategories
        public async Task<IActionResult> Index()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Entity set 'AristinoDbContext.Categories'  is null.");
        }

        // GET: AristinoAdmin/AdminCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AristinoAdmin/AdminCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM categoryVM)
        {
            UploadImage upload = new(_webHostEnvironment);
            categoryVM.Avatar = categoryVM.CategoryName;
            if (!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
                return Json(new { redirectToUrl = Url.Action("Create", "AdminCategories") });
            }
            if (upload.CheckFileExtension(categoryVM.UploadImage))
            {
                TempData["Error"] = "Chỉ Hỗ Trợ Định Dạng Hình: .png,.jpg,.jpeg,.gif";
                return Json(new { redirectToUrl = Url.Action("Create") });
            }
            if (upload.CheckFileSize(categoryVM.UploadImage))
            {
                TempData["Error"] = "Hình Vượt Quá Giới Hạn, Giới Hạn Tối Đa 30MB";
                return Json(new { redirectToUrl = Url.Action("Create") });
            }
            if (categoryVM.UploadImage != null)
            {
                var controllerName = ControllerContext.ActionDescriptor.ControllerName;
                var getImage = await upload.Upload(categoryVM.CategoryName, categoryVM.UploadImage, controllerName);
                categoryVM.Avatar = getImage.Item1;
            }
            var category = _mapper.Map<Category>(categoryVM);
            await _category.AddEntity(category);
            _category.SaveChanges();
            TempData["Success"] = "Category added";
            return Json(new { redirectToUrl = Url.Action("Index", "AdminCategories") });
        }

        // GET: AristinoAdmin/AdminCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            var categoryVM = _mapper.Map<CategoryVM>(category);
            ViewBag.Image = categoryVM.Avatar.Split(",").ToList();
            ViewBag.CategoryName = categoryVM.CategoryName;
            if (category == null)
            {
                return NotFound();
            }
            return View(categoryVM);
        }

        // POST: AristinoAdmin/AdminCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryVM categoryVM)
        {
            UploadImage upload = new(_webHostEnvironment);
            categoryVM.Avatar = categoryVM.CategoryName;
            var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            if (id != categoryVM.CategoryId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
                return Json(new { redirectToUrl = Url.Action("Index", "AdminCategories") });
            }
            foreach (var item in _context.Categories.Where(x => x.CategoryName == categoryVM.CategoryName).AsNoTracking())
            {
                if (categoryVM.CategoryId == item.CategoryId)
                    continue;
                TempData["CategoryExisted"] = "Category Existed";
                return Json(new { redirectToUrl = Url.Action("Edit", "AdminCategories") });
            }
            if (upload.CheckDirExist(categoryVM.CategoryName, ControllerName))
            {
                var getOriginalName = _context.Categories.AsNoTracking().FirstOrDefault(x => x.CategoryId == id).CategoryName;
                upload.DeleteUploadFile(getOriginalName, ControllerName);
            }
            if (upload.CheckFileExtension(categoryVM.UploadImage))
            {
                TempData["Error"] = "Chỉ Hỗ Trợ Định Dạng Hình: .png,.jpg,.jpeg,.gif";
                return Json(new { redirectToUrl = Url.Action("Edit") });
            }
            if (upload.CheckFileSize(categoryVM.UploadImage))
            {
                TempData["Error"] = "Hình Vượt Quá Giới Hạn, Giới Hạn Tối Đa 30MB";
                return Json(new { redirectToUrl = Url.Action("Edit") });
            }
            if (categoryVM.UploadImage != null)
            {
                var getImage = await upload.Upload(categoryVM.CategoryName, categoryVM.UploadImage, ControllerName);
                categoryVM.Avatar = getImage.Item1;
            }
            var category = _mapper.Map<Category>(categoryVM);
            await _category.UpdateEntity(category);
            _category.SaveChanges();
            return Json(new { redirectToUrl = Url.Action("Index", "AdminCategories") });
        }
        // POST: AristinoAdmin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UploadImage upload = new(_webHostEnvironment);
            var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            if (_context.Categories == null)
            {
                return Problem("Entity set 'AristinoDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (_context.Products.FirstOrDefault(x => x.CategoryId == category.CategoryId) != null)
            {
                TempData["ConstraintError"] = "Có Sản Phẩm Đã Dùng Danh Mục Này, Không Thể Xóa";
                return RedirectToAction(nameof(Index));
            }
            if (upload.CheckDirExist(category.CategoryName, ControllerName))
            {
                upload.DeleteUploadFile(category.CategoryName, ControllerName);
            }
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMultiple(List<int> ProductIDList)
        {
            UploadImage upload = new(_webHostEnvironment);
            var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            foreach(var item in ProductIDList)
            {
                var getCategory = _context.Categories.FirstOrDefault(x => x.CategoryId == item);
                var checkIsUsedProduct = _context.Products.FirstOrDefault(x => x.CategoryId == item);
                var checkIsUsedDiscountBanner=_context.DiscountBanners.FirstOrDefault(x=>x.CategoryId == item);
                if(checkIsUsedProduct != null)
                {
                    TempData["Error"] = "Danh Mục Đã Có Sản Phẩm Sử Dụng, Không Thể Xóa";
                    return Json(new { redirectToUrl = Url.Action("Index"), isSuccess = "Failed" });
                }
                if (checkIsUsedDiscountBanner != null)
                {
                    TempData["Error"] = "Danh Mục Đã Có Banner Giảm Giá Sử Dụng, Không Thể Xóa";
                    return Json(new { redirectToUrl = Url.Action("Index"), isSuccess = "Failed" });
                }
                if (upload.CheckDirExist(getCategory.CategoryName, ControllerName))
                {
                    upload.DeleteUploadFile(getCategory.CategoryName, ControllerName);
                }
                _context.Categories.Remove(getCategory);
                _context.SaveChanges();
            }
            return Json(new { Ok = "OK" });
        }
        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
