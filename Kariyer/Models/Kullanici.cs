using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Kullanici
{
    public Guid Id { get; set; }

    public int? KullaniciTurId { get; set; }

    public string? Tck { get; set; }

    public string? Eposta { get; set; }

    public string? Parola { get; set; }

    public string? ParolaSifirla { get; set; }

    public string? TelefonNo { get; set; }

    public bool? Sil { get; set; }

    public virtual ICollection<Begeni> Begenis { get; set; } = new List<Begeni>();

    public virtual ICollection<Cv> Cvs { get; set; } = new List<Cv>();

    public virtual ICollection<IsArayan> IsArayans { get; set; } = new List<IsArayan>();

    public virtual ICollection<IsVeren> IsVerens { get; set; } = new List<IsVeren>();

    public virtual ICollection<KullaniciPozisyonlar> KullaniciPozisyonlars { get; set; } = new List<KullaniciPozisyonlar>();

    public virtual KullaniciTur? KullaniciTur { get; set; }

    public virtual ICollection<Ozgecmi> Ozgecmis { get; set; } = new List<Ozgecmi>();

    public virtual ICollection<Profil> Profils { get; set; } = new List<Profil>();
}
