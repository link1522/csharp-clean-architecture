using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Services;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM registerVM)
        {
            try
            {
                _userService.RegisterUser(registerVM.Name, registerVM.Email, registerVM.Password);

                TempData["success"] = "User has been registered successfully";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["error"] = "User could not be registered";
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM loginVM)
        {
            try
            {
                var session = _userService.AuthenticateUser(loginVM.Email, loginVM.Password);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                loginVM.Password = "";
                return View(loginVM);
            }
        }
    }
}
