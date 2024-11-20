using Kariyer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kariyer.ViewModels
{
    public class ProfilViewModel
    {

        public string? Cvpdf { get; set; }
        public string? KullaiciId { get; set; }
        public bool? cvVarMi { get; set; }
        public string? AdSoyad { get; set; }
        public string? Eposta { get; set; }
        public string? TelefonNo { get; set; }
        public string? Adres { get; set; }
        public string? FirmaUnvan { get; set; }  
        public string? Foto { get; set; }
       
        public int kullaniciTür { get; set; }
        public bool IsArayan { get; set; }  
        public bool IsVeren { get; set; }
        public string? Meslek { get; set; }
     
        public List<SelectListItem>? Uyruklar { get; set; }
        public Guid? UyrukId { get; set; }
    
        public Guid? SehirId { get; set; }
        public string? Ilce { get; set; }
        public Guid? MeslekId { get; set; }
        public List<SelectListItem>? Meslekler { get; set; }
        public Guid? IlceId { get; set; }  // Seçilen ilçe ID'si
        public List<SelectListItem>? Ilceler { get; set; }

        
        public DateOnly? DogumTar { get; set; }
        public int yas { get; set; }
  
        public string? Deneyim { get; set; }

        public Guid? SosyalBaglantiId { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Linkedin { get; set; }
        public string? Github { get; set; }

    }
}
