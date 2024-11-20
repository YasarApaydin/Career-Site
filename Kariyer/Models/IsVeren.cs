using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class IsVeren
{
    public Guid Id { get; set; }

    public Guid? KullaniciId { get; set; }

    public Guid? MevcutPersonelSayiId { get; set; }

    public Guid? IlceId { get; set; }

    public string? FirmaUnvan { get; set; }

    public string? AdSoyad { get; set; }

    public string? VergiDairesi { get; set; }

    public string? VergiNo { get; set; }

    public string? Adres { get; set; }

    public string? Foto { get; set; }

    public string? FirmaWebAdres { get; set; }

    public bool? Sil { get; set; }

    public virtual ICollection<IsBasvuru> IsBasvurus { get; set; } = new List<IsBasvuru>();

    public virtual ICollection<IsIlanlar> IsIlanlars { get; set; } = new List<IsIlanlar>();

    public virtual Kullanici? Kullanici { get; set; }

    public virtual ICollection<Kurslar> Kurslars { get; set; } = new List<Kurslar>();

    public virtual MevcutPersonelSayi? MevcutPersonelSayi { get; set; }
}
