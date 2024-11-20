using System.ComponentModel.DataAnnotations;

namespace Kariyer.ViewModels
{
    public class SifremiUnuttumViewModel
    {
        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        public string? Email { get; set; }
    }
}
