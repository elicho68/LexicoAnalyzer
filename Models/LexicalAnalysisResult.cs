namespace LexicoAnalyzer.Web.Models
{
    public class LexicalAnalysisResult
    {
        public string Input { get; set; } = string.Empty;
        public bool IsValid { get; set; }
        public int TotalTokens { get; set; }
        public int ValidTokens { get; set; }
        public int InvalidTokens { get; set; }

        public List<LexicalToken> Tokens { get; set; } = new();
        public List<LexicalError> Errors { get; set; } = new();
    }
}