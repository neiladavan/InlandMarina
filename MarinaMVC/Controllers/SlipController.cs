using Humanizer.Localisation;
using MarinaData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarinaMVC.Controllers
{
    public class SlipController : Controller
    {
        private InlandMarinaContext _context { get; set; }

        public SlipController(InlandMarinaContext context)
        {
            _context = context;
        }

        public ActionResult Index(int? id, int? page)
        {
            // Prepare the dock list for the select element (dropdown)
            List<Dock> docks = DockDB.GetDocks(_context);

            var list = new SelectList(docks, "ID", "Name").ToList(); // to add "All" option
            list.Insert(0, new SelectListItem("All Docks", "0")); // 0 is the value for all

            ViewBag.Docks = new SelectList(list, "Value", "Text", id); // Set selected item
            ViewBag.SelectedDock = id ?? 0;

            // Pagination logic
            int pageSize = 10;
            int pageNumber = (page ?? 1);

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

            return View(paginatedSlips);
        }

        [HttpPost]
        public ActionResult Index(int id = 0, int? page = 1) // 0 means no filtering
        {
            return RedirectToAction("Index", new { id, page });
        }

        // GET: SlipController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SlipController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SlipController/Create
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

        // GET: SlipController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SlipController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: SlipController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SlipController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
