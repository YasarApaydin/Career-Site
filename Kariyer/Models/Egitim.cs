using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Egitim
{
    public Guid Id { get; set; }

    public string Ad { get; set; } = null!;

    public int Sira { get; set; }

    public bool Sil { get; set; }

    public virtual ICollection<IsArayan> IsArayans { get; set; } = new List<IsArayan>();

    public virtual ICollection<IsIlanlar> IsIlanlars { get; set; } = new List<IsIlanlar>();

    public virtual ICollection<Ozgecmi> Ozgecmis { get; set; } = new List<Ozgecmi>();

    public virtual ICollection<OzgecmisEgitim> OzgecmisEgitims { get; set; } = new List<OzgecmisEgitim>();
}
