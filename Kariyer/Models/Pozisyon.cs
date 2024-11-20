using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Pozisyon
{
    public Guid Id { get; set; }

    public string Ad { get; set; } = null!;

    public int Sira { get; set; }

    public bool Sil { get; set; }

    public virtual ICollection<IsIlanlar> IsIlanlars { get; set; } = new List<IsIlanlar>();

    public virtual ICollection<KullaniciPozisyonlar> KullaniciPozisyonlars { get; set; } = new List<KullaniciPozisyonlar>();

    public virtual ICollection<SektorPozisyon> SektorPozisyons { get; set; } = new List<SektorPozisyon>();
}
