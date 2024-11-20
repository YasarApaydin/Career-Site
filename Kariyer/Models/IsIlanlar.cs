using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class IsIlanlar
{
    public Guid Id { get; set; }

    public Guid? CalismaSekliId { get; set; }

    public Guid? IsVerenId { get; set; }

    public Guid? DeneyimId { get; set; }

    public Guid? EgitimId { get; set; }

    public Guid? PosizyonId { get; set; }

    public byte? CinsiyetId { get; set; }

    public string? IlanBaslik { get; set; }

    public string? Maas { get; set; }

    public string? CalismaKonumu { get; set; }

    public string? YasAralık { get; set; }

    public string? IlanAciklama { get; set; }

    public DateTime? BasTarih { get; set; }

    public int? AlinacakPersonelSayi { get; set; }

    public int? Goruntulenme { get; set; }

    public int? IlanNo { get; set; }

    public bool? Sil { get; set; }

    public virtual ICollection<Begeni> Begenis { get; set; } = new List<Begeni>();

    public virtual CalismaSekli? CalismaSekli { get; set; }

    public virtual Deneyim? Deneyim { get; set; }

    public virtual Egitim? Egitim { get; set; }

    public virtual ICollection<IsBasvuru> IsBasvurus { get; set; } = new List<IsBasvuru>();

    public virtual ICollection<IsIlanlarNitelik> IsIlanlarNiteliks { get; set; } = new List<IsIlanlarNitelik>();

    public virtual IsVeren? IsVeren { get; set; }

    public virtual Pozisyon? Posizyon { get; set; }
}
