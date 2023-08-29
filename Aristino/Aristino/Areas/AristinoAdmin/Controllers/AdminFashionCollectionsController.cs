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
using Aristino.Helper;
using System.Net;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    [Authorize(AuthenticationSchemes ="AdminLogin",Policy ="ForStaff")]
    public class AdminFashionCollectionsController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<FashionCollection> _fashion;
        private readonly IMapper _mappers;
        private readonly IWebHostEnvironment _env;

        public AdminFashionCollectionsController(AristinoDbContext context,IRepository<FashionCollection> fashion,IMapper mapper,IWebHostEnvironment env)
        {
            _context = context;
            _fashion = fashion;
            _mappers = mapper;
            _env = env;
        }

        // GET: AristinoAdmin/AdminFashionCollections
        public async Task<IActionResult> Index()
        {
            var getCollection = _fashion.GetAllAsync();
            var getCollectionVM = _mappers.Map<Task<List<FashionCollectionVM>>>(getCollection);
            return View(await getCollectionVM);
        }

        // GET: AristinoAdmin/AdminFashionCollections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var getProduct = _context.Products.Where(x => x.CollectionId == id).ToListAsync();
            var getProductVM=_mappers.Map<Task<List<ProductVM>>>(getProduct);
            var Collection=_context.FashionCollections.FirstOrDefault(x => x.CollectionId == id);
            ViewBag.CollectionName = Collection.CollectionName;
            ViewBag.CollectionId = Collection.CollectionId;
            return View(await getProductVM);
        }
        public async Task<IActionResult> AddProductToCollection(int CollectionId)
        {
            var getAllProduct = _context.Products.Where(x => x.CollectionId == null).ToListAsync();
            var getAllProductVM = _mappers.Map<Task<List<ProductVM>>>(getAllProduct);
            var Collection=_context.FashionCollections.FirstOrDefault(x=>x.CollectionId==CollectionId).CollectionName;
            ViewBag.CollectionName = Collection;
            ViewBag.CollectionID = CollectionId;
            return View(await getAllProductVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductToCollection(List<int> ProductIDList,int CollectionId)
        {
            foreach(var item in ProductIDList)
            {
               await _context.Products.Where(x => x.ProductId == item).ExecuteUpdateAsync(set => set.SetProperty(x => x.CollectionId, CollectionId));
            }
            TempData["Success"] = "Thêm Thành Công";
            return Json(new { redirectToUrl = Url.Action("Details", new { id = CollectionId }) });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProductFromCollection(List<int> ProductIDList)
        {
            foreach(var item in ProductIDList)
            {
              await _context.Products.Where(x => x.ProductId == item).ExecuteUpdateAsync(set => set.SetProperty(x => x.CollectionId, x => null));
            }
            return Json(new { Ok = "Ok" });
        }
        // GET: AristinoAdmin/AdminFashionCollections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AristinoAdmin/AdminFashionCollections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FashionCollectionVM fashionCollectionVM,bool ActiveButton)
        {
            UploadImage upload = new(_env);
            var ImageName = ("", "");
            var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            var checkExisted = _context.FashionCollections.FirstOrDefault(x => x.CollectionName == fashionCollectionVM.CollectionName);
            if (!ModelState.IsValid)
            {
                TempData["ErrorList"]=String.Join("<br>",ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage));
                return Json(new {redirectToUrl = Url.Action("Create") });
            }
            if(checkExisted!=null)
            {
                TempData["Error"] = "Tên Bộ Sưu Tập Đã Được Dùng";
                return Json(new {redirectToUrl = Url.Action("Create") });
            }
            if(fashionCollectionVM.UploadImage!=null)
            {
                ImageName= await upload.Upload(fashionCollectionVM.CollectionName, fashionCollectionVM.UploadImage, ControllerName);
                fashionCollectionVM.CollectionThumbnail = ImageName.Item1;
            }
            if(upload.CheckFileExtension(fashionCollectionVM.UploadImage))
            {
                TempData["Error"] = "Chỉ Hỗ Trợ Định Dạng Hình: .png,.jpg,.jpeg,.gif";
                return Json(new { redirectToUrl = Url.Action("Create") });
            }
            if(upload.CheckFileSize(fashionCollectionVM.UploadImage))
            {
                TempData["Error"] = "Hình Vượt Quá Giới Hạn, Giới Hạn Tối Đa 30MB";
                return Json(new { redirectToUrl = Url.Action("Create") });
            }
            fashionCollectionVM.CollectionDisable = ActiveButton;
            var fashionCollection = _mappers.Map<FashionCollection>(fashionCollectionVM);
            await _fashion.AddEntity(fashionCollection);
            _fashion.SaveChanges();
            TempData["Success"] = "Tạo Thành Công";
            return Json(new { redirectToUrl = Url.Action("Index") });
        }

        // GET: AristinoAdmin/AdminFashionCollections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var getFashionCollection = _context.FashionCollections.FirstOrDefault(x => x.CollectionId == id);
            var getFashionCollectionVM = _mappers.Map<FashionCollectionVM>(getFashionCollection);
            ViewBag.Image = getFashionCollectionVM.CollectionThumbnail.Split(",").ToList();
            ViewBag.CollectionName = getFashionCollectionVM.CollectionName;
            return View(getFashionCollectionVM);
        }

        // POST: AristinoAdmin/AdminFashionCollections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,FashionCollectionVM fashionCollectionVM,bool ActiveButton)
        {
            UploadImage upload = new(_env);
            var ControllerName=ControllerContext.ActionDescriptor.ControllerName;
            var CheckExisted = _context.FashionCollections.Where(x => x.CollectionName == fashionCollectionVM.CollectionName).AsNoTracking().ToList();
            var getOriginalName = _context.FashionCollections.AsNoTracking().FirstOrDefault(x => x.CollectionId == fashionCollectionVM.CollectionId).CollectionName;
            var ImageName = ("", "");
            if (!ModelState.IsValid)
            {
                TempData["ErrorList"]=String.Join("<br>",ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage));
                return Json(new {redirectToUrl=Url.Action("Edit") });
            }
            foreach(var item in CheckExisted)
            {
                if (fashionCollectionVM.CollectionId == item.CollectionId)
                    continue;
                TempData["Error"] = "Tên Collection Đã Được Sử Dụng";
                return Json(new { redirectToUrl = Url.Action("Edit") });
            }
            if (upload.CheckFileExtension(fashionCollectionVM.UploadImage))
            {
                TempData["Error"] = "Chỉ Hỗ Trợ Định Dạng Hình: .png,.jpg,.jpeg,.gif";
                return Json(new { redirectToUrl = Url.Action("Edit") });
            }
            if (upload.CheckFileSize(fashionCollectionVM.UploadImage))
            {
                TempData["Error"] = "Hình Vượt Quá Giới Hạn, Giới Hạn Tối Đa 30MB";
                return Json(new { redirectToUrl = Url.Action("Edit") });
            }
            if (upload.CheckDirExist(getOriginalName, ControllerName))
            {
                upload.DeleteUploadFile(getOriginalName, ControllerName);
            }
            if (fashionCollectionVM.UploadImage != null)
            {
               ImageName = await upload.Upload(fashionCollectionVM.CollectionName, fashionCollectionVM.UploadImage, ControllerName);
                fashionCollectionVM.CollectionThumbnail = ImageName.Item1;
            }
            fashionCollectionVM.CollectionDisable = ActiveButton;
            var fashionCollection = _mappers.Map<FashionCollection>(fashionCollectionVM);
            await _fashion.UpdateEntity(fashionCollection);
            _fashion.SaveChanges();
            TempData["Success"] = "Cập Nhật Thành Công";
            return Json(new { redirectToUrl = Url.Action("Index") });
        }

        // POST: AristinoAdmin/AdminFashionCollections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UploadImage upload = new(_env);
            var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            var getCollection = _context.FashionCollections.FirstOrDefault(x => x.CollectionId == id);
            var getAllProduct = _context.Products.Where(x => x.CollectionId == id).ToList();
            if (getCollection != null)
            {
                if (upload.CheckDirExist(getCollection.CollectionName, ControllerName))
                {
                    upload.DeleteUploadFile(getCollection.CollectionName, ControllerName);
                }
                foreach(var item in getAllProduct)
                {
                   await _context.Products.Where(x => x.ProductId == item.ProductId).ExecuteUpdateAsync(set => set.SetProperty(x => x.CollectionId, x => null));
                }
                _context.FashionCollections.Remove(getCollection);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMultiple(List<int> ProductIDList)
        {
            UploadImage upload = new(_env);
            var ControllerName=ControllerContext.ActionDescriptor.ControllerName;
            foreach(var item in ProductIDList)
            {
                var getCollection = _context.FashionCollections.FirstOrDefault(x => x.CollectionId == item);
                var getProduct = _context.Products.Where(x => x.CollectionId == item).ToList();
                if (upload.CheckDirExist(getCollection.CollectionName,ControllerName))
                {
                    upload.DeleteUploadFile(getCollection.CollectionName,ControllerName);
                }
                foreach(var product in getProduct)
                {
                   await _context.Products.Where(x => x.ProductId == product.ProductId).
                        ExecuteUpdateAsync(set => set.SetProperty(x => x.CollectionId, x => null));
                }
                _context.FashionCollections.Remove(getCollection);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        private bool FashionCollectionExists(int id)
        {
          return (_context.FashionCollections?.Any(e => e.CollectionId == id)).GetValueOrDefault();
        }
    }
}
