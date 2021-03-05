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
    public class DepartmentsController : Controller
    {
        private readonly DBPoliceContext _context;

        public DepartmentsController(DBPoliceContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Index", "Streets");

            ViewBag.StreetId = id;
            ViewBag.StreetName = name;
            var departmentByStreet = _context.Departments.Where(b => b.StreetId == id).Include(b => b.Street);
            return View(await departmentByStreet.ToListAsync());
        }
        //  GET: Departments/Return
        public IActionResult Return(int streetId)
        {
            int cityId = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().CityId;
            return RedirectToAction("Index", "Streets", new
            {
                id = cityId,
                name = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name
            }) ;
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Street)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Investigators", new { id = department.Id, house = department.House });
            //return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create(int streetId)
        {
            //ViewData["StreetId"] = new SelectList(_context.Streets, "Id", "Name", id);
            ViewBag.StreetId = streetId;
            ViewBag.StreetName = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name;
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StreetId,House")] Department department)
        {
            int streetId = department.StreetId;
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Departments", new { id = streetId, name = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name });
            }
            //ViewData["StreetId"] = new SelectList(_context.Streets, "Id", "Name", department.StreetId);
            //return RedirectToAction("Index", "Streets");
            return RedirectToAction("Index", "Departments", new { id = streetId, name = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name });
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int streetId, int? deptId)
        {
            if (deptId == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(deptId);
            if (department == null)
            {
                return NotFound();
            }
            ViewBag.StreetId = streetId;
            ViewBag.StreetName = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name;
            //ViewData["StreetId"] = new SelectList(_context.Streets, "Id", "Name", department.StreetId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int streetId, [Bind("Id,StreetId,House")] Department department)
        {
            //department.StreetId = streetId;
            /*if (deptId != department.Id)
            {
                return NotFound();
            }
            */
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Departments", new { id = streetId, name = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name });
                //return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Departments", new { id = streetId, name = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name });
            //ViewData["StreetId"] = new SelectList(_context.Streets, "Id", "Name", department.StreetId);
            //return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int streetId, int? deptId)
        {
            
            if (deptId == null)
            {
                return NotFound();
            }
            ViewBag.StreetId = streetId;
            ViewBag.StreetName = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name;
            ViewBag.DepartmentId = deptId;

            var department = await _context.Departments
                .Include(d => d.Street)
                .FirstOrDefaultAsync(m => m.Id == deptId);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int streetId, int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Departments", new { id = streetId, name = _context.Streets.Where(s => s.Id == streetId).FirstOrDefault().Name});
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
