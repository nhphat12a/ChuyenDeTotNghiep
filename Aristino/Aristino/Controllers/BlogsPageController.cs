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
    public class BlogsPageController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Blog> _blog;
        private readonly IMapper _mapper;

        public BlogsPageController(AristinoDbContext context, IRepository<Blog> blog, IMapper mapper)
        {
            _context = context;
            _blog = blog;
            _mapper = mapper;
        }

        // GET: BlogsPage
        public async Task<IActionResult> AllBlog()
        {
            var blog = _blog.GetAllAsync();
            var blogVM = _mapper.Map<Task<List<BlogVM>>>(blog);
            ViewBag.BlogComment = _context.BlogComments.ToList();
            return View(await blogVM);
        }
    }
}
