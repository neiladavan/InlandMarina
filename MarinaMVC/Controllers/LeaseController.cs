using MarinaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MarinaMVC.Controllers
{
    public class LeaseController : Controller
    {
        private InlandMarinaContext _context;
        public LeaseController(InlandMarinaContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: LeaseController
        // Initial load of the lease page
        public IActionResult Index()
        {
            // Prepare the dock list for the select element (dropdown)
            List<Dock> docks = DockDB.GetDocks(_context);
            var list = new SelectList(docks, "ID", "Name").ToList();
            list.Insert(0, new SelectListItem("All Docks", "0"));

            ViewBag.Docks = new SelectList(list, "Value", "Text");
            ViewBag.SelectedDock = 0;

            return View();
        }

        // Action to filter slips by dock
        public IActionResult FilterSlipsByDock(int? id, int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            try
            {
                List<Slip> slips;
                if (id == 0 || id == null)
                {
                    slips = MarinaDB.GetSlips(_context);
                }
                else
                {
                    slips = MarinaDB.GetSlipsByDockId(_context, id.Value);
                }

                var paginatedSlips = slips.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var totalSlips = slips.Count;
                var totalPages = (int)Math.Ceiling((double)totalSlips / pageSize);

                ViewBag.PageNumber = pageNumber;
                ViewBag.TotalPages = totalPages;
                ViewBag.DockId = id;
            }
            catch (SqlException)
            {
                TempData["Message"] = "Database is currently not available. Try again later.";
                TempData["IsError"] = true;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"An error occurred while filtering my lease slip record: {ex.Message}";
                TempData["IsError"] = true;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // method to be call from AJAX function
        public ActionResult GetSlipsByDocks(int id, int? page)
        {
            // invoke the view component
            return ViewComponent("SlipsByDock", new { id, page });
        }

        [Authorize]
        [HttpPost]
        public IActionResult LeaseSlip(int slipId)
        {
            int? customerId = HttpContext.Session.GetInt32("CurrentLoggedInCustomer");

            try
            {
                if (customerId != null)
                {
                    Lease newLease = new Lease
                    {
                        SlipID = slipId,
                        CustomerID = customerId.Value
                    };

                    MarinaDB.AddSlipLease(_context, newLease);

                    TempData["Message"] = $"Successfully lease slip number {slipId}";
                }
            }
            catch (SqlException)
            {
                TempData["Message"] = "Database is currently not available. Try again later.";
                TempData["IsError"] = true;

                return RedirectToAction(nameof(Index));
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

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"An error occurred while leasing a slip: {ex.Message}";
                TempData["IsError"] = true;

                return RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction("MySlips", "Slip");
        }

        // GET: LeaseController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LeaseController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
