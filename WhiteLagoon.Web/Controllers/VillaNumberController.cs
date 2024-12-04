using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

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
            var villaNumbers = await _context.VillaNumbers.Include(u => u.Villa).ToListAsync();
            return View(villaNumbers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var villas = await _context.Villas.ToListAsync();

            var model = new VillaNumberVM
            {
                VillaList = villas.Select(villa => new SelectListItem
                {
                    Text = villa.Name,
                    Value = villa.Id.ToString()
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _context.VillaNumbers.Any(v => v.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {

                await _context.VillaNumbers.AddAsync(obj.VillaNumber);
                await _context.SaveChangesAsync();
                TempData["success"] = "Villa Number has been created successfully";

                return RedirectToAction("Index");
            }

            if (roomNumberExists)
            {
                TempData["error"] = "Villa Number already exists!";
            }

            var villas = await _context.Villas.ToListAsync();
            obj.VillaList = villas.Select(villa => new SelectListItem
            {
                Text = villa.Name,
                Value = villa.Id.ToString()
            });

            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int villaNumberId)
        {
            var villas = await _context.Villas.ToListAsync();
            var model = new VillaNumberVM
            {
                VillaList = villas.Select(villa => new SelectListItem
                {
                    Text = villa.Name,
                    Value = villa.Id.ToString()
                }),
                VillaNumber = await _context.VillaNumbers.FirstOrDefaultAsync(u => u.Villa_Number == villaNumberId)
            };

            if (model.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(VillaNumberVM villaNumberVm)
        {
            if (ModelState.IsValid)
            {
                _context.VillaNumbers.Update(villaNumberVm.VillaNumber);
                await _context.SaveChangesAsync();
                TempData["success"] = "Villa Number has been updated successfully";

                return RedirectToAction("Index");
            }

            var villas = await _context.Villas.ToListAsync();
            villaNumberVm.VillaList = villas.Select(villa => new SelectListItem
            {
                Text = villa.Name,
                Value = villa.Id.ToString()
            });

            return View(villaNumberVm);
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
