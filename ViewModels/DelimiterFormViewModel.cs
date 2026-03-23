using System.ComponentModel.DataAnnotations;

namespace LexicoAnalyzer.Web.ViewModels
{
    public class DelimiterFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El delimitador es obligatorio.")]
        [StringLength(10, ErrorMessage = "El delimitador no puede exceder 10 caracteres.")]
        [Display(Name = "Delimitador")]
        public string Symbol { get; set; } = string.Empty;

        [Display(Name = "Activo")]
        public bool IsActive { get; set; } = true;
    }
}