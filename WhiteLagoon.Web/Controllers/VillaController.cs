using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Application.Common.Interfaces;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository _villaRepository;

        public VillaController(IVillaRepository villaRepository)
        {
            _villaRepository = villaRepository;
        }

        public IActionResult Index()
        {
            var villas = _villaRepository.GetAll();
            return View(villas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Description", "The description cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _villaRepository.Add(obj);
                _villaRepository.Save();
                TempData["success"] = "Villa has been created successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Villa? obj = _villaRepository.Get(villa => villa.Id == id);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid)
            {
                _villaRepository.Update(obj);
                _villaRepository.Save();

                TempData["success"] = "Villa has been updated successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Villa? obj = _villaRepository.Get(villa => villa.Id == id);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            var objFromDb = _villaRepository.Get(villa => villa.Id == obj.Id);

            if (objFromDb is not null)
            {
                _villaRepository.Remove(objFromDb);
                _villaRepository.Save();
                TempData["success"] = "Villa has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The Villa could not be deleted";
            return View(obj);
        }
    }
}
