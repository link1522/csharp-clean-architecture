using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var villas = await _context.Villas.ToListAsync();
            return View(villas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Description", "The description cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                await _context.Villas.AddAsync(obj);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Villa? obj = await _context.Villas.FindAsync(id);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Villa obj)
        {
            if (ModelState.IsValid)
            {
                _context.Villas.Update(obj);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Villa? obj = await _context.Villas.FindAsync(id);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Villa obj)
        {
            var objFromDb = await _context.Villas.FindAsync(obj.Id);

            if (objFromDb is not null)
            {
                _context.Villas.Remove(objFromDb);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
