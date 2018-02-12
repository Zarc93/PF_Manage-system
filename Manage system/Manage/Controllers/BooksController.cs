using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Manage.Models;

namespace Manage.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly InformationContext _context;
        
        public BooksController(InformationContext context)
        {
            _context = context;
        }

        //####搜索
        [AllowAnonymous]
        public IActionResult Search()
        {
            var history = Get_CookieInfo();
            ViewData["History"] = history;

            return View();
        }
  
        [HttpPost]
        public IActionResult Search([Bind("Name")] Book book)
        {
            if (book.Name == null)
            {
                ModelState.AddModelError(string.Empty, "您的输入信息过少!");

                Add_Cookie("History", book.Name);
                ViewData["History"] = book.Name;

                IEnumerable<Manage.Models.Book> EmptyList = null;
                return View("Result", EmptyList);
            }
            if (book.Name.Count() < 2)
            {
                ModelState.AddModelError(string.Empty, "您的输入信息过少!");

                Add_Cookie("History", book.Name);
                ViewData["History"] = book.Name;

                IEnumerable<Manage.Models.Book> EmptyList = null;
                return View("Result", EmptyList);
            }
            var searchBooks = Search_method(book);
            Add_Cookie("History", book.Name);

            //Add_SearchLog(searchBooks);

            return View("Result", searchBooks);
        }

        //####结果
        [AllowAnonymous]
        public async Task<IActionResult> Result()
        {
            var history = Get_CookieInfo();
            ViewData["History"] = history;

            return View(await _context.Books.ToListAsync());
        }
        
        [HttpPost]
        public IActionResult Result([Bind("Name")] Models.Book book)
        {
            if (book.Name == null)
            {
                ModelState.AddModelError(string.Empty, "您的输入信息过少!");

                Add_Cookie("History", book.Name);
                ViewData["History"] = book.Name;

                IEnumerable<Manage.Models.Book> EmptyList = null;
                return View("Result", EmptyList);
            }
            if (book.Name.Count() < 2)
            {
                ModelState.AddModelError(string.Empty, "您的输入信息过少!");

                Add_Cookie("History", book.Name);
                ViewData["History"] = book.Name;

                IEnumerable<Manage.Models.Book> EmptyList = null;
                return View("Result", EmptyList);
            }


            var searchBooks = Search_method(book);

            //添加Cookie
            Add_Cookie("History", book.Name);
            ViewData["History"] = book.Name;

            //Add_SearchLog(searchBooks);

            return View(searchBooks);
        }



        // GET: Books
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .SingleOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Author,Publisher,ISBN,Details,Rank,Price,ImageUrl")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.SingleOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Author,Publisher,ISBN,Details,Rank,Price,ImageUrl")] Book book)
        {
            if (id != book.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .SingleOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.SingleOrDefaultAsync(m => m.ID == id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.ID == id);
        }
        #region Cookie
        private bool Add_Cookie(string Name, string Content)
        {
            if (Name == null || Content == null) return false;
            CookieOptions cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Today.AddMonths(1)
            };
            Response.Cookies.Append(Name, Content, cookieOptions);
            return true;
        }

        private string Get_CookieInfo()
        {
            Request.Cookies.TryGetValue("History", out string history);
            return history;
        }
        #endregion

        #region 提取方法
        public IEnumerable<Manage.Models.Book> Search_method(Book book)
        {
            return (from p in _context.Books
                    where p.Name.Contains(book.Name) || p.ISBN.Contains(book.Name) || p.Publisher.Contains(book.Name)
                    select p)
                   .AsNoTracking()
                   .ToList();
        }



        //写入搜索记录
        public void Add_SearchLog(IEnumerable<Manage.Models.Book> BookList)
        {
            var firstBook = BookList.FirstOrDefault();
            if (firstBook != null)
            {
                SearchLog searchLog = new SearchLog
                {
                    Book = firstBook
                };
                _context.SearchLogs.Add(searchLog);
                _context.SaveChanges();
            }
        }
        #endregion

    }
}
