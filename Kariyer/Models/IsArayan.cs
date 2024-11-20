using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class IsArayan
{
    public Guid Id { get; set; }

    public Guid? KullaniciId { get; set; }

    public Guid? EgitimId { get; set; }

    public Guid? İlceId { get; set; }

    public Guid? DeneyimId { get; set; }

    public Guid? SrcbelgeId { get; set; }

    public Guid? SurucuBelgeId { get; set; }

    public Guid? UyrukId { get; set; }

    public Guid? MeslekId { get; set; }

    public byte? CinsiyetId { get; set; }

    public DateOnly? DogumTar { get; set; }

    public bool? MedeniHal { get; set; }

    public bool? Engelli { get; set; }

    public string? AdSoyad { get; set; }

    public string? Adres { get; set; }

    public string? Foto { get; set; }

    public bool? Sil { get; set; }

    public virtual Deneyim? Deneyim { get; set; }

    public virtual Egitim? Egitim { get; set; }

    public virtual ICollection<IsBasvuru> IsBasvurus { get; set; } = new List<IsBasvuru>();

    public virtual Kullanici? Kullanici { get; set; }

    public virtual Meslek? Meslek { get; set; }

    public virtual Srcbelge? Srcbelge { get; set; }

    public virtual SurucuBelge? SurucuBelge { get; set; }

    public virtual Uyruk? Uyruk { get; set; }
}
