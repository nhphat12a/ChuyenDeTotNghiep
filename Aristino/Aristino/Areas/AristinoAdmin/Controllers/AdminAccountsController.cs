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
using System.Security.Claims;
using Aristino.Helper;
using Microsoft.AspNetCore.Authorization;
using Aristino.SendMail;
using System.Drawing;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    [Authorize(AuthenticationSchemes ="AdminLogin",Policy ="AdminOnly")]
	public class AdminAccountsController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Account> _account;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _environtment;

        public AdminAccountsController(AristinoDbContext context, IRepository<Account> account, IMapper mapper,IMailService mailService,IWebHostEnvironment environment)
        {
            _context = context;
            _account = account;
            _mapper = mapper;
            _mailService = mailService;
            _environtment = environment;
        }
		[Authorize(Policy = "AdminOnly")]
		// GET: AristinoAdmin/AdminAccounts
		public async Task<IActionResult> Index()
        {
            var aristinoDbContext = _context.Accounts.Include(a => a.Role).ToListAsync();
            var accountVM = _mapper.Map<Task<List<AccountVM>>>(aristinoDbContext);
            return View(await accountVM);
        }
		[Authorize(Policy = "AdminOnly")]
		public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.UserRoles, "RoleId", "Name");
            ViewData["Gender"] = new SelectList(_context.Genders, "GenderId", "GenderName");
            return View();
        }

        // POST: AristinoAdmin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountVM accountVM)
        {
            var ExistedUsername = _context.Accounts.FirstOrDefault(x => x.Username == accountVM.Username);
            var ExistedEmail = _context.Customers.FirstOrDefault(x => x.Email == accountVM.Email);
            if (!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return RedirectToAction("Create");
            }
            if(ExistedUsername!=null || ExistedEmail!=null)
            {
                TempData["Error"] = "Username or Email Is Used";
                return RedirectToAction("Create");
            }
            var customer=_mapper.Map<Customer>(accountVM);
            if (customer.Avatar == null)
            {
                string DefaultAVT = Path.Combine(_environtment.WebRootPath, @"img\DefaultAvatar\DefaultAvatar.jpg");
                Image image = Image.FromFile(DefaultAVT);
                byte[] ImageData;
                using (var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, image.RawFormat);
                    ImageData = memoryStream.ToArray();
                }
                var file = new FormFile(new MemoryStream(ImageData), 0, ImageData.Length, "DefaultAvatar", "DefaultAvatar.jpg");
                var FolderPath = Path.Combine(_environtment.WebRootPath, @"uploads\" + "Customers", customer.Email);
                Directory.CreateDirectory(FolderPath);
                var filePath = Path.Combine(FolderPath, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(fileStream);
                customer.Avatar = file.FileName;
            }
            customer.StatusId = 1;
            customer.CreatedDate=DateTime.Now;
            _context.Customers.Add(customer);
            _context.SaveChanges();
            accountVM.CustomerId = customer.CustomersId;
            accountVM.RoleId = 4;
            accountVM.Passwords=SHA256Encryptor.Encrypt(accountVM.Passwords);
            accountVM.AccountStatus = true;
            accountVM.CreatedDate=DateTime.Now;
            var account=_mapper.Map<Account>(accountVM);
            await _account.AddEntity(account);
            _account.SaveChanges();
            TempData["Success"] = "Đã Tạo Tài Khoản";
            return RedirectToAction("Index");
        }
		// GET: AristinoAdmin/AdminAccounts/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.UserRoles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserAccount(AccountVM accountVM)
        {
            var ExistedUserName = _context.Accounts.FirstOrDefault(x => x.Username == accountVM.Username);
            var ExistedEmail = _context.Accounts.FirstOrDefault(x => x.Email == accountVM.Email);
            if (!ModelState.IsValid)
            {
                TempData["InputError"] = String.Join("<br>", ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            if (accountVM.ConfirmPasswords != accountVM.Passwords)
            {
                TempData["Error"] = "Confirm Password is not correct";
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            if (ExistedEmail != null || ExistedUserName != null)
            {
                TempData["Error"] = "Username or Email are used";
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            var PhoneNumber = Convert.ToInt32(accountVM.PhoneNumber);
            var customer = _mapper.Map<Customer>(accountVM);
            if (customer.Avatar == null)
            {
                string DefaultAVT = Path.Combine(_environtment.WebRootPath, @"img\DefaultAvatar\DefaultAvatar.jpg");
                Image image = Image.FromFile(DefaultAVT);
                byte[] ImageData;
                using (var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, image.RawFormat);
                    ImageData = memoryStream.ToArray();
                }
                var file = new FormFile(new MemoryStream(ImageData), 0, ImageData.Length, "DefaultAvatar", "DefaultAvatar.jpg");
                var FolderPath = Path.Combine(_environtment.WebRootPath, @"uploads\" + "Customers", customer.Email);
                Directory.CreateDirectory(FolderPath);
                var filePath = Path.Combine(FolderPath, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(fileStream);
                customer.Avatar = file.FileName;
            }
            customer.PhoneNumber=PhoneNumber;
            customer.StatusId = 1;
            _context.Customers.Add(customer);
            _context.SaveChanges();
            var encryptedPassword = SHA256Encryptor.Encrypt(accountVM.Passwords);
            accountVM.RoleId = 6;
            accountVM.AccountStatus = true;
            accountVM.CreatedDate = DateTime.Now;
            accountVM.Passwords = encryptedPassword;
            accountVM.CustomerId = customer.CustomersId;
            var account = _mapper.Map<Account>(accountVM);
            await _account.AddEntity(account);
            _account.SaveChanges();
            TempData["Success"] = "Registered Success Fully";
            MailRequest mailRequest = new()
            {
                ToEmail = accountVM.Email,
                Subject = "Thanks You",
                MailBody = "Thank You For Register Our Shop"
            };
            await _mailService.SendMailAsync(mailRequest);
            return RedirectToAction("Index", "Login", new { area = "" });

        }
        // POST: AristinoAdmin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Username,Passwords,RoleId,AccountStatus,CreatedDate")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
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
            ViewData["RoleId"] = new SelectList(_context.UserRoles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }

		// GET: AristinoAdmin/AdminAccounts/Delete/5
		[Authorize(Policy = "AdminOnly")]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: AristinoAdmin/AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'AristinoDbContext.Accounts'  is null.");
            }
            var account = _context.Accounts.Include(x=>x.Customer).FirstOrDefault(x=>x.AccountId==id);
            var getOrder = _context.Orders.FirstOrDefault(x => x.CustomerId == account.CustomerId);
			var getBlogComment = _context.BlogComments.Where(x => x.CustomerId == account.CustomerId);
			var getCart = _context.Carts.Where(x => x.CustomerId == account.CustomerId);
			var getWishlist = _context.Wishlists.Where(x => x.CustomerId == account.CustomerId);
			var getComment = _context.Comments.FirstOrDefault(x => x.CustomerId == account.CustomerId);
            var getCustomer = _context.Customers.FirstOrDefault(x => x.CustomersId == account.CustomerId);
			if (getOrder != null)
            {
                TempData["Error"] = "Người Dùng Này Đã Phát Sinh Hóa Đơn, Không Thể Xóa, Chỉ Có Thể Thiết Lập Active Hoặc Block";
                return RedirectToAction("Index");
			}
            if (account != null)
            {
				if (getBlogComment.ToList() != null)
				{
					foreach (var item in getBlogComment.ToList())
					{
						_context.BlogComments.Remove(item);
                        await _context.SaveChangesAsync();
                    }
				}
				if (getCart.ToList() != null)
				{
                    foreach (var item in getCart.ToList())
                    {
                        _context.Carts.Remove(item);
                        await _context.SaveChangesAsync();
                    }
				}
				if (getWishlist.ToList() != null)
				{
                    foreach (var item in getWishlist.ToList())
                    {
                        _context.Wishlists.Remove(item);
                    }
				}
				if (getComment != null)
				{
					_context.Comments.Remove(getComment);
                    await _context.SaveChangesAsync();
                }
				_context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
                if (getCustomer != null)
                {
                    _context.Customers.Remove(getCustomer);
                    await _context.SaveChangesAsync();
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
