namespace LexicoAnalyzer.Web.Models
{
    public class LexicalError
    {
        public string Lexeme { get; set; } = string.Empty;
        public int Position { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}