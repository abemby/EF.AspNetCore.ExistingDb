using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.AspNetCore.ExistingDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EF.AspNetCore.ExistingDb.Controllers
{
    public class EmployeesController : Controller
    {
        public BloggingContext _context;

        public EmployeesController(BloggingContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Produces("application/json")]
        public IActionResult Index()
        {
            var emps = _context.Employees.Select(e => new {
                Id = e.EmployeeId,
                Name = string.Format("{0} {1}", e.FirstName, e.LastName)
            });

            return this.Json(emps.ToList());
        }



        // GET: /<controller>/
        public async Task<IActionResult> Details()
        {
            return View(await _context.Employees.ToListAsync());
        }
    }
}