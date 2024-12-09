﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public  IActionResult Index()
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
        public IActionResult Update(int villaNumberId)
        {
            var villas = _unitOfWork.Villa.GetAll();
            var model = new VillaNumberVM
            {
                VillaList = villas.Select(villa => new SelectListItem
                {
                    Text = villa.Name,
                    Value = villa.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };

            if (model.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVm)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumberVm.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "Villa Number has been updated successfully";

                return RedirectToAction(nameof(Index));
            }

            var villas = _unitOfWork.Villa.GetAll();
            villaNumberVm.VillaList = villas.Select(villa => new SelectListItem
            {
                Text = villa.Name,
                Value = villa.Id.ToString()
            });

            return View(villaNumberVm);
        }

        [HttpGet]
        public IActionResult Delete(int villaNumberId)
        {
            var villas = _unitOfWork.Villa.GetAll();
            var model = new VillaNumberVM
            {
                VillaList = villas.Select(villa => new SelectListItem
                {
                    Text = villa.Name,
                    Value = villa.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };

            if (model.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            var objFromDb = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (objFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "Villa number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The Villa number could not be deleted";
            return View();
        }
    }
}