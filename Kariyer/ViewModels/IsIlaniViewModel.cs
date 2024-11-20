using Kariyer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Kariyer.ViewModels
{
    public class IsIlaniViewModel
    {
        public string? KullaiciId { get; set; }
        public string? AdSoyad { get; set; }
        public Guid? PozisyonId { get; set; }
        public string? Pozisyon { get; set; }
        public Guid? CalismaSekliId { get; set; }

        public Guid? IsVerenId { get; set; }

        public Guid? DeneyimId { get; set; }
        public bool IsArayan { get; set; }
        public bool IsVeren { get; set; }
        public Guid? EgitimId { get; set; }
        public int kullaniciTür { get; set; }
        public Guid? ilanId { get; set; }
		public Guid? SektorId { get; set; }
        public string? FirmaAd { get; set; }
        public string? Foto { get; set; }
        public string? CalismaSekli { get; set; }
        public string? Eposta { get; set; }
       
        public Guid? KategoriId { get; set; }
        public byte CinsiyetId { get; set; }
     
        public Guid? NitelikId { get; set; }
        public string? PozisyonAd { get; set;}
        public Guid? IsIlanlarId { get; set; }
       
        [Required]
        public string? IlanBaslik { get; set; }

        public int? Goruntulenme { get; set; }

        [Required]
        public string? CalismaKonumu { get; set; }
        [Required(ErrorMessage = "Maaş aralığı girilmesi zorunludur.")]
        //[RegularExpression(@"^\d+(\.\d{1,3})? - \d+(\.\d{1,3})?$", ErrorMessage = "Maaş aralığı sadece sayılar ve '-' karakteri içerebilir. Örnek: 15.000 - 20.000")]
        public string? Maas { get; set; }

        [Required(ErrorMessage = "Yaş aralığı girilmesi zorunludur.")]
        [RegularExpression(@"^\d{1,2}-\d{1,2}$", ErrorMessage = "Yaş aralığı sadece sayılar ve '-' karakteri içerebilir. Örnek: 18-35")]
        public string? YasAralık { get; set; }
       
        [Required]
          public string? IlanAciklama { get; set; }
   
		public DateTime? BasTarih { get; set; }
      

		public IEnumerable<SelectListItem>? CalismaSekliListesi { get; set; }
		public IEnumerable<SelectListItem>? KategoriListesi { get; set; }

		public string? MeslekAnahtarKelime { get; set; }
		public string? CalismaSekliAd { get; set; }
		public string? KategoriAd { get; set; }
      


        public int? AlinacakPersonelSayi { get; set; }
       
        public string? NitelikAd { get; set; } 
        public int? IlanNo { get; set; }
      
        public List<Cinsiyet>? Cinsiyetler { get; set; }
        public List<Pozisyon>? Pozisyonlar { get; set; }
        public List<Deneyim>? Deneyimler { get; set; }
        public List<CalismaSekli>? CalismaSekilleri { get; set; }
        public List<Egitim>? Egitimler { get; set; }
      
        public Kullanici? Kullanici { get; set; }
        public List<string>? NitelikSatirlari { get; set; }
        public IsVeren? IsVerenler { get; set; }
        public bool BegeniVarMi { get; set; }
        public string? IlanIdEncrypted { get; set; }



        public int ToplamSayfa { get; set; }
        public int MevcutSayfa { get; set; }

        public IEnumerable<IsIlaniViewModel>? IsIlanlari { get; set; }
        public List<IsIlanlar>? IsIlanlar { get; set; }
       
        public int basvuruSay { get; set; }
        public string? ZamanFarki
        {
            get
            {
                if (BasTarih == null)
                {
                    return null;
                }

                var zamanfark = DateTime.Now - BasTarih.Value;
                if (zamanfark.TotalDays >= 1)
                {
                    return $"{(int)zamanfark.TotalDays} Gün Önce";
                }
                else if (zamanfark.TotalHours >= 1)
                {
                    return $"{(int)zamanfark.TotalHours} Saat Önce";
                }
                else
                {
                    return $"{(int)zamanfark.TotalMinutes} Dakika Önce";
                }
            }
        }


    }
}
