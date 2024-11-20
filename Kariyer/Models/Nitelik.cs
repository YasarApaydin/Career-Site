using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Nitelik
{
    public Guid Id { get; set; }

    public string Ad { get; set; } = null!;

    public int Sira { get; set; }

    public bool Sil { get; set; }

    public virtual ICollection<IsIlanlarNitelik> IsIlanlarNiteliks { get; set; } = new List<IsIlanlarNitelik>();
}
