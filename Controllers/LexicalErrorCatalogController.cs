using LexicoAnalyzer.Web.Data;
using LexicoAnalyzer.Web.Models;
using LexicoAnalyzer.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LexicoAnalyzer.Web.Controllers
{
    public class LexicalErrorCatalogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LexicalErrorCatalogController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await _context.LexicalErrorCatalog
                .OrderBy(x => x.Code)
                .ToListAsync();

            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new LexicalErrorCatalogFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LexicalErrorCatalogFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string normalizedCode = model.Code.Trim().ToUpper();

            bool exists = await _context.LexicalErrorCatalog
                .AnyAsync(x => x.Code == normalizedCode);

            if (exists)
            {
                ModelState.AddModelError(nameof(model.Code), "Ya existe un error con ese código.");
                return View(model);
            }

            var entity = new LexicalErrorCatalog
            {
                Code = normalizedCode,
                Name = model.Name.Trim(),
                Description = model.Description.Trim(),
                IsActive = model.IsActive
            };

            _context.LexicalErrorCatalog.Add(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Error léxico creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.LexicalErrorCatalog.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new LexicalErrorCatalogFormViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LexicalErrorCatalogFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var entity = await _context.LexicalErrorCatalog.FindAsync(model.Id);

            if (entity == null)
            {
                return NotFound();
            }

            string normalizedCode = model.Code.Trim().ToUpper();

            bool exists = await _context.LexicalErrorCatalog
                .AnyAsync(x => x.Code == normalizedCode && x.Id != model.Id);

            if (exists)
            {
                ModelState.AddModelError(nameof(model.Code), "Ya existe otro error con ese código.");
                return View(model);
            }

            entity.Code = normalizedCode;
            entity.Name = model.Name.Trim();
            entity.Description = model.Description.Trim();
            entity.IsActive = model.IsActive;

            _context.LexicalErrorCatalog.Update(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Error léxico actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

    }
}