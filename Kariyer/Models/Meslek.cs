using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Meslek
{
    public Guid Id { get; set; }

    public string Ad { get; set; } = null!;

    public bool Sil { get; set; }

    public virtual ICollection<IsArayan> IsArayans { get; set; } = new List<IsArayan>();

    public virtual ICollection<Ozgecmi> Ozgecmis { get; set; } = new List<Ozgecmi>();

    public virtual ICollection<Profil> Profils { get; set; } = new List<Profil>();
}
