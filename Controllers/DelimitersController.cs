using LexicoAnalyzer.Web.Data;
using LexicoAnalyzer.Web.Models;
using LexicoAnalyzer.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LexicoAnalyzer.Web.Controllers
{
    public class DelimitersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DelimitersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await _context.Delimiters
                .OrderBy(x => x.Symbol)
                .ToListAsync();

            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DelimiterFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DelimiterFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string normalizedSymbol = model.Symbol.Trim();

            bool exists = await _context.Delimiters
                .AnyAsync(x => x.Symbol == normalizedSymbol);

            if (exists)
            {
                ModelState.AddModelError(nameof(model.Symbol), "Ese delimitador ya existe.");
                return View(model);
            }

            var entity = new Delimiter
            {
                Symbol = normalizedSymbol,
                IsActive = model.IsActive
            };

            _context.Delimiters.Add(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Delimitador creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.Delimiters.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new DelimiterFormViewModel
            {
                Id = entity.Id,
                Symbol = entity.Symbol,
                IsActive = entity.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DelimiterFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var entity = await _context.Delimiters.FindAsync(model.Id);

            if (entity == null)
            {
                return NotFound();
            }

            string normalizedSymbol = model.Symbol.Trim();

            bool exists = await _context.Delimiters
                .AnyAsync(x => x.Symbol == normalizedSymbol && x.Id != model.Id);

            if (exists)
            {
                ModelState.AddModelError(nameof(model.Symbol), "Ya existe otro delimitador con ese valor.");
                return View(model);
            }

            entity.Symbol = normalizedSymbol;
            entity.IsActive = model.IsActive;

            _context.Delimiters.Update(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Delimitador actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}