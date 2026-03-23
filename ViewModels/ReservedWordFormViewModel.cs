using System.ComponentModel.DataAnnotations;

namespace LexicoAnalyzer.Web.ViewModels
{
    public class ReservedWordFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La palabra reservada es obligatoria.")]
        [StringLength(50, ErrorMessage = "La palabra no puede exceder 50 caracteres.")]
        [Display(Name = "Palabra reservada")]
        public string Word { get; set; } = string.Empty;

        [Display(Name = "Activo")]
        public bool IsActive { get; set; } = true;
    }
}