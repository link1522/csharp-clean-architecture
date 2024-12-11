using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        public IActionResult Login(string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            try
            {
                var authenticated = _userService.AuthenticateUser(loginVM.Email, loginVM.Password);

                if (!authenticated)
                {
                    TempData["error"] = "Invalid email or password";
                    return View(loginVM);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginVM.Email),
                    new Claim(ClaimTypes.Role, "User")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "cookieAuth");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync("cookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                TempData["success"] = "User has been logged in successfully";

                if (Url.IsLocalUrl(loginVM.ReturnUrl))
                {
                    return Redirect(loginVM.ReturnUrl);
                } 
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                loginVM.Password = "";
                return View(loginVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("cookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}
