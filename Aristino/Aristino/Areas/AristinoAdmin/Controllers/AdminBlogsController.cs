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
using Microsoft.VisualBasic;
using Aristino.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
	public class AdminBlogsController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Blog> _blog;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public AdminBlogsController(AristinoDbContext context, IRepository<Blog> blog,IMapper mapper, IWebHostEnvironment environment)
        {
            _context = context;
            _blog = blog;
            _mapper = mapper;
            _environment = environment;
        }
		[Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
		// GET: AristinoAdmin/AdminBlogs
		public async Task<IActionResult> Index()
        {
            var blog = _context.Blogs.Include(x=>x.Customer).ToListAsync();
            var blogVM = _mapper.Map<Task<List<BlogVM>>>(blog);
            return View(await blogVM);
        }

		[Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
		// GET: AristinoAdmin/AdminBlogs/Create
		public IActionResult Create()
        {
            return View();
        }

        // POST: AristinoAdmin/AdminBlogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
		public async Task<IActionResult> Create(BlogVM blogVM)
        {
            UploadImage upload = new(_environment);
            var StaffId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            if(!ModelState.IsValid)
            {
                TempData["Error"] = String.Join("<br>", ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
                return Json(new { redirectToUrl = Url.Action("Create", "AdminBlogs") });

			}
            if(blogVM.UploadImage!=null)
            {
                var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
                var GetImageName= await upload.Upload(blogVM.BlogTitle, blogVM.UploadImage, ControllerName);
                blogVM.Thumbnail = GetImageName.Item1;
            }
            blogVM.CustomerId= StaffId;
            blogVM.PostDate = DateTime.Now.ToString();
            blogVM.BlogViews = 0;
            blogVM.BlogLike = 0;
            var blog = _mapper.Map<Blog>(blogVM);
            await _blog.AddEntity(blog);
            _blog.SaveChanges();
            TempData["Success"] = "New Blog Added";
            return Json(new {redirectToUrl=Url.Action("Index","AdminBlogs")});
        }

		// GET: AristinoAdmin/AdminBlogs/Edit/5
		[Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            var blogVM = _mapper.Map<BlogVM>(blog);
            ViewBag.Image = blogVM.Thumbnail.Split(",").ToList();
            ViewBag.Name = blogVM.BlogTitle;
            if (blog == null)
            {
                return NotFound();
            }
            return View(blogVM);
        }

        // POST: AristinoAdmin/AdminBlogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(AuthenticationSchemes = "AdminLogin", Policy = "ForStaff")]
		public async Task<IActionResult> Edit(int id, BlogVM blogVM)
        {
            UploadImage upload = new(_environment);
            var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            var CheckBlogExisted = _context.Blogs.Where(x => x.BlogTitle == blogVM.BlogTitle).AsNoTracking();
            var getBlogThumbnail = _context.Blogs.AsNoTracking().FirstOrDefault(x => x.BlogId == id).Thumbnail;
            if(!ModelState.IsValid)
            {
                TempData["Error"] = String.Join("<br>", ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
                return Json(new { redirectToUrl = Url.Action("Edit", "AdminBlogs") });

			}
            foreach(var item in CheckBlogExisted)
            {
                if (blogVM.BlogId == item.BlogId)
                    continue;
                TempData["Error"] = "This Name Is Used By Another Blog";
                return Json(new { redirectToUrl = Url.Action("Edit", "AdminBlogs") });

			}
            if(upload.CheckDirExist(getBlogThumbnail,ControllerName))
            {
                upload.DeleteUploadFile(getBlogThumbnail,ControllerName);
            }
            var GetImageName = await upload.Upload(blogVM.BlogTitle, blogVM.UploadImage, ControllerName);
            blogVM.Thumbnail = GetImageName.Item1;
            blogVM.PostDate = DateTime.Now.ToString();
            var blog=_mapper.Map<Blog>(blogVM);
            await _blog.UpdateEntity(blog);
            _blog.SaveChanges();
            return Json(new { redirectToUrl = Url.Action("Index", "AdminBlogs") });

		}
        // POST: AristinoAdmin/AdminBlogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UploadImage upload = new(_environment);
            var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            if (_context.Blogs == null)
            {
                return Problem("Entity set 'AristinoDbContext.Blogs'  is null.");
            }
            var blog = await _context.Blogs.FindAsync(id);
            if(upload.CheckDirExist(blog.BlogTitle,ControllerName))
            {
                upload.DeleteUploadFile(blog.BlogTitle,ControllerName);
            }
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LikeBlog(int blogId)
        {
            var getBlog = _context.Blogs.AsNoTracking().FirstOrDefault(x => x.BlogId == blogId);
            getBlog.BlogLike += 1;
            _context.Blogs.Update(getBlog);
            _context.SaveChanges();
            string Success = "Liked";
            return Json(new {message=Success});
        }
        private bool BlogExists(int id)
        {
          return (_context.Blogs?.Any(e => e.BlogId == id)).GetValueOrDefault();
        }
    }
}
