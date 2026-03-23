using System.ComponentModel.DataAnnotations;

namespace LexicoAnalyzer.Web.ViewModels
{
    public class LexicalErrorCatalogFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(50, ErrorMessage = "El código no puede exceder 50 caracteres.")]
        [Display(Name = "Código")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(250, ErrorMessage = "La descripción no puede exceder 250 caracteres.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Activo")]
        public bool IsActive { get; set; } = true;
    }
}