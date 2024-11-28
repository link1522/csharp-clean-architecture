using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaNumberController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var villaNumbers = await _context.VillaNumbers.ToListAsync();
            return View(villaNumbers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VillaNumber obj)
        {
            if (ModelState.IsValid)
            {
                await _context.VillaNumbers.AddAsync(obj);
                await _context.SaveChangesAsync();
                TempData["success"] = "Villa has been created successfully";

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
                TempData["success"] = "Villa has been updated successfully";

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
                TempData["success"] = "Villa has been deleted successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = "The Villa could not be deleted";
            return View(obj);
        }
    }
}
