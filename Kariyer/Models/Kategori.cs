using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Kategori
{
    public Guid Id { get; set; }

    public string? Ad { get; set; }

    public int? Sira { get; set; }

    public bool? Sil { get; set; }

    public virtual ICollection<SektorKategori> SektorKategoris { get; set; } = new List<SektorKategori>();
}
