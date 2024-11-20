using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Profil
{
    public Guid Id { get; set; }

    public Guid? KullaniciId { get; set; }

    public Guid? UyrukId { get; set; }

    public Guid? IlceId { get; set; }

    public Guid? SosyalBaglantiId { get; set; }

    public Guid? MeslekId { get; set; }

    public int? Yas { get; set; }

    public string? Deneyim { get; set; }

    public string? PostaKodu { get; set; }

    public bool? Sil { get; set; }

    public virtual Ilce? Ilce { get; set; }

    public virtual Kullanici? Kullanici { get; set; }

    public virtual Meslek? Meslek { get; set; }

    public virtual SosyalBaglanti? SosyalBaglanti { get; set; }

    public virtual Uyruk? Uyruk { get; set; }
}
