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
using Aristino.Helper;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    [Authorize(AuthenticationSchemes ="AdminLogin",Policy ="ForStaff")]
    public class AdminAccountDetailController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Customer> _account;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public AdminAccountDetailController(AristinoDbContext context,IRepository<Customer> account,IMapper mapper,IWebHostEnvironment environment)
        {
            _context = context;
            _account = account;
            _mapper = mapper;
            _environment= environment;
        }

        // GET: AristinoAdmin/AdminAccountDetail
        public async Task<IActionResult> Index()
        {
            var staffID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            var aristinoDbContext = _context.Customers.Include(x=>x.Gender).Include(a => a.Accounts).ThenInclude(x=>x.Role).Where(x=>x.CustomersId==staffID).ToListAsync();
            var employee = _mapper.Map<Task<List<CustomerVM>>>(aristinoDbContext);
            return View(await employee);
        }
    }
}
