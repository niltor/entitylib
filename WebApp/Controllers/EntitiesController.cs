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
    public class EntitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        [TempData]
        public Guid? LibId { get; set; }

        public EntitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Guid? libId = null)
        {
            if (libId == null)
            {

                return NotFound();
            }
            LibId = LibId;
            var entities = await _context.Entities
                .Where(e => e.Lib.Id == libId)
                .ToListAsync();
            return View(entities);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Entities
                .Include(e => e.Lib)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        public IActionResult Create(Guid? libId)
        {
            if (libId == null)
            {
                return NotFound();
            }
            var lib = _context.Libs.Find(libId);
            ViewBag.lib = lib;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? libId, [Bind("Name,Description,Content,Id")] Entity entity)
        {
            if (ModelState.IsValid)
            {
                entity.Id = Guid.NewGuid();
                var lib = _context.Libs.Find(libId);
                entity.Lib = lib;
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { libId });
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Entities
                .Include(e => e.Lib)
                .SingleOrDefaultAsync(e => e.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            LibId = entity.Lib.Id;
            TempData.Keep();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Description,Content,Id,CreatedTime,UpdatedTime")] Entity entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntityExists(entity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { libId = LibId });
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Entities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entity = await _context.Entities.FindAsync(id);
            _context.Entities.Remove(entity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntityExists(Guid id)
        {
            return _context.Entities.Any(e => e.Id == id);
        }
    }
}
