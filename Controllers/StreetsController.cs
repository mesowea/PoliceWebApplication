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
    public class StreetsController : Controller
    {
        private readonly DBPoliceContext _context;

        public StreetsController(DBPoliceContext context)
        {
            _context = context;
        }

        // GET: Streets
        public async Task<IActionResult> Index(int? id, string? name) 
        {
            if (id == null) return RedirectToAction("Index", "Cities");

            ViewBag.CityId = id;
            ViewBag.CityName = name;

            var streetsOfTheCity = _context.Streets.Where(s => s.CityId == id).Include(s => s.City);
            return View(await streetsOfTheCity.ToListAsync());
        }
        // GET: Streets/Return
        public IActionResult Return()
        {
            return RedirectToAction("Index", "Cities");
        }
        // GET: Streets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var street = await _context.Streets
                .Include(s => s.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (street == null)
            {
                return NotFound();
            }

            //return View(street);
            return RedirectToAction("Index", "Departments", new { id = street.Id, name = street.Name });
        }
       
        // GET: Streets/Create
        public IActionResult Create(int cityId)
        {
            //ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", id);
            ViewBag.CityId = cityId;
            ViewBag.CityName = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name;
            return View();
        }

        // POST: Streets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CityId")] Street street)
        {
            int cityId = street.CityId;
            if (ModelState.IsValid)
            {
                _context.Add(street);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Streets", new { id = cityId, name = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name });
            }
            //ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            //return View(street);
            return RedirectToAction("Index", "Streets", new { id = cityId, name = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name });
        }

        // GET: Streets/Edit/5
        public async Task<IActionResult> Edit(int cityId, int? streetId)
        {
            ViewBag.CityId = cityId;
            ViewBag.CityName = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name;

            if (streetId == null)
            {
                return NotFound();
            }

            var street = await _context.Streets.FindAsync(streetId);
            if (street == null)
            {
                return NotFound();
            }
           
            //ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", street.CityId);
            return View(street);
        }

        // POST: Streets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int cityId, [Bind("Id,Name,CityId")] Street street)
        {
            //street.CityId = cityId;
            /*
            if (streetId == street.Id)
            {
                return NotFound();
            }
            */
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(street);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StreetExists(street.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Streets", new { id = cityId, name = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name });
                //return RedirectToAction(nameof(Index));
            }
            //ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", street.CityId);
            return RedirectToAction("Index", "Streets", new { id = cityId, name = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name });
            //return View(street);
        }

        // GET: Streets/Delete/5
        public async Task<IActionResult> Delete(int cityId, int? streetId)
        { 
            if (streetId == null)
            {
                return NotFound();
            }

            ViewBag.CityId = cityId;
            ViewBag.CityName = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name;
            ViewBag.StreetId = streetId;

            var street = await _context.Streets
                .Include(s => s.City)
                .FirstOrDefaultAsync(m => m.Id == streetId);
            if (street == null)
            {
                return NotFound();
            }

            return View(street);
        }

        // POST: Streets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int cityId, int id)
        {
            var street = await _context.Streets.FindAsync(id);
            _context.Streets.Remove(street);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Streets", new { id = cityId, name = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name });
        }

        private bool StreetExists(int id)
        {
            return _context.Streets.Any(e => e.Id == id);
        }
    }
}
