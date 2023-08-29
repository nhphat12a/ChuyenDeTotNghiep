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
	public class AdminColorsController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IMapper _mapper;
        public AdminColorsController(AristinoDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: AristinoAdmin/AdminColors
        public async Task<IActionResult> Index()
        {
            return _context.Colors != null ?
                        View(await _context.Colors.ToListAsync()) :
                        Problem("Entity set 'AristinoDbContext.Colors'  is null.");
        }


        // GET: AristinoAdmin/AdminColors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AristinoAdmin/AdminColors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorVM colorVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorList"] =String.Join("<br>",ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return RedirectToAction("Create");
            }
            var checkExisted = _context.Colors.FirstOrDefault(x => x.ColorName == colorVM.ColorName);
            if (checkExisted != null)
            {
                TempData["Error"] = "Tên Màu Đã Sử Dụng";
                return RedirectToAction("Create");
            }
                var color=_mapper.Map<Color>(colorVM);
                _context.Colors.Add(color);
                await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm Thành Công";
                return RedirectToAction(nameof(Index));
        }

        // GET: AristinoAdmin/AdminColors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Colors == null)
            {
                return NotFound();
            }

            var color = await _context.Colors.FindAsync(id);
            var colorVM = _mapper.Map<ColorVM>(color);
            if (color == null)
            {
                return NotFound();
            }
            return View(colorVM);
        }

        // POST: AristinoAdmin/AdminColors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ColorVM colorVM)
        {
            if (id != colorVM.ColorId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                TempData["Error"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToString();
                return RedirectToAction("Edit");
            }
            var checkExisted = _context.Colors.AsNoTracking().FirstOrDefault(x => x.ColorName == colorVM.ColorName && x.ColorId!=colorVM.ColorId);
            if(checkExisted != null)
            {
                TempData["Error"] = "Tên Màu Đã Sử Dụng";
                return RedirectToAction("Edit");
            }
            var color = _mapper.Map<Color>(colorVM);
            _context.Colors.Update(color);
            _context.SaveChanges();
            TempData["Success"] = "Cập Nhật Thành Công";
            return RedirectToAction("Index");
        }
        // POST: AristinoAdmin/AdminColors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Colors == null)
            {
                return Problem("Entity set 'AristinoDbContext.Colors'  is null.");
            }
            var color = await _context.Colors.FindAsync(id);
            var checkIsUsed = _context.Products.FirstOrDefault(x => x.Color.Contains(color.ColorName));

            if(checkIsUsed!=null)
            {
                TempData["Error"] = "Màu Này Đã Có Sản Phẩm Sử Dụng, Không Thể Xóa";
                return RedirectToAction("Index");
            }
            if (color != null)
            {
                _context.Colors.Remove(color);
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
                var getColor = _context.Colors.FirstOrDefault(x => x.ColorId == item);
                var checkIsUse = _context.Products.FirstOrDefault(x => x.Color.Contains(getColor.ColorName));
                if(checkIsUse!=null)
                {
                    TempData["Error"] = "Màu Này Đã Có Sản Phẩm Sử Dụng, Không Thể Xóa";
                    return Json(new { redirectToUrl = Url.Action("Index"), isSuccess = "Failed" });
                }
                _context.Colors.Remove(getColor);
                await _context.SaveChangesAsync();
            }
            return Json(new { OK = "Ok" });
        }
        private bool ColorExists(int id)
        {
            return (_context.Colors?.Any(e => e.ColorId == id)).GetValueOrDefault();
        }
    }
}
