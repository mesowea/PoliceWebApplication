using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace PoliceWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ChartsController : ControllerBase
    {
        private readonly DBPoliceContext _context;
        public ChartsController(DBPoliceContext context)
        {
            _context = context;
        }
        [HttpGet("InvestigatorCasesJsonData")]
        public JsonResult Get(int deptid)
        {
            var investigators = _context.Investigators.Where(c => c.DepartmentId == deptid).Include(c => c.Cases).ToList();
            List<object> investigatorCases = new List<object>();
            investigatorCases.Add(new[] { "Слідчий", "Кількість справ" });

            foreach (var i in investigators)
            {
                investigatorCases.Add(new object[] { i.Name, i.Cases.Count() });
            }
            return new JsonResult(investigatorCases);
        }
        [HttpGet("CityCasesJsonData")]
        public JsonResult Get()
        {
            var caseCity = _context.Cases.Include(c => c.Investigator.Department.Street.City).ToArray();
            var cityCases = caseCity.GroupBy(c => c.Investigator.Department.Street.City.Name, c => c.Id, (cityName, count) => new { Key = cityName, Count = count.Count()});
            List<object> citiesAndCases = new List<object>();
            citiesAndCases.Add(new[] { "Місто", "Кількість справ" });

            foreach (var groupbyCity in cityCases)
            {
                citiesAndCases.Add(new object[] { groupbyCity.Key, groupbyCity.Count});
            }
            return new JsonResult(citiesAndCases);
        }
    }
}
