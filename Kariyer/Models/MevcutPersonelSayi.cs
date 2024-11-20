using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class MevcutPersonelSayi
{
    public Guid Id { get; set; }

    public string Ad { get; set; } = null!;

    public int Sira { get; set; }

    public bool Sil { get; set; }

    public virtual ICollection<IsVeren> IsVerens { get; set; } = new List<IsVeren>();
}
