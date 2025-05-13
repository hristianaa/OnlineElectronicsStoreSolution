using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models.ViewModels;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _auth;
        private readonly IUserService _users;

        public AccountController(IAuthService authService, IUserService userService)
        {
            _auth = authService;
            _users = userService;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
            => View(new LoginViewModel { ReturnUrl = returnUrl });

        // POST: /Account/Login
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _auth.ValidateCredentialsAsync(vm.Email, vm.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(vm);
            }

            // Build claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,           user.FullName),
                new Claim(ClaimTypes.Email,          user.Email),
                new Claim(ClaimTypes.Role,           user.Role)
            };
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in with cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            // Redirect to returnUrl or home
            if (!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                return Redirect(vm.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
            => View(new RegisterViewModel());

        // POST: /Account/Register
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (await _users.GetByEmailAsync(vm.Email) != null)
            {
                ModelState.AddModelError(nameof(vm.Email), "Email already in use.");
                return View(vm);
            }

            var dto = new RegisterDto
            {
                FullName = vm.FullName,
                Email = vm.Email,
                Password = vm.Password,
                Role = "User"
            };
            var newUser = await _auth.RegisterUserAsync(dto);

            // Auto sign-in after register
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()),
                new Claim(ClaimTypes.Name,           newUser.FullName),
                new Claim(ClaimTypes.Email,          newUser.Email),
                new Claim(ClaimTypes.Role,           newUser.Role)
            };
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
            => View();

        // GET: /Account/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var user = await _users.GetByIdAsync(int.Parse(userId));
            if (user == null) return NotFound();

            var vm = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email
            };
            return View(vm);
        }

        // POST: /Account/Profile
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var updateDto = new UpdateUserDto
            {
                Id = int.Parse(userId),
                FullName = vm.FullName,
                Email = vm.Email,
                NewPassword = vm.NewPassword
            };
            var result = await _users.UpdateProfileAsync(updateDto);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(vm);
            }

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Profile));
        }
    }
}