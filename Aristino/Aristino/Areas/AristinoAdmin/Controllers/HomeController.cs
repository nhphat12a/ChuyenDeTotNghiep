using Aristino.Models;
using Aristino.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
	[Area("AristinoAdmin")]
	[Authorize(AuthenticationSchemes = "AdminLogin", Policy ="ForStaff")]
	public class HomeController : Controller
	{
		private readonly AristinoDbContext _context;
		private readonly IMapper _mapper;
		public HomeController(AristinoDbContext context,IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		public IActionResult Index()
		{
			var SalePerDay = new int[7];
			int TotalPrice = 0;
			int TotalSaleQuantity = 0;
			int TotalOriginalPrice = 0;
			int TotalSalePrice = 0;
			var SuccessfullyOrder = _context.Orders.Where(x => x.TransacId == 5).Include(x=>x.OrdersDetails);
			//Thống Kê Doanh thu đã bán qua từng ngày và tổng số lượng sản phẩm
			foreach(var item in SuccessfullyOrder.ToList())
			{
				if (item.PaymentDate != null)
				{
					switch (DateTime.Parse(item.PaymentDate.ToString()).DayOfWeek.ToString())
					{
						case "Monday":
							foreach (var orderDetail in item.OrdersDetails)
							{
								TotalSaleQuantity += orderDetail.Quantity;
								TotalPrice += orderDetail.Price;
								if (orderDetail.Discount > 0)
									TotalSalePrice += orderDetail.Price;
								if (orderDetail.Discount == 0)
									TotalOriginalPrice += orderDetail.Price;
							}
							SalePerDay[0] = TotalPrice;
							TotalPrice = 0;
							break;
						case "TuesDay":
							foreach (var orderDetail in item.OrdersDetails)
							{
								TotalSaleQuantity += orderDetail.Quantity;
								TotalPrice += orderDetail.Price;
								if (orderDetail.Discount > 0)
									TotalSalePrice += orderDetail.Price;
								if (orderDetail.Discount == 0)
									TotalOriginalPrice += orderDetail.Price;

							}
							SalePerDay[1] = TotalPrice;
							TotalPrice = 0;
							break;
						case "Wednesday":
							foreach (var orderDetail in item.OrdersDetails)
							{
								TotalSaleQuantity += orderDetail.Quantity;
								TotalPrice += orderDetail.Price;
								if (orderDetail.Discount > 0)
									TotalSalePrice += orderDetail.Price;
								if (orderDetail.Discount == 0)
									TotalOriginalPrice += orderDetail.Price;

							}
							SalePerDay[2] = TotalPrice;
							TotalPrice = 0;
							break;
						case "ThursDay":
							foreach (var orderDetail in item.OrdersDetails)
							{
								TotalSaleQuantity += orderDetail.Quantity;
								TotalPrice += orderDetail.Price;
								if (orderDetail.Discount > 0)
									TotalSalePrice += orderDetail.Price;
								if (orderDetail.Discount == 0)
									TotalOriginalPrice += orderDetail.Price;
							}
							SalePerDay[3] = TotalPrice;
							TotalPrice = 0;
							break;
						case "Friday":
							foreach (var orderDetail in item.OrdersDetails)
							{
								TotalSaleQuantity += orderDetail.Quantity;
								TotalPrice += orderDetail.Price;
								if (orderDetail.Discount > 0)
									TotalSalePrice += orderDetail.Price;
								if (orderDetail.Discount == 0)
									TotalOriginalPrice += orderDetail.Price;
							}
							SalePerDay[4] = TotalPrice;
							TotalPrice = 0;
							break;
						case "Saturday":
							foreach (var orderDetail in item.OrdersDetails)
							{
								TotalSaleQuantity += orderDetail.Quantity;
								TotalPrice += orderDetail.Price;
								if (orderDetail.Discount > 0)
									TotalSalePrice += orderDetail.Price;
								if (orderDetail.Discount == 0)
									TotalOriginalPrice += orderDetail.Price;
							}
							SalePerDay[5] = TotalPrice;
							TotalPrice = 0;
							break;
						case "Sunday":
							foreach (var orderDetail in item.OrdersDetails)
							{
								TotalSaleQuantity += orderDetail.Quantity;
								TotalPrice += orderDetail.Price;
								if (orderDetail.Discount > 0)
									TotalSalePrice += orderDetail.Price;
								if (orderDetail.Discount == 0)
									TotalOriginalPrice += orderDetail.Price;
							}
							SalePerDay[6] = TotalPrice;
							TotalPrice = 0;
							break;
						default:
							break;
					}
				}
			}
			ViewBag.SalePerDay = SalePerDay;
			ViewBag.TotalSaleQuantity = TotalSaleQuantity;
			ViewBag.TotalPrice=(TotalSalePrice+TotalOriginalPrice).ToString("C",System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
			//ViewBag.TotalOriginalPrice = TotalOriginalPrice;
			//ViewBag.TotalSalePrice=TotalSalePrice;
			ViewBag.MostSoldProduct = _context.MostSoldProducts.Include(x=>x.Product).OrderByDescending(x=>x.Sold).Take(10).ToList();
			ViewBag.TotalSuccessfullyOrder = SuccessfullyOrder.Count();
			DateTime now = DateTime.Now;
			ViewBag.MonthlyStatistic = "Thống Kê: " + new DateTime(now.Year, now.Month, 1).ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy");
			return View();
		}
	}
}
