using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Sehir
{
    public Guid Id { get; set; }

    public string Ad { get; set; } = null!;

    public bool Sil { get; set; }

    public virtual ICollection<Ilce> Ilces { get; set; } = new List<Ilce>();
}
