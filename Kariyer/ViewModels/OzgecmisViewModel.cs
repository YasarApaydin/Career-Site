using Kariyer.Models;

namespace Kariyer.ViewModels
{
    public class OzgecmisViewModel
    {
        public MedeniHal? MedeniHal { get; set; }
        public string? MedeniHal1 { get; set; }

        public IEnumerable<OzgecmisViewModel>? Ozgecmis { get; set; }
        public string? NitelikAd { get; set; }


        public string? Referans { get; set; }
        public string? ReferansPozisyon { get; set; }
        public string? IletisimBilgi { get; set; }


        public string? AdSoyad { get; set; }
      
  
       
     
    
        public string? Meslek { get; set; }


        public string? Dil { get; set; }

        public double? Seviye { get; set; }

        public string? Sehir { get; set; }
        public string? Adres { get; set; }
        public string? Ilce { get; set; }


        public Guid? OzgecmisId { get; set; }
        public string? KurumAd { get; set; }
        public string? Pozisyon { get; set; }
        public string? BasBityil { get; set; } // İş başlangıç ve bitiş yılı
        public string? Aciklama { get; set; }
        public string? Foto { get; set; }
        public IFormFile? FotoDosya { get; set; }
        public int OzgecmisNo { get; set; }

        public int Sira { get; set; }
        public string? OkulAd { get; set; }
        public string? MezuniyetYil { get; set; }
        public Guid? MezunuyetBilgiId { get; set; }
        public string? Bolum { get; set; }
        public byte CinsiyetId { get; set; }
        public DateOnly? DogumTarihi { get; set; }
        public Guid? EgitimId { get; set; }

        public string? Beceri { get; set; }
        public double? Yuzde { get; set; }
        public Guid? MeslekId { get; set; }
        public Guid? DeneyimId { get; set; }

        public Guid? SurucuBelgeId { get; set; }
        public string? Sertifika { get; set; }
        public string? Kurum { get; set; }
        public int AlinmaYili { get; set; }
        public Guid? SrcbelgeId { get; set; }

        public List<Srcbelge>? Srcbelgeler { get; set; }
        public List<Egitim>? MezunuyetBilgiler { get; set; }
        public List<SurucuBelge>? SurucuBelgeler { get; set; }
        public List<Cinsiyet>? Cinsiyetler { get; set; }
        public List<Egitim>? Egitimler { get; set; }
        public List<Deneyim>? Deneyimler { get; set; }
        public List<Meslek>? Meslekler { get; set; }



        public List<OzgecmisBeceri>? Beceriler { get; set; } = new List<OzgecmisBeceri> { new OzgecmisBeceri() };

        public List<OzgecmisYabanciDil> YabnciDil { get; set; } = new List<OzgecmisYabanciDil> { new OzgecmisYabanciDil() };
        public List<OzgecmisReferan> Referanslar { get; set; } = new List<OzgecmisReferan> { new OzgecmisReferan() };
        public List<OzgecmisEgitim> Egitim { get; set; } = new List<OzgecmisEgitim> { new OzgecmisEgitim() };
        public List<OzgecmisSertifika> Sertifikalar { get; set; } = new List<OzgecmisSertifika> { new OzgecmisSertifika() };
      
        public List<OzgecmisIsDeneyimleri> Deneyimlerim { get; set; } = new List<OzgecmisIsDeneyimleri> { new OzgecmisIsDeneyimleri() };

    }





}

