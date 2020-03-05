using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Data.Entity;

namespace WebApp.Controllers
{
    public class LibsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Libs.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lib = await _context.Libs
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
                _context.Add(lib);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lib);
        }

        // GET: Libs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lib = await _context.Libs.FindAsync(id);
            if (lib == null)
            {
                return NotFound();
            }
            return View(lib);
        }

        // POST: Libs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Namespace,Description,Language,IsValid,Id,CreatedTime,UpdatedTime")] Lib lib)
        {
            if (id != lib.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lib);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibExists(lib.Id))
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
            return View(lib);
        }

        // GET: Libs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lib = await _context.Libs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lib == null)
            {
                return NotFound();
            }

            return View(lib);
        }

        // POST: Libs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var lib = await _context.Libs.FindAsync(id);
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
