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

namespace Aristino.Controllers
{
    public class CustomerOrdersController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Order> _order;
        private readonly IMapper _mapper;

        public CustomerOrdersController(AristinoDbContext context, IRepository<Order> order,IMapper mapper)
        {
            _context = context;
            _order = order;
            _mapper = mapper;
        }
        [Authorize]
        public async Task<IActionResult> AllOrder()
        {
            var CustomerId = 0;
            if (Request.Cookies.ContainsKey("UserLogin"))
            {
                CustomerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            }
            var order = _context.Orders.Where(x => x.CustomerId == CustomerId).Include(x => x.Payment).Include(x => x.Transac).Include(x => x.OrdersDetails).ThenInclude(x => x.Product).ToListAsync();
            var orderVM = _mapper.Map<Task<List<OrderVM>>>(order);
            ViewBag.CustomerDetail = _context.Customers.Where(x => x.CustomersId == CustomerId);
            ViewBag.AllOrderStatus = _context.TransactionStatuses.ToList();
            return View(await orderVM);
        }
        public async Task<IActionResult> FindOrderBaseOnStatus(int id)
        {
            var CustomerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            var order = _context.Orders.Where(x => x.TransacId == id && x.CustomerId == CustomerId).Include(x => x.Payment).Include(x => x.Transac).Include(x => x.OrdersDetails).ThenInclude(x => x.Product);
            ViewBag.CustomerDetail = _context.Customers.Where(x => x.CustomersId == CustomerId);
            ViewBag.AllOrderStatus = _context.TransactionStatuses.ToList();
            ViewBag.CurrentTransac = _context.TransactionStatuses.FirstOrDefault(x => x.TransacId == id).TransacName;
            var orderVM = _mapper.Map<Task<List<OrderVM>>>(order.ToListAsync());
            return View(await orderVM);
        }

    }
}
