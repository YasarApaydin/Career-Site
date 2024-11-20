using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class CalismaKonumu
{
    public Guid Id { get; set; }

    public string Ad { get; set; } = null!;

    public int Sira { get; set; }

    public bool Sil { get; set; }

    public virtual ICollection<IsIlanlar> IsIlanlars { get; set; } = new List<IsIlanlar>();
}
