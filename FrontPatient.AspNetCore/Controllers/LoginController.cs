using System.Security.Claims;
using FrontPatient.AspNetCore.Models.ViewModels;
using FrontPatient.AspNetCore.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FrontPatient.AspNetCore.Controllers
{
    public class LoginController(ILoginServices loginServices, ILogger<LoginController> logger) : Controller
    {
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await loginServices.LoginAsync(model);
            if (result == null)
            {
                ModelState.AddModelError("", "Identifiant ou mot de passe incorrect.");
                return View(model);
            }

            var token = result.Token!;

            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });

            var identity = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim("jwt", token)
                ],
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                });

            logger.LogInformation("L'utilisateur {0} s'est connecté avec succès.", model.UserName);
            return returnUrl == null ? RedirectToAction("Index", "Patient") : LocalRedirect(returnUrl);
  
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login");
        }
    }
}
