using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Data.Entity;

namespace WebApp.Controllers
{
    public class LibsController : UserBaseController
    {
        public LibsController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context) : base(httpContextAccessor, context)
        {
        }

        public async Task<IActionResult> Index()
        {
            var result = await _context.Libs.Where(l => l.User.Id == UserId).ToListAsync();
            return View(result);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lib = await _context.Libs
                .Where(l => l.User.Id == UserId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lib == null)
            {
                return NotFound();
            }

            return View(lib);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Namespace,Description,Language,IsValid")] Lib lib)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(UserId);
                if (user != null)
                {
                    lib.User = user;
                    _context.Add(lib);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound("not exist user");
                }
            }
            return View(lib);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var lib = await _context.Libs.Where(l => l.User.Id == UserId).SingleOrDefaultAsync();
            if (lib == null) return NotFound();
            return View(lib);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Lib lib)
        {
            if (id != lib.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var currentLib = _context.Libs.Where(l => l.User.Id == UserId).FirstOrDefault();
                if (currentLib == null) return NotFound();
                currentLib.Namespace = lib.Namespace;
                currentLib.Description = lib.Description;
                currentLib.Language = lib.Language;
                currentLib.IsValid = lib.IsValid;

                _context.Update(currentLib);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lib);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lib = await _context.Libs
                .Where(l => l.User.Id == UserId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lib == null)
            {
                return NotFound();
            }

            return View(lib);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var lib = _context.Libs.Where(l => l.User.Id == UserId).FirstOrDefault();
            if (lib == null) return NotFound();
            _context.Libs.Remove(lib);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibExists(Guid id)
        {
            return _context.Libs.Any(e => e.Id == id);
        }
    }
}
