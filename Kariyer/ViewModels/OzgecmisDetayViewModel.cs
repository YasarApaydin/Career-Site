using Kariyer.Models;

namespace Kariyer.ViewModels
{
    public class OzgecmisDetayViewModel
    {
        public string? MeslekAd
        {
            get; set;
        }
        public Guid Id { get; set; }
        public MedeniHal? MedeniHal { get; set; }

        public bool? EngelDurumu { get; set; }
        public string? AdSoyad { get; set; }
        public string? Deneyim { get; set; }
        public string? Telefon { get; set; }
        public string? Eposta { get; set; }
        public string? Sehir { get; set; }
        public string? Ilce { get; set; }
        public string? Adres { get; set; }
        public DateOnly? DogumTarihi { get; set; }
        public string? NitelikAd { get; set; }
        public string? Foto { get; set; }
      
        public string? Cinsiyet { get; set; }
        public string? SurucuBelge { get; set; }
        public string? SrcBelge { get; set; }
        public List<OzgecmisEgitim> Egitimler { get; set; } = new();
        public List<OzgecmisIsDeneyimleri> Deneyimler { get; set; } = new();
        public List<OzgecmisBeceri> Beceriler { get; set; } = new();
        public List<OzgecmisReferan> Referanslar { get; set; } = new();
        public List<OzgecmisSertifika> Sertifikalar { get; set; } = new();
        public List<OzgecmisYabanciDil> YabanciDiller { get; set; } = new();
    }
}
