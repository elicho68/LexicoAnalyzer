namespace LexicoAnalyzer.Web.Models
{
    public class ReservedWord
    {
        public int Id { get; set; }
        public string Word { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}