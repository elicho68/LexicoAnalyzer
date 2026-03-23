using System.ComponentModel.DataAnnotations;

namespace LexicoAnalyzer.Web.Models
{
    public class AnalyzerViewModel
    {
        [Required(ErrorMessage = "Debe ingresar una cadena para analizar.")]
        [Display(Name = "Cadena de entrada")]
        public string InputText { get; set; } = string.Empty;

        public LexicalAnalysisResult? Result { get; set; }
    }
}