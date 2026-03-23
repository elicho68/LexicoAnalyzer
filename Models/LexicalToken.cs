namespace LexicoAnalyzer.Web.Models
{
    public class LexicalToken
    {
        public int Order { get; set; }
        public string Lexeme { get; set; } = string.Empty;
        public TokenType Type { get; set; }
        public bool IsValid { get; set; }
        public int Position { get; set; }
        public string? ErrorMessage { get; set; }
    }
}