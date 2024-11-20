namespace Kariyer.ViewModels
{
    public class IlanBasvuruViewModel
    {

        public string? KullaniciIdEncrypted { get; set; }
        public bool IsVeren { get; set; }
        public bool IsArayan { get; set; }
        public string? Eposta { get; set; }
        public int kullaniciTür { get; set; }
        public string? AdSoyad { get; set; }
        public string? Meslek { get; set; }

        public DateOnly? BasvuruTarihi { get; set; }
        public string? Foto { get; set; }
        public IEnumerable<IlanBasvuruViewModel>? IlanBasvuru { get; set; }
    }
}
