using Kariyer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kariyer.ViewModels
{
	public class AnasayfaViewModel
	{
       
        public IEnumerable<SelectListItem>? CalismaSekliListesi { get; set; }
        public IEnumerable<SelectListItem>? KategoriListesi { get; set; }
        public int? AktifIsIlanlari { get; set; }
        public int? UyeFirma { get; set; }
        public int? KayitliKullanici { get; set; }
        public string? MeslekAnahtarKelime { get; set; }
        public string? CalismaSekliAd { get; set; }
        public Guid? IlanId { get; set; }
        public int? IlanNo { get; set; }
        public string? KategoriAd { get; set; }
        public IEnumerable<IsIlaniViewModel>? IsIlanlari { get; set; }
      

    }

	


}
