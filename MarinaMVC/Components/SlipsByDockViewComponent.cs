using MarinaData;
using Microsoft.AspNetCore.Mvc;

namespace MarinaMVC.Components
{
    public class SlipsByDockViewComponent : ViewComponent
    {
        private InlandMarinaContext _context { get; set; }

        // context injected to the constructor
        public SlipsByDockViewComponent(InlandMarinaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id, int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

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

            return View(paginatedSlips); // The view for the view component should be named Default
        }
    }
}
