using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.AspNetCore.ExistingDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EF.AspNetCore.ExistingDb.Controllers
{
    public class PostsController : Controller
    {
        public BloggingContext _context;

        public PostsController(BloggingContext context)
        {
            _context = context;
        }

        // GET: /posts/blogId
        public async Task<IActionResult> Index(int? blogId, string blogUrl)
        {

            if (blogId == null)
            {
                return NotFound();
            }

            ViewData["BlogId"] = blogId;
            ViewData["BlogUrl"] = blogUrl;

            var posts = from p in _context.Post
                        where p.BlogId == blogId
                        select p;
            
            return View(await posts.ToListAsync());
        }

        // GET: Posts/Create
        public IActionResult Create(int blogId, string blogUrl)
        {
            ViewData["BlogId"] = blogId;
            ViewData["BlogUrl"] = blogUrl;
            return View();
        }

        // POST: Post/Create/post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int blogId, string blogUrl,  [Bind("PostId,Title, Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.BlogId = blogId;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Posts", new { BlogId = blogId.ToString(), BlogUrl = blogUrl });
            }
            return View(post);
        }


        // GET: Posts/Details/3
        public async Task<IActionResult> Details(int? postId, int blogId, string blogUrl)
        {
            if (postId == null)
            {
                return NotFound();
            }

            ViewData["BlogId"] = blogId;
            ViewData["BlogUrl"] = blogUrl;

            var post = await _context.Post.SingleOrDefaultAsync(p => p.PostId == postId);
            if (post == null)
            {
                return NotFound();

            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? postId, int blogId, string blogUrl)
        {
            if (postId == null)
            {
                return NotFound();
            }

            ViewData["BlogId"] = blogId;
            ViewData["BlogUrl"] = blogUrl;

            var post = await _context.Post.SingleOrDefaultAsync(p => p.PostId == postId);
            if (post == null)
            {
                return NotFound();

            }

            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? postId, int blogId, string blogUrl, [Bind("PostId, Title, Content, BlogId")] Post post)
        {
            if (postId != post.PostId)
            {
                return NotFound();
            }

            ViewData["BlogId"] = blogId;
            ViewData["BlogUrl"] = blogUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Posts", new { blogId = blogId.ToString(), blogUrl = blogUrl });
            }

            return View(post);
        }


        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? postId, int blogId, string blogUrl)
        {
            if (postId == null)
            {
                return NotFound();
            }

            ViewData["BlogId"] = blogId;
            ViewData["BlogUrl"] = blogUrl;
            
            var post = await _context.Post.SingleOrDefaultAsync(p => p.PostId == postId);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? postId, int blogId, string blogUrl)
        {
            var post = await _context.Post.SingleOrDefaultAsync(p => p.PostId == postId);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Posts", new { blogId = blogId.ToString(), blogUrl = blogUrl });
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(p => p.PostId == id);
        }
    }
}
