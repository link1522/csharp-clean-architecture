using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var villas = _unitOfWork.Villa.GetAll();

            var ViewModel = new AmenityVM
            {
                VillaList = villas.Select(villa => new SelectListItem
                {
                    Text = villa.Name,
                    Value = villa.Id.ToString()
                })
            };

            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(amenityVM.Amenity);
                _unitOfWork.Save();

                TempData["success"] = "Amenity has been created successfully";

                return RedirectToAction(nameof(Index));
            }

            var villas = _unitOfWork.Villa.GetAll();
            amenityVM.VillaList = villas.Select(villa => new SelectListItem
            {
                Text = villa.Name,
                Value = villa.Id.ToString()
            });

            return View(amenityVM);
        }

        [HttpGet]
        public IActionResult Update(int amenityId)
        {
            var villas = _unitOfWork.Villa.GetAll();
            var model = new AmenityVM
            {
                VillaList = villas.Select(villa => new SelectListItem
                {
                    Text = villa.Name,
                    Value = villa.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };

            if (model.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "Amenity has been updated successfully";

                return RedirectToAction(nameof(Index));
            }

            var villas = _unitOfWork.Villa.GetAll();
            amenityVM.VillaList = villas.Select(villa => new SelectListItem
            {
                Text = villa.Name,
                Value = villa.Id.ToString()
            });

            return View(amenityVM);
        }

        [HttpGet]
        public IActionResult Delete(int amenityId)
        {
            var villas = _unitOfWork.Villa.GetAll();
            var viewModel = new AmenityVM
            {
                VillaList = villas.Select(villa => new SelectListItem
                {
                    Text = villa.Name,
                    Value = villa.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };

            if (viewModel.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            var objFromDb = _unitOfWork.Amenity.Get(u => u.Id == amenityVM.Amenity.Id);

            if (objFromDb is not null)
            {
                _unitOfWork.Amenity.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "Amenity has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The Amenity could not be deleted";
            return View();
        }
    }
}
