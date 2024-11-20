using Kariyer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Kariyer.ViewModels
{
    public class IsVerenViewModel
    {

        public Guid? KullaniciId { get; set; }
        public string? AdSoyad { get; set; }
        public int? KullaniciTurId { get; set; }
        public string? VergiDairesi { get; set; }
        public string? KullaniciTur { get; set; }

        [Required(ErrorMessage = "Şehir Seçilmelidir.")]
        public Guid? SehirId { get; set; }
        [Required(ErrorMessage ="İlçe Seçilmelidir.")]
        public Guid? IlceId { get; set; }

        public string? VergiNo { get; set; }
        public string? Eposta { get; set; }
        public string? Telefon { get; set; }
        public string? Adres { get; set; }
        [DataType(DataType.Password)]
        public string? Parola { get; set; }
        public Guid? MevcutPersonelSayiId { get; set; }
        public string? FirmaWebAdres { get; set; }
        public string? FirmaUnvan { get; set; }
        public string? Foto { get; set; }
        public IFormFile? FotoDosya { get; set; } 
        [Compare("Parola", ErrorMessage = "Şifreler eşleşmiyor.")]
        [DataType(DataType.Password)]
        public string? ParolaTekrar { get; set; }
        public List<MevcutPersonelSayi>? MevcutPersonelSayi { get; set; }
       public IEnumerable<SelectListItem>? Sehirler { get; set; }
        public IEnumerable<SelectListItem>? Ilceler { get; set; }
    }
}
