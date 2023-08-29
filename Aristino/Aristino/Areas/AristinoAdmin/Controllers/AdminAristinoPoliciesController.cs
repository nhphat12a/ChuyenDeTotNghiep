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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    [Authorize(AuthenticationSchemes ="AdminLogin",Policy ="ForStaff")]
    public class AdminAristinoPoliciesController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<AristinoPolicy> _policy;
        private readonly IMapper _mapper;

        public AdminAristinoPoliciesController(AristinoDbContext context,IRepository<AristinoPolicy> policy,IMapper mapper)
        {
            _context = context;
            _policy = policy;
            _mapper = mapper;
        }

        // GET: AristinoAdmin/AdminAristinoPolicies
        public async Task<IActionResult> Index()
        {
            var getPolicy = _policy.GetAllAsync();
            var getPolicyVM = _mapper.Map<Task<List<AristinoPolicyVM>>>(getPolicy);
            return View(await getPolicyVM);
        }

        // GET: AristinoAdmin/AdminAristinoPolicies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AristinoAdmin/AdminAristinoPolicies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AristinoPolicyVM aristinoPolicyVM)
        {
            if(!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return RedirectToAction("Create");
            }
            var checkExisted=_context.AristinoPolicies.FirstOrDefault(x=>x.PolicyTitle== aristinoPolicyVM.PolicyTitle);
            if(checkExisted!=null)
            {
                TempData["Error"] = "Tên Chính Sách Đã Được Dùng";
                return RedirectToAction("Create");
            }
            var policy = _mapper.Map<AristinoPolicy>(aristinoPolicyVM);
            await _policy.AddEntity(policy);
            _policy.SaveChanges();
            TempData["Success"] = "Thêm Thành Công";
            return RedirectToAction("Index");
        }

        // GET: AristinoAdmin/AdminAristinoPolicies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var getPolicy=_context.AristinoPolicies.FirstOrDefault(x=>x.PolicyId==id);
            var getPolicyVM = _mapper.Map<AristinoPolicyVM>(getPolicy);
            return View(getPolicyVM);
        }

        // POST: AristinoAdmin/AdminAristinoPolicies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,AristinoPolicyVM aristinoPolicyVM)
        {
            if(!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return RedirectToAction("Edit");
            }
            var checkExisted=_context.AristinoPolicies.AsNoTracking().FirstOrDefault(x=>x.PolicyTitle==aristinoPolicyVM.PolicyTitle && x.PolicyId!=aristinoPolicyVM.PolicyId);
            if(checkExisted!=null)
            {
                TempData["Error"] = "Tên Chính Sách Đã Được Dùng";
                return RedirectToAction("Edit");
            }
            var policy = _mapper.Map<AristinoPolicy>(aristinoPolicyVM);
            await _policy.UpdateEntity(policy);
            _policy.SaveChanges();
            TempData["Success"] = "Chỉnh Sửa Thành Công";
            return RedirectToAction("Index");
        }
        // POST: AristinoAdmin/AdminAristinoPolicies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AristinoPolicies == null)
            {
                return Problem("Entity set 'AristinoDbContext.AristinoPolicies'  is null.");
            }
            var aristinoPolicy = await _context.AristinoPolicies.FindAsync(id);
            if (aristinoPolicy != null)
            {
                _context.AristinoPolicies.Remove(aristinoPolicy);
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
                var getPolicy = _context.AristinoPolicies.FirstOrDefault(x => x.PolicyId == item);
                _context.AristinoPolicies.Remove(getPolicy);
                _context.SaveChanges();
            }
            return Json(new { Ok = "ok" });
        }
        private bool AristinoPolicyExists(int id)
        {
          return (_context.AristinoPolicies?.Any(e => e.PolicyId == id)).GetValueOrDefault();
        }
    }
}
