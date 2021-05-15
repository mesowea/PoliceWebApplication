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
    public class InvestigatorsController : Controller
    {
        private readonly DBPoliceContext _context;

        public InvestigatorsController(DBPoliceContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin, user")]
        // GET: Investigators
        public async Task<IActionResult> Index(int? id, int? house)
        {
            if (id == null) return RedirectToAction("Index", "Departments");

            ViewBag.DeptId = id;
            ViewBag.DeptHouse = house;
            int streetId = _context.Departments.Where(d => d.Id == id).FirstOrDefault().StreetId;
            ViewBag.DeptStreet = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name;

            var investigatorsByDept = _context.Investigators.Where(i => i.DepartmentId == id).Include(i => i.Department);
            return View(await investigatorsByDept.ToListAsync());
        }
        public IActionResult Return(int deptId)
        {
            int streetId = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().StreetId;
            return RedirectToAction("Index", "Departments", new { id = streetId, name = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name });
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyDOB(DateTime DateOfBirth)
        {
            if ( DateTime.Today.AddYears(-18) <  DateOfBirth || DateTime.Today.AddYears(-100) > DateOfBirth)
            {
                
                return Json($"Введіть коректну дату народження");
            }

            return Json(true);
        }

        [Authorize(Roles = "admin, user")]
        // GET: Investigators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investigator = await _context.Investigators
                .Include(i => i.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investigator == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Cases", new { id = investigator.Id, name = investigator.Name });
            //return View(investigator);
        }

        [Authorize(Roles = "admin")]
        // GET: Investigators/Create
        public IActionResult Create(int deptId)
        {
            ViewBag.DeptId = deptId;
            ViewBag.DeptHouse = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().House;
            int streetId = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().StreetId;
            ViewBag.DeptStreet = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name;
            //ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
            return View();
        }

        // POST: Investigators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,DepartmentId,Name,DateOfBirth,Characteristic")] Investigator investigator)
        {
            int deptId = investigator.DepartmentId;
            if (ModelState.IsValid)
            {
                _context.Add(investigator);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Investigators", new { id = deptId, house = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().House });
                //return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Investigators", new { id = deptId, house = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().House });
            //ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", investigator.DepartmentId);
            //return View(investigator);
        }

        [Authorize(Roles = "admin")]
        // GET: Investigators/Edit/5
        public async Task<IActionResult> Edit(int deptId, int? investigatorId)
        {
            if (investigatorId == null)
            {
                return NotFound();
            }

            var investigator = await _context.Investigators.FindAsync(investigatorId);
            if (investigator == null)
            {
                return NotFound();
            }

            ViewBag.DeptId = deptId;
            ViewBag.DeptHouse = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().House;
            int streetId = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().StreetId;
            ViewBag.DeptStreet = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name;

            return View(investigator);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int deptId, [Bind("Id,DepartmentId,Name,DateOfBirth,Characteristic")] Investigator investigator)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(investigator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvestigatorExists(investigator.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Investigators", new { id = deptId, house = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().House });
            }
            return RedirectToAction("Index", "Investigators", new { id = deptId, house = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().House });
        }

        [Authorize(Roles = "admin")]
        // GET: Investigators/Delete/5
        public async Task<IActionResult> Delete(int deptId, int? investigatorId)
        {
            if (investigatorId == null)
            {
                return NotFound();
            }

            var investigator = await _context.Investigators
                .Include(i => i.Department)
                .FirstOrDefaultAsync(m => m.Id == investigatorId);
            if (investigator == null)
            {
                return NotFound();
            }

            ViewBag.DeptId = deptId;
            ViewBag.DeptHouse = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().House;
            int streetId = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().StreetId;
            ViewBag.DeptStreet = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name;

            return View(investigator);
        }

        // POST: Investigators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int deptId, int id)
        {
            var investigator = await _context.Investigators.FindAsync(id);
            _context.Investigators.Remove(investigator);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Investigators", new { id = deptId, house = _context.Departments.Where(d => d.Id == deptId).FirstOrDefault().House});
        }

        private bool InvestigatorExists(int id)
        {
            return _context.Investigators.Any(e => e.Id == id);
        }
    }
}
