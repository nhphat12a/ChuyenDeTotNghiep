using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Aristino.ViewModel;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
	[Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
	public class AdminSizesController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IMapper _mapper;

        public AdminSizesController(AristinoDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper= mapper;
        }

        // GET: AristinoAdmin/AdminSizes
        public async Task<IActionResult> Index()
        {
            return _context.Sizes != null ?
                        View(await _context.Sizes.ToListAsync()) :
                        Problem("Entity set 'AristinoDbContext.Sizes'  is null.");
        }
        // GET: AristinoAdmin/AdminSizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AristinoAdmin/AdminSizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SizeVM sizeVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return RedirectToAction("Create");
            }
            var checkExisted=_context.Sizes.FirstOrDefault(x=>x.SizeName==sizeVM.SizeName);
            if (checkExisted != null)
            {
                TempData["Error"] = "Tên Size Đã Tồn Tại";
                return RedirectToAction("Create");
            }
            var size=_mapper.Map<Size>(sizeVM);
            await _context.Sizes.AddAsync(size);
            _context.SaveChanges();
            TempData["Success"] = "Thêm Thành Công";
            return RedirectToAction("Index");
        }

        // GET: AristinoAdmin/AdminSizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sizes == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes.FindAsync(id);
            var sizeVM = _mapper.Map<SizeVM>(size);
            if (size == null)
            {
                return NotFound();
            }
            return View(sizeVM);
        }

        // POST: AristinoAdmin/AdminSizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,SizeVM sizeVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return RedirectToAction("Edit");
            }
            var checkExisted = _context.Sizes.AsNoTracking().FirstOrDefault(x => x.SizeName == sizeVM.SizeName && x.SizeId!=sizeVM.SizeId);
            if (checkExisted != null)
            {
                TempData["Error"] = "Tên Size Đã Tồn Tại";
                return RedirectToAction("Edit");
            }
            var size = _mapper.Map<Size>(sizeVM);
            _context.Sizes.Update(size);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập Nhật Thành Công";
            return RedirectToAction("Index");
        }

        // POST: AristinoAdmin/AdminSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sizes == null)
            {
                return Problem("Entity set 'AristinoDbContext.Sizes'  is null.");
            }
            var size = await _context.Sizes.FindAsync(id);
            var checkIsUsed = _context.Products.FirstOrDefault(x => x.Size.Contains(size.SizeName));
            if (checkIsUsed != null)
            {
                TempData["Error"] = "Size Này Đã Có Sản Phẩm Sử Dụng, Không Thể Xóa";
                return RedirectToAction("Index");
            }
            if (size != null)
            {
                _context.Sizes.Remove(size);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMultiple(List<int> ProductIDList)
        {
            foreach(var item in ProductIDList)
            {
                var getSize = _context.Sizes.FirstOrDefault(x => x.SizeId == item);
                var checkIsUsed = _context.Products.FirstOrDefault(x => x.Size.Contains(getSize.SizeName));
                if (checkIsUsed != null)
                {
                    TempData["Error"] = "Size Này Đã Có Sản Phẩm Sử Dụng, Không Thể Xóa";
                    return Json(new { redirectToUrl = Url.Action("Index"), isSuccess = "Failed" });
                }
                _context.Sizes.Remove(getSize);
                await _context.SaveChangesAsync();
            }
            return Json(new { Ok = "ok" });
        }
        private bool SizeExists(int id)
        {
            return (_context.Sizes?.Any(e => e.SizeId == id)).GetValueOrDefault();
        }
    }
}
