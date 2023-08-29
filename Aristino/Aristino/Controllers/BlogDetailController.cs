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

namespace Aristino.Controllers
{
    public class BlogDetailController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Blog> _blog;
        private readonly IMapper _mapper;

        public BlogDetailController(AristinoDbContext context,IRepository<Blog> blog, IMapper mapper)
        {
            _context = context;
            _blog = blog;
            _mapper = mapper;
        }

        // GET: BlogDetail
        public async Task<IActionResult> Index(int id)
        {
            var blog = _context.Blogs.Where(x=>x.BlogId==id).Include(x=>x.Customer).AsNoTracking().ToListAsync();
            var blogVM=_mapper.Map<Task<List<BlogVM>>>(blog);

            //Lấy comment của người dùng đã đăng nhập
            if (Request.Cookies.ContainsKey("UserLogin"))
            {
                var customerID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
                ViewBag.BlogComment = _context.BlogComments.Where(x => x.BlogId == id && x.CustomerId == customerID).AsNoTracking().Include(x=>x.Customer).ToList();
            }
            var getBlog=_context.Blogs.AsNoTracking().FirstOrDefault(x=>x.BlogId== id);
            getBlog.BlogViews = getBlog.BlogViews + 1;
            _context.Blogs.Update(getBlog);
            _context.SaveChanges();
            ViewBag.CommentCount = _context.BlogComments.Where(x=>x.BlogId== id).AsNoTracking().Count();
            return View(await blogVM);
        }
    }
}
