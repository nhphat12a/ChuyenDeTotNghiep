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

namespace Aristino.Controllers
{
    public class LoginController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Account> _Account;
        private readonly IMapper _mapper;

        public LoginController(AristinoDbContext context, IRepository<Account> Account, IMapper mapper)
        {
            _context = context;
            _Account = Account;
            _mapper = mapper;
        }

        // GET: Login
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
