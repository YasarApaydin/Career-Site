using System.ComponentModel.DataAnnotations;

namespace Kariyer.ViewModels
{
    public class ResetPasswordViewModel
    {
   
        public string? Token { get; set; }  // Şifre sıfırlama işlemi için gerekli olan token

        [Required(ErrorMessage = "Yeni şifre girilmesi zorunludur.")]
        [StringLength(100, ErrorMessage = "Şifre en az {2} ve en fazla {1} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        public string? NewPassword { get; set; }  

        [Required(ErrorMessage = "Şifre onayı girilmesi zorunludur.")]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string? ConfirmPassword { get; set; }  // Kullanıcının yeni şifreyi onaylaması için gerekli alan
    }
}
