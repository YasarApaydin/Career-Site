using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Ozgecmi
{
    public Guid Id { get; set; }

    public Guid? KullaniciId { get; set; }

    public Guid? DeneyimId { get; set; }

    public Guid? SurucuBelgeId { get; set; }

    public Guid? MeslekId { get; set; }

    public Guid? SrcbelgeId { get; set; }

    public Guid? EgitimId { get; set; }

    public byte? CinsiyetId { get; set; }

    public string? Hakkinda { get; set; }

    public string? Foto { get; set; }

    public string? AdSoyad { get; set; }

    public string? Sehir { get; set; }

    public string? Ilce { get; set; }

    public string? Adres { get; set; }

    public DateTime? OlusturmaTarihi { get; set; }

    public DateOnly? DogumTar { get; set; }

    public bool? MedeniHal { get; set; }

    public int? Sira { get; set; }

    public int? OzgecmisSay { get; set; }

    public bool? Sil { get; set; }

    public virtual Cinsiyet? Cinsiyet { get; set; }

    public virtual Deneyim? Deneyim { get; set; }

    public virtual Egitim? Egitim { get; set; }

    public virtual Kullanici? Kullanici { get; set; }

    public virtual Meslek? Meslek { get; set; }

    public virtual ICollection<OzgecmisBeceri> OzgecmisBeceris { get; set; } = new List<OzgecmisBeceri>();

    public virtual ICollection<OzgecmisEgitim> OzgecmisEgitims { get; set; } = new List<OzgecmisEgitim>();

    public virtual ICollection<OzgecmisIsDeneyimleri> OzgecmisIsDeneyimleris { get; set; } = new List<OzgecmisIsDeneyimleri>();

    public virtual ICollection<OzgecmisReferan> OzgecmisReferans { get; set; } = new List<OzgecmisReferan>();

    public virtual ICollection<OzgecmisSertifika> OzgecmisSertifikas { get; set; } = new List<OzgecmisSertifika>();

    public virtual ICollection<OzgecmisYabanciDil> OzgecmisYabanciDils { get; set; } = new List<OzgecmisYabanciDil>();

    public virtual Srcbelge? Srcbelge { get; set; }

    public virtual SurucuBelge? SurucuBelge { get; set; }
}
