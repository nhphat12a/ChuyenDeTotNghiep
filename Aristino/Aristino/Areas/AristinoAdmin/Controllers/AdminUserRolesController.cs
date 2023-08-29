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

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    public class AdminUserRolesController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<UserRole> _role;
        private readonly IMapper _mapper;

        public AdminUserRolesController(AristinoDbContext context, IRepository<UserRole> role, IMapper mapper)
        {
            _context = context;
            _role = role;
            _mapper = mapper;
        }

        // GET: AristinoAdmin/AdminUserRoles
        public async Task<IActionResult> Index()
        {
            var Roles = _context.UserRoles.ToListAsync();
            var RolesVM = _mapper.Map<Task<List<UserRoleVM>>>(Roles);
            return View(await RolesVM);
        }


        // POST: AristinoAdmin/AdminUserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(UserRoleVM userRoleVM)
        {
            //UserRoleVM userRoleVM = new();
            var UserRole = _mapper.Map<UserRole>(userRoleVM);
            await _role.AddEntity(UserRole);
            _role.SaveChanges();
            return Json(new { redirectToUrl = Url.Action("Index", "AdminUserRoles") });
        }

        // GET: AristinoAdmin/AdminUserRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserRoles == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRoles.FindAsync(id);
            var userRoleVM = _mapper.Map<UserRoleVM>(userRole);
            if (userRole == null)
            {
                return NotFound();
            }
            return View(userRoleVM);
        }

        // POST: AristinoAdmin/AdminUserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserRoleVM userRoleVM)
        {
            if (id != userRoleVM.RoleId)
            {
                return NotFound();
            }
            var userRole = _mapper.Map<UserRole>(userRoleVM);
            _role.UpdateEntity(userRole);
            _role.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: AristinoAdmin/AdminUserRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserRoles == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // POST: AristinoAdmin/AdminUserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserRoles == null)
            {
                return Problem("Entity set 'AristinoDbContext.UserRoles'  is null.");
            }
            var userRole = await _context.UserRoles.FindAsync(id);
            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRoleExists(int id)
        {
            return (_context.UserRoles?.Any(e => e.RoleId == id)).GetValueOrDefault();
        }
    }
}
