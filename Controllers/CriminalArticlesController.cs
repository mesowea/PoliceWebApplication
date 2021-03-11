using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PoliceWebApplication;

namespace PoliceWebApplication.Controllers
{
    public class CriminalArticlesController : Controller
    {
        private readonly DBPoliceContext _context;

        public CriminalArticlesController(DBPoliceContext context)
        {
            _context = context;
        }

        // GET: CriminalArticles
        public async Task<IActionResult> Index()
        {
            return View(await _context.CriminalArticles.ToListAsync());
        }

        // GET: CriminalArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criminalArticle = await _context.CriminalArticles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (criminalArticle == null)
            {
                return NotFound();
            }

            return View(criminalArticle);
        }

        // GET: CriminalArticles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CriminalArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Info")] CriminalArticle criminalArticle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(criminalArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(criminalArticle);
        }

        // GET: CriminalArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criminalArticle = await _context.CriminalArticles.FindAsync(id);
            if (criminalArticle == null)
            {
                return NotFound();
            }
            return View(criminalArticle);
        }

        // POST: CriminalArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Info")] CriminalArticle criminalArticle)
        {
            if (id != criminalArticle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(criminalArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CriminalArticleExists(criminalArticle.Id))
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
            return View(criminalArticle);
        }

        // GET: CriminalArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criminalArticle = await _context.CriminalArticles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (criminalArticle == null)
            {
                return NotFound();
            }

            return View(criminalArticle);
        }

        // POST: CriminalArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var criminalArticle = await _context.CriminalArticles.FindAsync(id);
            _context.CriminalArticles.Remove(criminalArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CriminalArticleExists(int id)
        {
            return _context.CriminalArticles.Any(e => e.Id == id);
        }
    }
}
