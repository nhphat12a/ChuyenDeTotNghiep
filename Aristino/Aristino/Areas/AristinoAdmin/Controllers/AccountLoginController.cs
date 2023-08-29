using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using Aristino.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Aristino.Repository;
using Aristino.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    public class AccountLoginController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Account> _account;

        public AccountLoginController(AristinoDbContext context, IRepository<Account> account)
        {
            _context = context;
            _account = account;
        }

        // GET: AristinoAdmin/AccountLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLogin(AccountVM accountVM)
        {
            if (accountVM.Username == null || accountVM.Passwords == null)
            {
                TempData["Error"] = "Vui Lòng Nhập Tài Khoản";
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            var userPassword = SHA256Encryptor.Encrypt(accountVM.Passwords);
            var CheckUsername = _context.Accounts.Include(x=>x.Role).Include(x=>x.Customer).ThenInclude(x=>x.Status).FirstOrDefault(x => x.Username == accountVM.Username && x.Passwords == userPassword);
            if (CheckUsername == null)
            {
                TempData["Error"] = "Sai Mật Khẩu Hoặc Tài Khoản Không Tồn Tại";
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            if(CheckUsername.Customer.StatusId==2)
            {
                TempData["Error"] = "Tài Khoản Của Bạn Đã Bị Khóa,Nếu Có Bất Cứ Thắc Mắc Vui Lòng Liên Hệ Đến Bộ Phận Hỗ Trợ Của Aristino";
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            var claimList = new List<Claim>
            {
                new Claim("Username",CheckUsername.Username),
                new Claim("Role",CheckUsername.Role.RoleName),
                new Claim("CustomerId",CheckUsername.CustomerId.ToString()),
                new Claim("Email",CheckUsername.Customer.Email),
                new Claim("Avatar",CheckUsername.Customer.Avatar),
            };
            var identity = new ClaimsIdentity(claimList,CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(AccountVM account)
        {
            if (account.Username == null || account.Passwords == null)
            {
                TempData["Error"] = "Vui Lòng Nhập Thông Tin Tài Khoản";
                return RedirectToAction("Login", "AdminLogin");
            }
            var EncryptPassword = SHA256Encryptor.Encrypt(account.Passwords);
            var checkAccountExist = _context.Accounts.Include(x=>x.Customer).Include(x=>x.Role).FirstOrDefault(x => x.Username == account.Username && x.Passwords == EncryptPassword);
            if(checkAccountExist== null)
            {
                TempData["Error"] = "Tài Khoản Không Tồn Tại";
                return RedirectToAction("Login", "AdminLogin");
            }
            if (_context.Accounts.FirstOrDefault(x => x.Username == account.Username && x.Passwords == EncryptPassword && x.RoleId == 6)!=null)
            {
                TempData["Error"] = "Tài Khoản Của Bạn Không Có Quyền Truy Cập";
                return RedirectToAction("Login", "AdminLogin");
            }
            var ClaimList = new List<Claim>
            {
                new Claim("CustomerId",checkAccountExist.CustomerId.ToString()),
                new Claim("Username",checkAccountExist.Username),
                new Claim("Role",checkAccountExist.Role.RoleName),
                new Claim("Email",checkAccountExist.Email),
                new Claim("Avatar",checkAccountExist.Customer.Avatar)
            };
            var AdminIdentity=new ClaimsIdentity(ClaimList,"AdminLogin");
            var AdminPricipal = new ClaimsPrincipal(AdminIdentity);
            await HttpContext.SignInAsync("AdminLogin", AdminPricipal);
            return RedirectToAction("Index", "Home", new { area = "AristinoAdmin" });
        }
        [AllowAnonymous]
        [Route("/Forbidden/403")]
        public IActionResult Forbidden()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public async Task<IActionResult> AdminLogout()
        {
            await HttpContext.SignOutAsync("AdminLogin");
            return RedirectToAction("Index", "Home", new { area = "AristinoAdmin" });
        }
	}
}
