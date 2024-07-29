using MarinaData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarinaMVC.Controllers
{
    public class CustomerController : Controller
    {
        private InlandMarinaContext _context { get; set; }

        // context injected to the constructor
        public CustomerController(InlandMarinaContext context)
        {
            _context = context;
        }
        public IActionResult Login(string returnUrl = "")
        {
            if (returnUrl != "")
            {
                TempData["ReturnUrl"] = returnUrl;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(Customer cust)
        {
            Customer cst = CustomerDB.Authenticate(_context, cust.Username, cust.Password);
            if (cst == null) // auth failed
            {
                return View(); // stay on login page
            }

            HttpContext.Session.SetInt32("CurrentLoggedInCustomer", (int)cst.ID);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, cst.Username),
                new Claim("FullName", $"{cst.FirstName} {cst.LastName}")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            if (String.IsNullOrEmpty(TempData["ReturnUrl"]?.ToString())) // no return URL
                return RedirectToAction("Index", "Home"); // home page
            else
                return Redirect(TempData["ReturnUrl"]!.ToString()!);
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // release cookies

            return RedirectToAction("Index", "Home"); // home page
        }

        public IActionResult AccessDenied() // it has to be AccessDenied to work
        {
            return View();
        }
    }
}
