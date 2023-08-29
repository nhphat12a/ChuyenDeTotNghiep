using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using Aristino.ViewModel;
using Aristino.Helper;
using Aristino.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
	public class AdminOrdersController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Order> _order;
        private readonly IRepository<OrdersDetail> _orderDetail;
        private readonly IRepository<MostSoldProduct> _mostSold;
        private readonly IMapper _mapper;

        public AdminOrdersController(AristinoDbContext context,IRepository<Order> order, IMapper mapper, IRepository<OrdersDetail> orderDettail, IRepository<MostSoldProduct> mostSold)
        {
            _context = context;
            _order = order;
            _orderDetail = orderDettail;
            _mapper = mapper;
            _mostSold = mostSold;
        }

        // GET: AristinoAdmin/AdminOrders
        [Authorize(AuthenticationSchemes ="AdminLogin",Policy ="ForStaff")]
        public async Task<IActionResult> Index()
        {
            var aristinoDbContext = _context.Orders.Include(o => o.Customer).Include(o => o.Payment).Include(o => o.Transac).ToListAsync();
            var orderVM = _mapper.Map<Task<List<OrderVM>>>(aristinoDbContext);
            return View(await orderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(OrderDetailVM orderDetailVM)
        {
            var customerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);          
            var getUserAddress = _context.Customers.Find(customerId);
            var TotalPrice = 0;
            if(getUserAddress.CustomerAddress == null || getUserAddress.PhoneNumber==null)
            {
                string ErrorMsg = "Bạn Chưa Nhập Thông Tin Địa Chỉ Và Số Điện Thoại";
                return Json(new { message = ErrorMsg, isSuccess = "Failed" });
            }
            if(orderDetailVM.PaymentId==0)
            {
                string ErrorMsg = "Vui Lòng Chọn Phương Thức Thanh Toán";
                return Json(new { message = ErrorMsg, isSuccess = "Failed" });
            }
            if(orderDetailVM.PaymentId==1 )
            { 
                if(getUserAddress.CardNumber == null || getUserAddress.Cvc == null || getUserAddress.CardOwner == null || getUserAddress.ExpiredDate == null)

                    {
                        string ErrorMsg = "Vui Lòng Nhập Đầy Đủ Thông Tin Thẻ Visa";
                        return Json(new { message = ErrorMsg, isSuccess = "Failed" });
                    }
            }
            foreach(var item in orderDetailVM.QuantityList)
            {
                var i = 0;
                var InStock = _context.Products.FirstOrDefault(x => x.ProductId == orderDetailVM.ProductIdList[i]).Quantity;
                if(item>InStock)
                {
                    string ErrorMsg = "Số Lượng Sản Phẩm Bạn Chọn Vượt Quá Số Lượng Trong Kho";
                    return Json(new { message = ErrorMsg, isSuccess = "Failed" });
				}
                i++;
            }
            foreach(var item in orderDetailVM.QuantityList)
            {
                if (item == 0)
                {
                    string ErrorMsg = "Vui Lòng Nhập Số Lượng Muốn Mua";
                    return Json(new { message = ErrorMsg, isSuccess = "Failed" });
                }
            }
            foreach(var item in orderDetailVM.ProductIdList)
            {
                var getProduct = _context.Products.FirstOrDefault(x => x.ProductId == item);
                if (Convert.ToBoolean(getProduct.ProductDiscontinue))
                {
                    string ErrorMsg = "Có Sản Phẩm Đã Ngừng Kinh Doanh, Không Thể Đặt Hàng";
                    return Json(new { message = ErrorMsg, isSuccess = "Failed" });
                }
                if (getProduct.Quantity == 0)
                {
                    string ErrorMsg = "Có Sản Phẩm Đã Hết Hàng, Không Thể Đặt Hàng";
                    return Json(new { message = ErrorMsg, isSuccess = "Failed" });
                }
            }
            for(int i = 0; i < orderDetailVM.ProductIdList.Count(); i++)
            {
                var getProduct = _context.Products.FirstOrDefault(x => x.ProductId == orderDetailVM.ProductIdList[i]);
                if (getProduct.Discount != 0)
                {
                    var DiscountPrice = orderDetailVM.QuantityList[i] * (getProduct.Price - (getProduct.Price * Convert.ToDecimal(getProduct.Discount)/100));
                    TotalPrice += Convert.ToInt32(DiscountPrice);
                }
                else
                {
                    TotalPrice += getProduct.Price * orderDetailVM.QuantityList[i];
                }
            }
            OrderVM orderVM = new()
            {
                OrderNumber = OrderNumberGenerator.Generate(),
                CustomerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value),
                PaymentId = orderDetailVM.PaymentId,
                TransacId = 1,
                IsPaid = false,
                ShipDate =DateTime.Now.AddDays(10),
                OrderDate=DateTime.Now,
                UpdateDate=DateTime.Now,
                PaymentDate=null,
                TotalPrice=TotalPrice,
            };
            var order=_mapper.Map<Order>(orderVM);
            await _order.AddEntity(order);
            _order.SaveChanges();
            for(int i=0;i<orderDetailVM.ProductIdList.Count();i++)
            {
                var getProductPrice = _context.Products.FirstOrDefault(x => x.ProductId == orderDetailVM.ProductIdList[i]);
                var getDiscount = _context.Products.FirstOrDefault(x => x.ProductId == orderDetailVM.ProductIdList[i]).Discount;
                orderDetailVM.OrderId = order.OrderId;
                orderDetailVM.ProductId = orderDetailVM.ProductIdList[i];
                orderDetailVM.Quantity = orderDetailVM.QuantityList[i];
                if(getDiscount!=0)
                {
                    orderDetailVM.ProductPrice = Convert.ToInt32(getProductPrice.Price-(getProductPrice.Price*Convert.ToDecimal(getProductPrice.Discount)/100));
                    orderDetailVM.Price = Convert.ToInt32(orderDetailVM.Quantity * (orderDetailVM.ProductPrice));
                    orderDetailVM.Discount = getDiscount;
                }
                else
                {
                    orderDetailVM.ProductPrice = getProductPrice.Price;
                    orderDetailVM.Price = orderDetailVM.Quantity * getProductPrice.Price;
                    orderDetailVM.Discount = 0;
                }
                orderDetailVM.Size = orderDetailVM.SizeList[i];
                orderDetailVM.Color = orderDetailVM.ColorList[i];
                var orderDetail = _mapper.Map<OrdersDetail>(orderDetailVM);
                await _orderDetail.AddEntity(orderDetail);
                _orderDetail.SaveChanges();
            }
            var deleteCart = _context.Carts.Where(x => x.CustomerId == customerId).ToList();
            foreach (var cart in deleteCart)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
            string Success = "Order Successfully";
            return Json(new { message = Success });
        }

        // GET: AristinoAdmin/AdminOrders/Details/5
        [Authorize(AuthenticationSchemes ="AdminLogin",Policy ="ForStaff")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var orderDetail = _context.OrdersDetails.Where(x => x.OrderId == id).Include(x=>x.Product).ToListAsync();
            var orderDetailVM = _mapper.Map<Task<List<OrderDetailVM>>>(orderDetail);
            ViewBag.OrderNumber = _context.Orders.FirstOrDefault(x => x.OrderId == id).OrderNumber;
            return View(await orderDetailVM);
        }
        // GET: AristinoAdmin/AdminOrders/Edit/5
        [Authorize(AuthenticationSchemes ="AdminLogin",Policy ="ForStaff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            var orderVM=_mapper.Map<OrderVM>(order);
            if (order == null)
            {
                return NotFound();
            }
			//Đơn hàng đang ở trạng thái nào thì không được quay lại trạng thái trước đó và mỗi lần cập nhật là chỉ cập nhật trạng tiếp theo
			var Transac = _context.TransactionStatuses.Where(x=>x.TransacId>orderVM.TransacId).FirstOrDefault();
            var TransacSelectList = new List<TransactionStatusVM>();
            if (Transac != null)
            {
                TransactionStatusVM trasac = new()
                {
                    TransacId = Transac.TransacId,
                    TransacName = Transac.TransacName,
                };
                TransacSelectList.Add(trasac);
            }   
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomersId", "LastName", order.CustomerId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentName", order.PaymentId);
            ViewData["TransacId"] = new SelectList(TransacSelectList, "TransacId", "TransacName", order.TransacId);
            return View(orderVM);
        }

        // POST: AristinoAdmin/AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
        public async Task<IActionResult> ChangeTransacStatus(OrderVM orderVM)
            {
            if(!ModelState.IsValid)
            {
                TempData["ErrorList"] = String.Join("<br>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return RedirectToAction("Edit", "AdminOrders");
            }
            var getUserOrderDetail=_context.OrdersDetails.Where(x=>x.OrderId== orderVM.OrderId).ToList();
            //Khi đơn hàng sang trạng thái đang lấy hàng thì sẽ trừ số lượng hiện có trong kho
            if(orderVM.TransacId==2)
            {
                foreach(var item in getUserOrderDetail)
                {
                    var getProduct=_context.Products.AsNoTracking().FirstOrDefault(x=>x.ProductId==item.ProductId);
                    if (getProduct.Quantity <= 0)
                    {
                        TempData["Error"] = "This Product Is Out Of Stock";
                        var getOutOfStockTransac = _context.TransactionStatuses.OrderBy(x=>x.TransacId).LastOrDefault();
                        var TransacViewData = new List<TransactionStatusVM>();
                        TransactionStatusVM transaction = new()
                        {
                            TransacId = getOutOfStockTransac.TransacId,
                            TransacName = getOutOfStockTransac.TransacName,
                        };
                        TransacViewData.Add(transaction);
                        ViewData["TransacId"] = new SelectList(TransacViewData, "TransacId", "TransacName");
						ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomersId", "LastName", orderVM.CustomerId);
						ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentName", orderVM.PaymentId);
						return View(orderVM);
                    }
                    if(item.Quantity>getProduct.Quantity)
                    {
                        TempData["Warning"] = "Số Lượng Trong Kho Của Sản Phẩm Này Không Đủ, Vui Lòng Liên Hệ Tới Khách Hàng";
                    }
                    getProduct.Quantity -= item.Quantity;
                        //Không cho số lượng sản phẩm bị âm
                    if (getProduct.Quantity <= 0)
                    {
                            getProduct.Quantity = 0;
                    }
                    _context.Products.Update(getProduct);
                    _context.SaveChanges();
                }
            }
            if(orderVM.TransacId==5)
            {
                orderVM.IsPaid = true;
                orderVM.PaymentDate= DateTime.Now;
                var mostSoldProductVM = new MostSoldProductVM();
                //Kiểm tra xem sản phẩm có tồn tại trong table chưa, nếu rồi thì cộng số lượng đã bán
                foreach(var item in getUserOrderDetail)
                {
                    if (_context.MostSoldProducts.FirstOrDefault(x => x.ProductId == item.ProductId) == null)
                    {
                        mostSoldProductVM.ProductId=item.ProductId;
                        mostSoldProductVM.Sold = item.Quantity;
                        mostSoldProductVM.IsReset = false;
                        var mostSoldProduct=_mapper.Map<MostSoldProduct>(mostSoldProductVM);
                        await _mostSold.AddEntity(mostSoldProduct);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var getSold = _context.MostSoldProducts.AsNoTracking().FirstOrDefault(x => x.ProductId == item.ProductId).Sold;
                        getSold += item.Quantity;
                        _context.MostSoldProducts.Where(x => x.ProductId == item.ProductId).ExecuteUpdate(set =>
                        set.SetProperty(x => x.Sold, getSold));
                    }
                }
            }
            orderVM.UpdateDate = DateTime.Now;
            var order = _mapper.Map<Order>(orderVM);
            await _order.UpdateEntity(order);
            _order.SaveChanges();
            TempData["Success"] = "Cập Nhật Trạng Thái Thành Công";
            return RedirectToAction("Edit", new {id=orderVM.OrderId});
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var getTransacId=_context.OrdersDetails.AsNoTracking().Include(x=>x.Order).FirstOrDefault(x=>x.OrderId== id);
			if (getTransacId.Order.TransacId==2)
            {
				var getOrderDetail = _context.OrdersDetails.AsNoTracking().Where(x => x.OrderId == id).ToList();
				foreach (var item in getOrderDetail)
                {
                    var getProduct=_context.Products.AsNoTracking().FirstOrDefault(x=>x.ProductId==item.ProductId);
                    getProduct.Quantity+= item.Quantity;
                    _context.Products.Update(getProduct);
                    _context.SaveChanges();
                }
            }
            var getOrder = _context.Orders.AsNoTracking().FirstOrDefault(x=>x.OrderId== id);
            getOrder.TransacId = 6;
            _order.UpdateEntity(getOrder);
            _order.SaveChanges();
            string Success = "Your Order Have Been Canceled";
            return Json(new {message=Success});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int OrderID)
        {
            var getOrder = _context.Orders.FirstOrDefault(x=>x.OrderId==OrderID);
            var getOrderDetail=_context.OrdersDetails.Where(x=>x.OrderId==getOrder.OrderId);
            if(getOrder.TransacId >= 3)
            {
                TempData["Error"] = "Không Thể Xóa Hóa Đơn Này";
                return RedirectToAction("Index");
            }
            foreach (var item in getOrderDetail.ToList())
            {
                _context.OrdersDetails.Remove(item);
                await _context.SaveChangesAsync();
            }
            _context.Orders.Remove(getOrder);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa Thành Công";
            return RedirectToAction("Index");
        }
    }

}
