using MarinaData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
                TempData["Message"] = "Invalid username/password.";
                TempData["IsError"] = true;
                return View(); // stay on login page
            }

            HttpContext.Session.SetInt32("CurrentLoggedInCustomer", (int)cst.ID);

            // Set user claims and sign in
            await SignInUserAsync(cst);

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

        // GET: Customer/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Customer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if the username is already taken
                    if (CustomerDB.UsernameAlreadyExists(_context, customer.Username))
                    {
                        ModelState.AddModelError("Username", "Username is already taken.");
                        return View(customer);
                    }

                    // Use the CustomerData class to register the new customer
                    CustomerDB.Register(_context, customer);

                    // Set the session for the logged-in customer
                    HttpContext.Session.SetInt32("CurrentLoggedInCustomer", customer.ID);

                    // Set user claims and sign in
                    await SignInUserAsync(customer);

                    TempData["Message"] = "Successfully created an account.";

                    return RedirectToAction("Index", "Home"); // Redirect to home or another appropriate action
                }

                return View(customer);
            }
            catch (SqlException)
            {
                TempData["Message"] = "Database is currently not available. Try again later.";
                TempData["IsError"] = true;

                return View(customer);
            }
            catch (DbUpdateException ex)
            {
                // thrown when save changes fails - can be multiple errors
                string msg = "";
                var sqlException = (SqlException)ex.InnerException!;

                foreach (SqlError error in sqlException.Errors)
                {
                    msg += $"ERROR CODE {error.Number}: {error.Message}\n";
                }

                TempData["Message"] = msg;
                TempData["IsError"] = true;

                return View(customer);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"An error occurred while creating an account: {ex.Message}";
                TempData["IsError"] = true;

                return View(customer);
            }
        }

        // Method to set user claims and sign in
        private async Task SignInUserAsync(Customer customer)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.Username),
                new Claim("FullName", $"{customer.FirstName} {customer.LastName}")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }

        public IActionResult AccessDenied() // it has to be AccessDenied to work
        {
            return View();
        }
    }
}
