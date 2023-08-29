using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using AutoMapper;
using Aristino.Repository;
using Aristino.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    [Authorize(AuthenticationSchemes ="AdminLogin",Policy ="ForStaff")]
    public class AdminAboutUsController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IRepository<AboutU> _aboutus;

        public AdminAboutUsController(AristinoDbContext context,IMapper mapper,IRepository<AboutU> aboutus)
        {
            _context = context;
            _aboutus = aboutus;
            _mapper = mapper;
        }

        // GET: AristinoAdmin/AdminAboutUs
        public async Task<IActionResult> Index()
        {
            var getAboutus = _aboutus.GetAllAsync();
            var getaboutusVM = _mapper.Map<Task<List<AboutUsVM>>>(getAboutus);
            return View(await getaboutusVM);
        }

        // GET: AristinoAdmin/AdminAboutUs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AristinoAdmin/AdminAboutUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutUsVM aboutUsVM)
        {
            if(!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return RedirectToAction("Create");
            }
            var aboutus=_mapper.Map<AboutU>(aboutUsVM);
            await _aboutus.AddEntity(aboutus);
            _aboutus.SaveChanges();
            TempData["Success"] = "Thêm Thành Công";
            return RedirectToAction("Index");
        }

        // GET: AristinoAdmin/AdminAboutUs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AboutUs == null)
            {
                return NotFound();
            }

            var aboutU = await _context.AboutUs.FindAsync(id);
            if (aboutU == null)
            {
                return NotFound();
            }
            return View(aboutU);
        }

        // POST: AristinoAdmin/AdminAboutUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShopAddress,City,Province,PhoneNumber,Email")] AboutU aboutU)
        {
            if (id != aboutU.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutU);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutUExists(aboutU.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aboutU);
        }

        // POST: AristinoAdmin/AdminAboutUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AboutUs == null)
            {
                return Problem("Entity set 'AristinoDbContext.AboutUs'  is null.");
            }
            var aboutU = await _context.AboutUs.FindAsync(id);
            if (aboutU != null)
            {
                _context.AboutUs.Remove(aboutU);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutUExists(int id)
        {
          return (_context.AboutUs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
