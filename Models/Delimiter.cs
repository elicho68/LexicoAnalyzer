namespace LexicoAnalyzer.Web.Models
{
    public class Delimiter
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}