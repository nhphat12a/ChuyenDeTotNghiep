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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Area("AristinoAdmin")]
    public class AdminCommentsController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IRepository<Comment> _comment;
        private readonly IRepository<BlogComment> _blogComment;
        private readonly IMapper _mapper;

        public AdminCommentsController(AristinoDbContext context,IRepository<Comment> comment,IMapper mapper,IRepository<BlogComment> blogComment)
        {
            _context = context;
            _comment = comment;
            _mapper = mapper;
            _blogComment = blogComment;
        }

        // GET: AristinoAdmin/AdminComments
        public async Task<IActionResult> Index()
        {
            var aristinoDbContext = _context.Comments.Include(c => c.Customer);
            return View(await aristinoDbContext.ToListAsync());
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentVM commentVM)
        {
            var CustomerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            if(!ModelState.IsValid)
            {
                string ErrorMsg = String.Join("<br>", ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
                return Json(new { message = ErrorMsg, isSuccess = "Failed" });
            }
            if (commentVM.StarRating == 0)
            {
                string ErrorMsg = "Vui Lòng Đánh Giá Sao Cho Sản Phẩm";
                return Json(new { message = ErrorMsg, isSuccess = "Failed" });
            }
            //Kiểm tra xem người dùng đã bình luận ở sản phẩm này chưa, nếu rồi thì chỉ cho sửa bình luận
            if(_context.Comments.AsNoTracking().FirstOrDefault(x=>x.CustomerId==CustomerId && x.ProductId == commentVM.ProductId) != null)
            {
                var getCommentID = _context.Comments.AsNoTracking().FirstOrDefault(x => x.CustomerId == CustomerId && x.ProductId == commentVM.ProductId).CommentId;
                commentVM.CommentId=getCommentID;
                commentVM.CommentedDate = DateTime.Now.ToString();
                commentVM.CustomerId= CustomerId;
                var Updatecomment = _mapper.Map<Comment>(commentVM);
                await _comment.UpdateEntity(Updatecomment);
                _comment.SaveChanges();
                string UpdateSuccess = "Bạn Đã Thay Đổi Bình Luận";
                return Json(new { message = UpdateSuccess });
            }
            commentVM.CustomerId = CustomerId;
            commentVM.CommentedDate = DateTime.Now.ToString();
            var comment=_mapper.Map<Comment>(commentVM);
            await _comment.AddEntity(comment);
            _comment.SaveChanges();
            string Success = "Cảm Ơn Bạn Đã Đánh Giá Sản Phẩm Của Chúng Tôi";
            return Json(new { message = Success });
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlogComment(BlogCommentVM blogCommentVM)
        {
            var customerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value);
            if(!ModelState.IsValid)
            {
                string ErrorMsg=String.Join("<br>",ModelState.Values.SelectMany(x=> x.Errors).Select(x=>x.ErrorMessage));
                return Json(new { message = ErrorMsg,isSuccess="Failed" });
            }
            blogCommentVM.CommentDate= DateTime.Now.ToString();
            blogCommentVM.CustomerId = customerId;
            var blogComment=_mapper.Map<BlogComment>(blogCommentVM);
            await _blogComment.AddEntity(blogComment);
            _blogComment.SaveChanges();
            string Success = "Cảm Ơn Vì Đã Chia Sẻ Ý Kiến Của Bạn";
            return Json(new {message=Success});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBlogComment(int BlogCommentId)
        {
            var blogComment = _context.BlogComments.Find(BlogCommentId);
            _context.BlogComments.Remove(blogComment);
            _context.SaveChanges();
            string Success = "Đã Xóa Bình Luận";
            return Json(new {message=Success});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int CommentId)
        {
            var comment=_comment.GetByWhereAsync(CommentId);
            await _comment.DeleteEntity(await comment);
            _comment.SaveChanges();
            string Success = "Đã Xóa Bình Luận";
            return Json(new { message = Success });

        }
    }
}
