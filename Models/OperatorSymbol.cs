namespace LexicoAnalyzer.Web.Models
{
    public class OperatorSymbol
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}