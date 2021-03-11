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
    public class CasesController : Controller
    {
        private readonly DBPoliceContext _context;

        public CasesController(DBPoliceContext context)
        {
            _context = context;
        }

        // GET: Cases
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Index", "Investigators");
            ViewBag.InvestigatorId = id;
            ViewBag.InvestigatorName = name;
            var casesByIvestigators = _context.Cases.Where(c => c.InvestigatorId == id).Include(c => c.Investigator);
            return View(await casesByIvestigators.ToListAsync());
        }
        public IActionResult Return(int investigatorId)
        {
            int deptId = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().DepartmentId;
            return RedirectToAction("Index", "Investigators", new { id = deptId, house = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().House});
        }
        // GET: Cases/Details/5
        public async Task<IActionResult> Details(int? investigatorId, int? caseId)
        {
            if (investigatorId == null || caseId == null)
            {
                return NotFound();
            }
            ViewBag.InvestigatorId = investigatorId;
            ViewBag.InvestigatorName = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().Name;
            ViewBag.CaseId = caseId;
            var @case = await _context.Cases
                .Include(c => c.Investigator)
                .FirstOrDefaultAsync(m => m.Id == caseId);
            if (@case == null)
            {
                return NotFound();
            }
            return View(@case);
        }
        public IActionResult People(int investigatorId, int caseId)
        {
            return RedirectToAction("Index", "CasePersons", new { investigatorId = investigatorId, caseId = caseId });
        }
        public IActionResult Articles(int? investigatorId, int? caseId)
        {
            if (investigatorId == null || caseId == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "CaseArticles", new { id = investigatorId, caseId = caseId });
        }

        // GET: Cases/Create
        public IActionResult Create(int investigatorId)
        {
            ViewBag.InvestigatorId = investigatorId;
            ViewBag.InvestigatorName = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().Name;
            //ViewData["InvestigatorId"] = new SelectList(_context.Investigators, "Id", "Name");
            return View();
        }

        // POST: Cases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvestigatorId,Info,DateStarted,DateFinished")] Case @case)
        {
            int investigatorId = @case.InvestigatorId;
            if (ModelState.IsValid)
            {
                _context.Add(@case);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Cases", new { id = investigatorId, name = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().Name });
                //return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Cases", new { id = investigatorId, name = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().Name });
            //ViewData["InvestigatorId"] = new SelectList(_context.Investigators, "Id", "Name", @case.InvestigatorId);
            //return View(@case);
        }

        // GET: Cases/Edit/5
        public async Task<IActionResult> Edit(int? investigatorId, int? caseId)
        {
            if (caseId == null)
            {
                return NotFound();
            }

            var @case = await _context.Cases.FindAsync(caseId);
            if (@case == null)
            {
                return NotFound();
            }
            ViewBag.CaseId = caseId;
            ViewBag.InvestigatorId = investigatorId;
            ViewBag.InvestigatorName = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().Name;

            return View(@case);
        }

        // POST: Cases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int investigatorId, [Bind("Id,InvestigatorId,Info,DateStarted,DateFinished")] Case @case)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@case);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseExists(@case.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Cases", new { id = investigatorId, name = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().Name});
            }
            return RedirectToAction("Index", "Cases", new { id = investigatorId, name = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().Name });
            //ViewData["InvestigatorId"] = new SelectList(_context.Investigators, "Id", "Name", @case.InvestigatorId);
            //return View(@case);
        }

        // GET: Cases/Delete/5
        public async Task<IActionResult> Delete(int? investigatorId, int? caseId)
        {
            if (caseId == null)
            {
                return NotFound();
            }

            var @case = await _context.Cases
                .Include(c => c.Investigator)
                .FirstOrDefaultAsync(m => m.Id == caseId);
            if (@case == null)
            {
                return NotFound();
            }
            ViewBag.CaseId = caseId;
            ViewBag.InvestigatorId = investigatorId;
            ViewBag.InvestigatorName = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().Name;

            return View(@case);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int investigatorId, int id)
        {
            var @case = await _context.Cases.FindAsync(id);
            _context.Cases.Remove(@case);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Cases", new { id = investigatorId, name = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().Name});
        }

        private bool CaseExists(int id)
        {
            return _context.Cases.Any(e => e.Id == id);
        }
    }
}
