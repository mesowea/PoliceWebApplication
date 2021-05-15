using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PoliceWebApplication;
using Microsoft.AspNetCore.Authorization;

namespace PoliceWebApplication.Controllers
{
    public class CasePersonsController : Controller
    {
        private readonly DBPoliceContext _context;

        public CasePersonsController(DBPoliceContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin, user")]
        // GET: CasePersons
        public async Task<IActionResult> Index(int? investigatorId, int? caseId)
        {
            if (investigatorId == null || caseId == null) return RedirectToAction("Index", "Cases");
            ViewBag.CaseId = caseId;
            //for return
            ViewBag.InvestigatorId = investigatorId;
            ViewBag.InvestigatorName = _context.Investigators.Where(i => i.Id == investigatorId).FirstOrDefault().Name;
            var casesToPeople = _context.CasesPeople.Where(c => c.CaseId == caseId).Include(c => c.Case).Include(c => c.Person).Include(c => c.Person.Type);
            return View(await casesToPeople.ToListAsync());
        }
        public IActionResult Return(int investigatorId, string investigatorName)
        {
            return RedirectToAction("Index", "Cases", new { id = investigatorId, name = investigatorName });
        }

        [Authorize(Roles = "admin")]
        public IActionResult NewPerson(int caseId)
        {
            return RedirectToAction("Create", "People", new { caseId = caseId});
        }
        // GET: CasePersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        // GET: CasePersons/Create
        public IActionResult Create(int caseId)
        {
            ViewBag.CaseId = caseId;
            ViewBag.InvestigatorId = _context.Cases.Where(c => c.Id == caseId).FirstOrDefault().InvestigatorId;
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Id");
            return View();
        }

        // POST: CasePersons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,CaseId,PersonId")] CasePerson casePerson)
        {
            int caseId = casePerson.CaseId;
            int investigatorId = _context.Cases.Where(c => c.Id == caseId).FirstOrDefault().InvestigatorId;
            if (ModelState.IsValid)
            {
                _context.Add(casePerson);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "CasePersons", new { investigatorId = investigatorId, caseId = caseId});
            }
            return RedirectToAction("Index", "CasePersons", new { investigatorId = investigatorId, caseId = caseId });
        }

        [Authorize(Roles = "admin")]
        // GET: CasePersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return NotFound();
        }

        // POST: CasePersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CaseId,PersonId")] CasePerson casePerson)
        {
            if (id != casePerson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(casePerson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CasePersonExists(casePerson.Id))
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
            ViewData["CaseId"] = new SelectList(_context.Cases, "Id", "Info", casePerson.CaseId);
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Name", casePerson.PersonId);
            return View(casePerson);
        }

        [Authorize(Roles = "admin")]
        // GET: CasePersons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePerson = await _context.CasesPeople
                .Include(c => c.Case)
                .Include(c => c.Person)
                .Include(c => c.Person.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (casePerson == null)
            {
                return NotFound();
            }
            int caseId = _context.CasesPeople.Where(c => c.Id == id).FirstOrDefault().CaseId;
            ViewBag.CaseId = caseId;
            ViewBag.InvestigatorId = _context.Cases.Where(c => c.Id == caseId).FirstOrDefault().InvestigatorId;
            return View(casePerson);
        }

        // POST: CasePersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var casePerson = await _context.CasesPeople.FindAsync(id);
            _context.CasesPeople.Remove(casePerson);
            await _context.SaveChangesAsync();
            int caseId = casePerson.CaseId;
            int investigatorId = _context.Cases.Where(c => c.Id == caseId).FirstOrDefault().InvestigatorId;
            return RedirectToAction("Index", "CasePersons", new { investigatorId = investigatorId, caseId = caseId });
        }

        private bool CasePersonExists(int id)
        {
            return _context.CasesPeople.Any(e => e.Id == id);
        }
    }
}
