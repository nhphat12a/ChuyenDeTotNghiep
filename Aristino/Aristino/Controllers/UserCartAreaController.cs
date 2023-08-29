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
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aristino.Controllers
{
    [Authorize]
    public class UserCartAreaController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Wishlist> _wishlist;
        private readonly IRepository<Cart> _cart;
        private readonly IMapper _mapper;

        public UserCartAreaController(AristinoDbContext context,IRepository<Wishlist> wishlist, IMapper mapper, IRepository<Cart> cart)
        {
            _context = context;
            _wishlist = wishlist;
            _mapper = mapper;
            _cart = cart;
        }

        // GET: UserCartArea
        [Authorize]
        //truyền ID được lưu từ claim cho Cart
        public async Task<IActionResult> Cart()
        {
            var customerId = Convert.ToInt32(User.Claims.FirstOrDefault(x=>x.Type=="CustomerId").Value);
            var cart=_context.Carts.Where(x=>x.CustomerId==customerId).Include(x=>x.Product).ThenInclude(x=>x.DiscountNavigation).ToListAsync();
            var cartVM = _mapper.Map<Task<List<CartVM>>>(cart);
            ViewBag.PaymentMethod = _context.Payments.ToList();
            var EndSaleList = new List<string>();
            foreach(var item in _context.Carts.Where(x => x.CustomerId == customerId).Include(x => x.Product).ThenInclude(x => x.DiscountNavigation).ToList())
            {
                if (item.Product.Discount != 0)
                {
                    if(DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm"))>=DateTime.Parse(item.Product.DiscountNavigation.EndSale))
                    {
                        _context.DiscountBanners.Where(x => x.DiscountId == item.Product.DiscountId).ExecuteUpdate(set =>
                        set.SetProperty(x => x.DisableDiscount, true));
                        _context.Products.Where(x => x.DiscountId == item.Product.DiscountId).ExecuteUpdate(set =>
                        set.SetProperty(x => x.Discount, 0).SetProperty(x => x.DiscountId, x => null));
                    }
                }
            }
            foreach (var item in await cartVM)
            {
                if (item.Product.Discount != 0)
                {
                    EndSaleList.Add(item.Product.DiscountNavigation.EndSale);
                }
            }
            ViewBag.EndSaleList = EndSaleList;
            return View(await cartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCart(int getProductId,string ColorName,string SizeName,int getQuantity)
        {
            var customerId = 0;
            if (User.Identity.IsAuthenticated)
            {
                customerId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "CustomerId").Value);
            }
            var checkWishlistExist = _context.Wishlists.AsNoTracking().FirstOrDefault(x => x.ProductId == getProductId && x.CustomerId==customerId);
            var checkCartExist=_context.Carts.AsNoTracking().FirstOrDefault(x=>x.Color==ColorName && x.Size==SizeName && x.CustomerId==customerId);
            var InStock = _context.Products.FirstOrDefault(x => x.ProductId == getProductId);
			if (checkCartExist != null)
            {
                string ErrorMsg = "Sản Phẩm Này Đã Có Trong Giỏ Hàng";
                return Json(new { message = ErrorMsg, isSuccess="Failed" });
            }
            if(ColorName==null||SizeName==null||getQuantity==null)
            {
                string ErrorMsg = "Vui Lòng Chọn Số Lượng Và Kích Cỡ";
                return Json(new { message = ErrorMsg, isSuccess = "Failed" });
            }
            if(getQuantity>InStock.Quantity)
            {
                string ErrorMsg = "Số Lượng Bạn Chọn Vượt Quá Số Lượng Trong Kho";
                return Json(new { message = ErrorMsg, isSuccess = "Failed" });
			}
            if (Convert.ToBoolean(InStock.ProductDiscontinue))
            {
                string ErrorMsg = "Sản Phẩm Đã Ngừng Kinh Doanh";
                return Json(new { message = ErrorMsg, isSuccess = "Failed" });
            }
            CartVM cartVM = new CartVM
            {
                CustomerId = customerId,
                ProductId = getProductId,
                Quantity = getQuantity,
                Color = ColorName,
                Size = SizeName,
            };
            var cart = _mapper.Map<Cart>(cartVM);
            await _cart.AddEntity(cart);
            _cart.SaveChanges();
            string Success = "Đã Thêm Vào Giỏ Hàng";
			//Xoá sản phẩm khỏi wishlist khi thêm vào Cart
			if (checkWishlistExist != null)
			{
				await _wishlist.DeleteEntity(checkWishlistExist);
				_wishlist.SaveChanges();
			}
            return Json(new { message = Success });

		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItemFromCart(int id)
        {
            var customerId = User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value;
            var cart = _context.Carts.FirstOrDefault(x => x.CartId == id);
            await _cart.DeleteEntity(cart);
            _cart.SaveChanges();
            TempData["Success"] = "Đã Xóa";
            //return RedirectToAction("Cart", new { id = customerId });
            //return PartialView("SidebarPartialView");
            string referer = Request.Headers["Referer"];
            return Redirect(referer);
        }
        [Authorize]
        public async Task<IActionResult> Wishlists()
        {
            var customerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            var wishlist = _context.Wishlists.Where(x => x.CustomerId == customerId).Include(x=>x.Product).ToListAsync();
            var wishlistVM = _mapper.Map<Task<List<WishlistVM>>>(wishlist);
            return View(await wishlistVM);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToWishlist(int getProductId, string ColorName, string SizeName, int getQuantity)
        {
            string Referer = Request.Headers["Referer"];
            var customerId = 0;
            if (User.Identity.IsAuthenticated)
            {
                customerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            }
            var checkItemInWishlist = _context.Wishlists.Include(x=>x.Product).FirstOrDefault(x => x.ProductId == getProductId && x.CustomerId==customerId);
            if (checkItemInWishlist != null)
            {
                string ErrorMsg = "Sản Phẩm Đã Tồn Tại Trong DS Yêu Thích";
                return Json(new { message = ErrorMsg, isSuccess = "Failed" });
            }
            if(ColorName==null||SizeName==null||getQuantity==null)
            {
                string ErrorMsg = "Vui Lòng Chọn Size Và Kích Cỡ";
                return Json(new { message = ErrorMsg, isSuccess = "Failed" });
			}
            if (Convert.ToBoolean(checkItemInWishlist.Product.ProductDiscontinue))
            {
                string ErrorMsg = "Sản Phẩm Đã Ngừng Kinh Doanh";
                return Json(new { message = ErrorMsg, isSuccess = "Failed" });
            }
            WishlistVM wishlistVM = new WishlistVM
            {
                CustomerId = customerId,
                ProductId = getProductId,
                Color=ColorName,
                Size=SizeName,
                Quantity=getQuantity
			};
            var wishlist = _mapper.Map<Wishlist>(wishlistVM);
            await _wishlist.AddEntity(wishlist);
            _wishlist.SaveChanges();
            string Success = "Đã Thêm Vào DS Yêu Thích";
            return Json(new { message = Success });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItemWishList(int id)
        {
            var wishlist = _context.Wishlists.FirstOrDefault(x => x.WishlistId == id);
            await _wishlist.DeleteEntity(wishlist);
            _wishlist.SaveChanges();
            TempData["Success"] = "Đã Xóa";
            return RedirectToAction("Wishlists");
        }
    }
}
