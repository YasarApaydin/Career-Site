using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class SurucuBelge
{
    public Guid Id { get; set; }

    public string Ad { get; set; } = null!;

    public int Sira { get; set; }

    public bool Sil { get; set; }

    public virtual ICollection<IsArayan> IsArayans { get; set; } = new List<IsArayan>();

    public virtual ICollection<Ozgecmi> Ozgecmis { get; set; } = new List<Ozgecmi>();
}
