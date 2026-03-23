using LexicoAnalyzer.Web.Models;
using LexicoAnalyzer.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LexicoAnalyzer.Web.Controllers
{
    public class AnalyzerController : Controller
    {
        private readonly ILexicalAnalyzerService _lexicalAnalyzerService;

        public AnalyzerController(ILexicalAnalyzerService lexicalAnalyzerService)
        {
            _lexicalAnalyzerService = lexicalAnalyzerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new AnalyzerViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(AnalyzerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Result = await _lexicalAnalyzerService.AnalyzeAsync(model.InputText);

            return View(model);
        }
    }
}