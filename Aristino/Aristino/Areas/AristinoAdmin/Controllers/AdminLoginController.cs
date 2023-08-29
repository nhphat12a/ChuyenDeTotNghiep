using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    public class AdminLoginController : Controller
    {
        private readonly AristinoDbContext _context;

        public AdminLoginController(AristinoDbContext context)
        {
            _context = context;
        }
        [Route("AdminLogin/Login")]
        public IActionResult Login()
        {
            return View();
        }

    }
}
