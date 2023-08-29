using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Aristino.ViewModel;
using Humanizer;
using Aristino.Helper;
using System.Globalization;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    [Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
    public class AdminStatisticController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IMapper _mapper;
        public AdminStatisticController(AristinoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: AristinoAdmin/AdminStatistic
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DateTime FromDate, DateTime ToDate)
        {
            int TotalPrice = 0;
            if(FromDate.ToString("MM/dd/yyyy").Equals("01/01/0001") || ToDate.ToString("MM/dd/yyyy").Equals("01/01/0001"))
            {
                TempData["Error"] = "Vui Lòng Chọn Ngày Để Tìm Kiếm";
                return RedirectToAction("Index");
            }
            if(FromDate>ToDate)
            {
                TempData["Error"] = "Ngày Bắt Đầu Không Thể Lớn Hơn Ngày Kết Thúc";
                return RedirectToAction("Index");
            }
            var FromDateConvert =  StartAndEndOfDay.StartOfDay(FromDate);
            var ToDateConvert=StartAndEndOfDay.EndOfDay(ToDate);
            var getOrder = _context.OrdersDetails.
               Where(x =>x.Order.OrderDate >= FromDateConvert && x.Order.OrderDate
                <= ToDateConvert && x.Order.TransacId==5).Include(x => x.Order).Include(x => x.Product).ToListAsync();
            var getOrderVM=_mapper.Map<Task<List<OrderDetailVM>>>(getOrder);
            ViewBag.StartAndEndDate = ":" + FromDate.ToString("MM/dd/yyyy") + " - " + ToDate.ToString("MM/dd/yyyy");
            foreach(var item in await getOrderVM)
            {
                TotalPrice += item.Price;
            }
            ViewBag.TotalPrice = TotalPrice.ToString("C",System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
            return View(await getOrderVM);

        }
    }
}
