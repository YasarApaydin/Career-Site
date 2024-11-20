using Kariyer.Models;

namespace Kariyer.ViewModels
{
    public class IlanDetayViewModel
    {
     
       
        public string? IlanBaslik { get; set; }
        public Guid ilanId { get; set; }
        public string? IlanIdEncrypted { get; set; } 
        public string? Maas { get; set; }
        public Kullanici? Kullanici { get; set; }
        public IsVeren? IsVerenler { get; set; }
        public string? CalismaKonumu { get; set; }
        public string? YasAralık { get; set; }
        public string? IlanAciklama { get; set; }
        public int? IlanNo { get; set; }
        public int? AlinacakPersonelSayi { get; set; }
        public string? Cinsiyet { get; set; }
        public DateTime? BasTarih { get; set; }

      
        public string? FirmaAd { get; set; }
        public string? Logo { get; set; }
        public string? WebAdresi { get; set; }
        public string? Pozisyon { get; set; }
     
        public string? Deneyim { get; set; }
        public string? Egitim { get; set; }
        public string? Sektor { get; set; }
        public List<string> Nitelikler { get; set; } = new List<string>();
		public string? CalismaSekli { get; set; }

        public bool IsVeren { get; set; }
        public IEnumerable<DigerIsIlan>? DigerIsIlanlari1 { get; set; }
        public IEnumerable<DigerIsIlan>? DigerIsIlanlari { get; set; }
    }

    public class DigerIsIlan
    {public string? CalismaSekli { get; set; }
        public string? Maas { get; set; }
        public string? CalismaKonumu { get; set; }
        public string? Sektor { get; set; }
        public string? Logo { get; set; }
        public string? IlanBaslik { get; set; }
        public int? IlanNo { get; set; }
        public string? FirmaAd { get; set; }
        public DateTime? BasTarih { get; set; }
        public string?  Pozisyon { get; set; }
        public string? IlanIdEncrypted { get; set; }

    }
}

