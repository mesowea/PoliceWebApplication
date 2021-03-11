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
    public class CaseArticlesController : Controller
    {
        private readonly DBPoliceContext _context;

        public CaseArticlesController(DBPoliceContext context)
        {
            _context = context;
        }

        // GET: CaseArticles
        public async Task<IActionResult> Index(int? id, int? caseId)
        {
            if (id == null || caseId == null) return RedirectToAction("Index", "Cases");
            ViewBag.InvestigatorId = id;
            ViewBag.InvestigatorName = _context.Investigators.Where(i => i.Id == id).FirstOrDefault().Name;
            ViewBag.CaseId = caseId;
            var articleByCase = _context.CasesArticles.Where(c => c.CaseId == caseId).Include(c => c.Article).Include(c => c.Case);
            return View(await articleByCase.ToListAsync());
        }
        public IActionResult Return(int investigatorId)
        {
            return RedirectToAction("Index", "Cases", new { id = investigatorId, name = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().Name });
        }
        // GET: CaseArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return NotFound();
        }

        // GET: CaseArticles/Create
        public IActionResult Create(int caseId)
        {
            ViewData["ArticleId"] = new SelectList(_context.CriminalArticles, "Id","Name", "Info");
            ViewBag.CaseId = caseId;
            ViewBag.InvestigatorId = _context.Cases.Where(c => c.Id == caseId).FirstOrDefault().InvestigatorId;
            return View();
        }

        // POST: CaseArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CaseId,ArticleId")] CaseArticle caseArticle)
        {
            int caseId = caseArticle.CaseId;
            if (ModelState.IsValid)
            {
                _context.Add(caseArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "CaseArticles", new { id = _context.Cases.Where(c => c.Id == caseId).FirstOrDefault().InvestigatorId, caseId = caseId });
            }
            return RedirectToAction("Index", "CaseArticles", new { id = _context.Cases.Where(c => c.Id == caseId).FirstOrDefault().InvestigatorId, caseId = caseId });
        }

        // GET: CaseArticles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return NotFound();
        }

        // POST: CaseArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CaseId,ArticleId")] CaseArticle caseArticle)
        {
            if (id != caseArticle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseArticleExists(caseArticle.Id))
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
            ViewData["ArticleId"] = new SelectList(_context.CriminalArticles, "Id", "Info", caseArticle.ArticleId);
            ViewData["CaseId"] = new SelectList(_context.Cases, "Id", "Info", caseArticle.CaseId);
            return View(caseArticle);
        }

        // GET: CaseArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseArticle = await _context.CasesArticles
                .Include(c => c.Article)
                .Include(c => c.Case)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caseArticle == null)
            {
                return NotFound();
            }
            int caseId = _context.CasesArticles.Where(c => c.Id == id).FirstOrDefault().CaseId;
            ViewBag.CaseId = caseId;
            ViewBag.InvestigatorId = _context.Cases.Where(c => c.Id == caseId).FirstOrDefault().InvestigatorId;
            return View(caseArticle);
        }

        // POST: CaseArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseArticle = await _context.CasesArticles.FindAsync(id);
            _context.CasesArticles.Remove(caseArticle);
            await _context.SaveChangesAsync();
            int caseId = caseArticle.CaseId;
            int investigatorId = _context.Cases.Where(c => c.Id == caseId).FirstOrDefault().InvestigatorId;
            return RedirectToAction("Index", "CaseArticles", new { id = investigatorId, caseId = caseId });
        }

        private bool CaseArticleExists(int id)
        {
            return _context.CasesArticles.Any(e => e.Id == id);
        }
    }
}
