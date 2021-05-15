using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PoliceWebApplication;

using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace PoliceWebApplication.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly DBPoliceContext _context;

        public DepartmentsController(DBPoliceContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin, user")]
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

        [Authorize(Roles = "admin, user")]
        public ActionResult Export(int streetId)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var departments = _context.Departments.Include("Investigators").Where(c => c.StreetId == streetId).ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані) DONE
                foreach (var c in departments)
                {
                    var worksheet = workbook.Worksheets.Add(c.Id.ToString());

                    worksheet.Cell("A1").Value = "Ім'я";
                    worksheet.Cell("B1").Value = "Дата народження";
                    worksheet.Cell("C1").Value = "Характеристика";
                    //worksheet.Cell("D1").Value = "Справи:";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var investigators = c.Investigators.ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < investigators.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = investigators[i].Name;
                        worksheet.Cell(i + 2, 2).Value = investigators[i].DateOfBirth;
                        worksheet.Cell(i + 2, 3).Value = investigators[i].Characteristic;
                        /*
                        var invcases = _context.Cases.Where(a => a.InvestigatorId == investigators[i].Id).ToList();
                        //більше 4-ох нікуди писати NO
                        int j = 0;
                        foreach (var cas in invcases)
                        {
                            {
                                worksheet.Cell(i + 2, j + 4).Value = cas.Id;
                                j++;
                            }
                        }
                        */
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"PoliceWebApp_Street{streetId}.xlsx"
                    };
                }
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Import(IFormFile fileExcel, int streetId)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Department newdept;
                                var d = (from dept in _context.Departments
                                         where dept.Id == Int32.Parse(worksheet.Name)
                                         select dept).ToList();
                                if (d.Count > 0)
                                {
                                    newdept = d[0];
                                }
                                else
                                {
                                    newdept = new Department();
                                    newdept.Id = Int32.Parse(worksheet.Name);
                                    newdept.StreetId = streetId;
                                    newdept.House = 9999999;
                                    //додати в контекст
                                    _context.Departments.Add(newdept);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Investigator investigator = new Investigator();
                                        investigator.Name = row.Cell(1).Value.ToString();
                                        investigator.DateOfBirth = DateTime.Parse(row.Cell(2).Value.ToString());
                                        investigator.Characteristic = row.Cell(3).Value.ToString();
                                        investigator.Department = newdept;


                                        var cloneInv = (from inv in _context.Investigators
                                                        where inv.Name == investigator.Name && inv.DateOfBirth == investigator.DateOfBirth && inv.Characteristic == investigator.Characteristic
                                                        select inv).ToList();
                                        if(cloneInv.Count == 0)
                                        _context.Investigators.Add(investigator);

                                        /*
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                        for (int i = 4; i <= 999999; i++)
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Case cas;

                                                var casss = (from cass in _context.Cases
                                                         where cass.Id == Int32.Parse(row.Cell(i).Value.ToString())
                                                         select cass).ToList();
                                                if (casss.Count > 0)
                                                {
                                                    cas = casss[0];
                                                }
                                                else
                                                {
                                                    cas = new Case();
                                                    cas.Id = Int32.Parse(row.Cell(i).Value.ToString());
                                                    cas.Info = "from EXCEL";
                                                    //додати в контекст
                                                    _context.Cases.Add(cas);
                                                }
                                            }
                                        }
                                        */
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)

                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin, user")]
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


        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
