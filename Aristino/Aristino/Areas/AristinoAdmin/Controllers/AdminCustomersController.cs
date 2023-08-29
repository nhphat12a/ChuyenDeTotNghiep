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
using Microsoft.AspNetCore.Authorization;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    [Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
    public class AdminCustomersController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Customer> _customer;
        private readonly IMapper _mappers;
        public AdminCustomersController(AristinoDbContext context,IRepository<Customer> customer,IMapper mapper)
        {
            _context = context;
            _customer = customer;
            _mappers = mapper;
        }

        // GET: AristinoAdmin/AdminCustomers
        public async Task<IActionResult> Index()
        {
            var customer = _customer.GetAll().Include(x=>x.Gender).Include(x=>x.Status).ToListAsync();
            var customerVM=_mappers.Map<Task<List<CustomerVM>>>(customer);
            return View(await customerVM);
        }
        // GET: AristinoAdmin/AdminCustomers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var customer = _context.Customers.FirstOrDefault(x=>x.CustomersId==id);
            var customerVM = _mappers.Map<CustomerVM>(customer);
            ViewData["Status"] = new SelectList(_context.UserStatuses, "StatusId", "StatusName");
            ViewData["Gender"] = new SelectList(_context.Genders, "GenderId", "GenderName");
            return View(customerVM);
        }

        // POST: AristinoAdmin/AdminCustomers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerVM customerVM)
        {
            if(!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return RedirectToAction("Edit");
            }
            var customer = _mappers.Map<Customer>(customerVM);
			if (customerVM.StatusId == 1)
			{
				_context.Accounts.Where(x => x.CustomerId == customerVM.CustomersId).ExecuteUpdate(set => set.SetProperty(x => x.AccountStatus, true));
			}
			if (customerVM.StatusId == 2)
            {
                _context.Accounts.Where(x => x.CustomerId == customerVM.CustomersId).ExecuteUpdate(set => set.SetProperty(x => x.AccountStatus, false));
            }
            await _customer.UpdateEntity(customer);
            _customer.SaveChanges();
            TempData["Success"] = "Cập Nhật Thành Công";
            return RedirectToAction("Index");
        }
        // POST: AristinoAdmin/AdminCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'AristinoDbContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            var getCustomerAccount = _context.Accounts.FirstOrDefault(x => x.CustomerId == id);
            var getComment=_context.Comments.FirstOrDefault(x=>x.CustomerId== id);
            var getOrder=_context.Orders.Where(x=>x.CustomerId== id);
            var getBlogComment = _context.BlogComments.Where(x => x.CustomerId == id);
            var getCart=_context.Carts.Where(x=>x.CustomerId== id);
            var getWishlist=_context.Wishlists.Where(x=>x.CustomerId== id);
            if (customer != null)
            {
                if (getOrder.ToList() != null)
                {
                    TempData["Error"] = "Người Dùng Này Đã Phát Sinh Hóa Đơn, Không Thể Xóa, Chỉ Có Thể Thiết Lập Active Hoặc Block";
                    return RedirectToAction("Index");
                }
                if (getBlogComment.ToList() != null)
                {
                    foreach (var item in getBlogComment.ToList())
                    {
                        _context.BlogComments.Remove(item);
                    }
                }
                if (getCart.ToList() != null)
                {
                    foreach (var item in getCart.ToList())
                        _context.Carts.Remove(item);
                }
                if (getWishlist.ToList() != null)
                {
                    foreach (var item in getWishlist.ToList())
                        _context.Wishlists.Remove(item);
                }
                if (getComment!= null)
                {
                    _context.Comments.Remove(getComment);
                }
                if (getCustomerAccount != null)
                {
                    _context.Accounts.Remove(getCustomerAccount);
                }
                _context.SaveChanges();
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xoá Thành Công";
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.CustomersId == id)).GetValueOrDefault();
        }
    }
}
