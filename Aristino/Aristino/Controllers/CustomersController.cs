    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using AutoMapper;
using Aristino.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Aristino.Repository;
using Aristino.Helper;
using System.Drawing;
using Microsoft.Extensions.Hosting;

namespace Aristino.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IRepository<Customer> _customer;
        private readonly IWebHostEnvironment _environtment;

        public CustomersController(AristinoDbContext context, IMapper mapper, IRepository<Customer> customer, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _customer = customer;
            _environtment = environment;
        }
        [Authorize]
        public async Task<IActionResult> UserDetail()
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x=>x.Type=="CustomerId").Value);
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.CustomersId == userId);
            var customerVM = _mapper.Map<CustomerVM>(customer);
            ViewBag.CountryCode = customerVM.Country;
            if (customerVM.Avatar != null)
            {
                ViewBag.CustomerImage = customerVM.Avatar.Split(",").ToList();
            }
            ViewBag.CustomerEmail = customerVM.Email;
            return View(customerVM);
        }
        public async Task<IActionResult> PurchasedProducts()
        {
            var customerID =Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            var getOrderDetail = _context.OrdersDetails.Where(x=>x.Order.CustomerId==customerID && x.Order.TransacId==5).Include(x=>x.Order).Include(x=>x.Product).ToListAsync();
            var getOrderVM = _mapper.Map<Task<List<OrderDetailVM>>>(getOrderDetail);
            return View(await getOrderVM);
        }
        [HttpPost, ActionName("UserDetail")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserInfoChange(CustomerVM customerVM, string country_selector_code)
        {
            UploadImage upload = new(_environtment);
            var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            if(!ModelState.IsValid)
            {
                TempData["Error"]=String.Join("<br>",ModelState.Values.SelectMany(x=> x.Errors).Select(x=>x.ErrorMessage));
                return Json(new { redirectToUrl = Url.Action("UserDetail", "Customers") });
            }
            if (upload.CheckDirExist(customerVM.Email, ControllerName))
            {
                upload.DeleteUploadFile(customerVM.Email, ControllerName);
            }
            if (customerVM.UploadImage != null) 
            {
                var getImageName = await upload.Upload(customerVM.Email, customerVM.UploadImage, ControllerName);
                customerVM.Avatar = getImageName.Item1;
            }
            else
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
                var FolderPath = Path.Combine(_environtment.WebRootPath, @"uploads\" + ControllerName, customerVM.Email);
                Directory.CreateDirectory(FolderPath);
                var filePath = Path.Combine(FolderPath, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(fileStream);
                customerVM.Avatar = file.FileName;
            }
            
            customerVM.Country = country_selector_code;
            var customer = _mapper.Map<Customer>(customerVM);
            _customer.UpdateEntity(customer);
            _customer.SaveChanges();
            TempData["Success"] = "Saved";
            return Json(new { redirectToUrl = Url.Action("UserDetail", "Customers") });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePaymentMethod(CustomerVM customerVM)
        {
            //Excute Update là update một dòng cụ thể mà không cần phải update tất cả bảng
            var customerID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            if(customerVM.CardNumber==null||customerVM.CardNumber.Length != 16)
            {
                TempData["Error"] = "Số Thẻ Phải Chứa 16 Chữ Số";
                return RedirectToAction("UserDetail");
            }
            if(customerVM.CardOwner==null)
            {
                TempData["Error"] = "Tên Chủ Thẻ Không Được Rỗng";
                return RedirectToAction("UserDetail");
            }
            if(customerVM.ExpiredDate==null)
            {
                TempData["Error"] = "Ngày Hết Hạn Không Được Rỗng";
                return RedirectToAction("UserDetail");
            }
            if(customerVM.Cvc==null||(customerVM.Cvc.ToString().Length != 3))
            {
                TempData["Error"] = "Mã CVC Phải Chứa 3 Chữ Số";
                return RedirectToAction("UserDetail");
            }
            _context.Customers.Where(x => x.CustomersId == customerID).
                ExecuteUpdate(set =>
                set.SetProperty(x => x.CardNumber,customerVM.CardNumber).
                SetProperty(x=>x.CardOwner,customerVM.CardOwner).
                SetProperty(x=>x.ExpiredDate,customerVM.ExpiredDate).
                SetProperty(x=>x.Cvc,customerVM.Cvc)
                );
            TempData["Success"] = "Thêm Thanh Toán Thành Công";
            return RedirectToAction("UserDetail");
        }
        [Authorize(AuthenticationSchemes ="AdminLogin",Policy ="ForStaff")]
        [Route("Admin/ChangeDetail")]
        public async Task<IActionResult> UpdateInformation()
        {
            var StaffId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            var getStaffInfo = _context.Customers.Find(StaffId);
            var getStaffInfoVM = _mapper.Map<CustomerVM>(getStaffInfo);
            ViewData["Gender"] = new SelectList(_context.Genders, "GenderId", "GenderName", getStaffInfo.GenderId);
            ViewBag.Image = getStaffInfo.Avatar.Split(",").ToList();
            ViewBag.Email = getStaffInfo.Email;
            ViewBag.CountryCode = getStaffInfo.Country;
            return View(getStaffInfoVM);
        }

        // POST: AristinoAdmin/AdminAccountDetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Admin/ChangeDetail")]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
        public async Task<IActionResult> UpdateInformation(CustomerVM customerVM, string country_selector_code)
        {
            UploadImage upload = new(_environtment);
            var staffId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            var getStaffEmail = _context.Customers.AsNoTracking().FirstOrDefault(x => x.CustomersId == staffId).Email;
            var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            if (!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Json(new { redirectToUrl = Url.Action("UpdateInformation", "Customers") });
            }
            var checkEmailExisted = _context.Customers.AsNoTracking().FirstOrDefault(x => x.Email == customerVM.Email);
            if (checkEmailExisted != null)
            {
                var isSelfEmail = _context.Customers.AsNoTracking().FirstOrDefault(x => x.Email == customerVM.Email && x.CustomersId==staffId);
                if (isSelfEmail == null)
                {
                    TempData["Error"] = "Email Đã Tồn Tại";
                    return Json(new { redirectToUrl = Url.Action("UpdateInformation", "Customers") });
                }
            }
            if (upload.CheckDirExist(getStaffEmail, ControllerName))
            {
                upload.DeleteUploadFile(getStaffEmail, ControllerName);
            }
            if (customerVM.UploadImage != null)
            {
                var getImageName = await upload.Upload(customerVM.Email, customerVM.UploadImage, ControllerName);
                customerVM.Avatar = getImageName.Item1;
            }
            else
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
                var FolderPath = Path.Combine(_environtment.WebRootPath, @"uploads\" + ControllerName, customerVM.Email);
                Directory.CreateDirectory(FolderPath);
                var filePath = Path.Combine(FolderPath, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(fileStream);
                customerVM.Avatar = file.FileName;
            }
            customerVM.Country = country_selector_code;
            var customer = _mapper.Map<Customer>(customerVM);
            _context.Customers.Update(customer);
            _context.SaveChanges();
            TempData["Success"] = "Cập Nhật Thông Tin Thành Công";
            return Json(new { redirectToUrl = Url.Action("Index", "AdminAccountDetail", new {area="AristinoAdmin"}) });
        }
    }
}
