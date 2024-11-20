using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class SektorKategori
{
    public Guid Id { get; set; }

    public Guid? SektorId { get; set; }

    public Guid? KategoriId { get; set; }

    public bool? Sil { get; set; }

    public virtual Kategori? Kategori { get; set; }

    public virtual Sektor? Sektor { get; set; }
}
