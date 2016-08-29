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
    public class BlogsController : Controller
    {
        public BloggingContext _context;

        public BlogsController(BloggingContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blog.ToListAsync());
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? Id)
        {
            {
            if (Id == null)
                return NotFound();
            }

            //Eager loading...

            var blog = await _context.Blog.Include(b=>b.Post).SingleOrDefaultAsync(b => b.BlogId.Equals(Id));
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Url")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? blogId)
        {
            if (blogId == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.SingleOrDefaultAsync(b => b.BlogId == blogId);
            if (blog == null)
            {
                return NotFound();
            }

            return  View(blog);
        }

        // POST: Blogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int blogId, [Bind("BlogId, Url")] Blog blog)
        {
            if (blogId != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!BlogExists(blogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? blogId)
        {
            if (blogId == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.SingleOrDefaultAsync(b => b.BlogId == blogId);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }


        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int blogId)
        {
            var blog = await _context.Blog.SingleOrDefaultAsync(b => b.BlogId == blogId);
            if (blog == null)
            {
                return NotFound();
            }
            _context.Blog.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BlogExists(int blogId)
        {
            return _context.Blog.Any(b => b.BlogId == blogId);
        }
    }
}
