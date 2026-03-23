using LexicoAnalyzer.Web.Data;
using LexicoAnalyzer.Web.Models;
using LexicoAnalyzer.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LexicoAnalyzer.Web.Controllers
{
    public class ReservedWordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservedWordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await _context.ReservedWords
                .OrderBy(x => x.Word)
                .ToListAsync();

            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ReservedWordFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservedWordFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string normalizedWord = model.Word.Trim().ToLower();

            bool exists = await _context.ReservedWords
                .AnyAsync(x => x.Word == normalizedWord);

            if (exists)
            {
                ModelState.AddModelError(nameof(model.Word), "Esa palabra reservada ya existe.");
                return View(model);
            }

            var entity = new ReservedWord
            {
                Word = normalizedWord,
                IsActive = model.IsActive
            };

            _context.ReservedWords.Add(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Palabra reservada creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.ReservedWords.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ReservedWordFormViewModel
            {
                Id = entity.Id,
                Word = entity.Word,
                IsActive = entity.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReservedWordFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var entity = await _context.ReservedWords.FindAsync(model.Id);

            if (entity == null)
            {
                return NotFound();
            }

            string normalizedWord = model.Word.Trim().ToLower();

            bool exists = await _context.ReservedWords
                .AnyAsync(x => x.Word == normalizedWord && x.Id != model.Id);

            if (exists)
            {
                ModelState.AddModelError(nameof(model.Word), "Ya existe otra palabra reservada con ese valor.");
                return View(model);
            }

            entity.Word = normalizedWord;
            entity.IsActive = model.IsActive;

            _context.ReservedWords.Update(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Palabra reservada actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}