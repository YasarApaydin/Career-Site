using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Askerlik
{
    public Guid Id { get; set; }

    public string Ad { get; set; } = null!;

    public int Sıra { get; set; }

    public bool Sil { get; set; }

    public virtual ICollection<IsArayan> IsArayans { get; set; } = new List<IsArayan>();
}
