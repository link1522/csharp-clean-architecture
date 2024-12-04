using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Create(VillaNumberVM villaNumberVM)
        {
            bool roomNumberExists = _context.VillaNumbers.Any(v => v.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {

                await _context.VillaNumbers.AddAsync(villaNumberVM.VillaNumber);
                await _context.SaveChangesAsync();
                TempData["success"] = "Villa Number has been created successfully";

                return RedirectToAction("Index");
            }

            if (roomNumberExists)
            {
                TempData["error"] = "Villa Number already exists!";
            }

            var villas = await _context.Villas.ToListAsync();
            villaNumberVM.VillaList = villas.Select(villa => new SelectListItem
            {
                Text = villa.Name,
                Value = villa.Id.ToString()
            });

            return View(villaNumberVM);
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
        public async Task<IActionResult> Delete(int villaNumberId)
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
        public async Task<IActionResult> Delete(VillaNumberVM villaNumberVM)
        {
            var objFromDb = await _context.VillaNumbers.FindAsync(villaNumberVM.VillaNumber.Villa_Number);

            if (objFromDb is not null)
            {
                _context.VillaNumbers.Remove(objFromDb);
                await _context.SaveChangesAsync();
                TempData["success"] = "Villa number has been deleted successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = "The Villa number could not be deleted";
            return View();
        }
    }
}
