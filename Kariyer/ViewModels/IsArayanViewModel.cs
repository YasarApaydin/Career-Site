using Kariyer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kariyer.ViewModels
{
    public class IsArayanViewModel 
    {
        public string? Meslek { get; set; }
        public DateOnly? DogumTar { get; set; }
        public string? Adres { get; set; } 
        public string? AdSoyad { get; set; }
        public string? Kimlik { get; set; }
        public MedeniHal? MedeniHal { get; set; }
        public EngelDurum? EngelDurum { get; set; }
        public byte CinsiyetId { get; set; }
        public Guid? EgitimId { get; set; }
        public Guid? MeslekId { get; set; }
        public Guid? DeneyimId { get; set; }
     

        [Required(ErrorMessage = "Şehir seçilmelidir.")]
        public Guid? SehirId { get; set; }

        [Required(ErrorMessage = "İlçe seçilmelidir.")]
        public Guid? IlceId { get; set; }


        public Guid? KullaniciId { get; set; }
        public string? Foto { get; set; }
        public IFormFile? FotoDosya { get; set; }
        public int? KullaniciTurId { get; set; }
        public Guid? SurucuBelgeId { get; set; }

        public Guid? SrcbelgeId { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik numarası 11 haneli olmalıdır.")]
        public string? Tck { get; set; }
        public Guid? UyrukId { get; set; }
        public string? KullaniciTur { get; set; }


       
        public string? Parola { get; set; }

       
        [Compare("Parola", ErrorMessage = "Şifreler eşleşmiyor.")]
        [DataType(DataType.Password)]
        public string? ParolaTekrar { get; set; }


       

        public Kullanici? KullaniciBilgiler { get; set; }

        public Guid? PozisyonId { get; set; }


        public List<Egitim>? Egitimler { get; set; }
        public List<Meslek>? Meslekler { get; set; }





        public string[]? SelectedPozisyonIds { get; set; } 
        public List<SelectListItem> Pozisyonlar { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem>? Sehirler { get; set; }
        public IEnumerable<SelectListItem>? Ilceler { get; set; }

        public List<Srcbelge>? Srcbelgeler { get; set; }
       
        public List<SurucuBelge>? SurucuBelgeler { get; set; }
        public List<KullaniciTur>? KullaniciTurler { get; set; }
        public List<Uyruk>? Uyruklar { get; set; }
        public List<Cinsiyet>? Cinsiyetler { get; set; }
        public List<Askerlik>? Askerlikler { get; set; }
        public List<Deneyim>? Deneyimler { get; set; }
    }
}




