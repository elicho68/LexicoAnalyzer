using LexicoAnalyzer.Web.Models;

namespace LexicoAnalyzer.Web.Services
{
    public interface ILexicalAnalyzerService
    {
        Task<LexicalAnalysisResult> AnalyzeAsync(string input);
    }
}