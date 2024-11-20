using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class KullaniciTur
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public bool Sil { get; set; }

    public virtual ICollection<Kullanici> Kullanicis { get; set; } = new List<Kullanici>();
}
