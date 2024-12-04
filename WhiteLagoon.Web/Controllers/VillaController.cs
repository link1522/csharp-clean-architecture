using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Application.Common.Interfaces;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
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
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Villa.Save();
                TempData["success"] = "Villa has been created successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Villa? obj = _unitOfWork.Villa.Get(villa => villa.Id == id);

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
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Villa.Save();

                TempData["success"] = "Villa has been updated successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Villa? obj = _unitOfWork.Villa.Get(villa => villa.Id == id);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            var objFromDb = _unitOfWork.Villa.Get(villa => villa.Id == obj.Id);

            if (objFromDb is not null)
            {
                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Villa.Save();
                TempData["success"] = "Villa has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The Villa could not be deleted";
            return View(obj);
        }
    }
}
