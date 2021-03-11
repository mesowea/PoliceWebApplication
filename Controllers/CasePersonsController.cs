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
    public class CasePersonsController : Controller
    {
        private readonly DBPoliceContext _context;

        public CasePersonsController(DBPoliceContext context)
        {
            _context = context;
        }

        // GET: CasePersons
        public async Task<IActionResult> Index()
        {
            var dBPoliceContext = _context.CasesPeople.Include(c => c.Case).Include(c => c.Person);
            return View(await dBPoliceContext.ToListAsync());
        }

        // GET: CasePersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePerson = await _context.CasesPeople
                .Include(c => c.Case)
                .Include(c => c.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (casePerson == null)
            {
                return NotFound();
            }

            return View(casePerson);
        }

        // GET: CasePersons/Create
        public IActionResult Create()
        {
            ViewData["CaseId"] = new SelectList(_context.Cases, "Id", "Info");
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Name");
            return View();
        }

        // POST: CasePersons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CaseId,PersonId")] CasePerson casePerson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(casePerson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseId"] = new SelectList(_context.Cases, "Id", "Info", casePerson.CaseId);
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Name", casePerson.PersonId);
            return View(casePerson);
        }

        // GET: CasePersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePerson = await _context.CasesPeople.FindAsync(id);
            if (casePerson == null)
            {
                return NotFound();
            }
            ViewData["CaseId"] = new SelectList(_context.Cases, "Id", "Info", casePerson.CaseId);
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Name", casePerson.PersonId);
            return View(casePerson);
        }

        // POST: CasePersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (casePerson == null)
            {
                return NotFound();
            }

            return View(casePerson);
        }

        // POST: CasePersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var casePerson = await _context.CasesPeople.FindAsync(id);
            _context.CasesPeople.Remove(casePerson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasePersonExists(int id)
        {
            return _context.CasesPeople.Any(e => e.Id == id);
        }
    }
}
