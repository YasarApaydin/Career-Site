using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Ilce
{
    public Guid Id { get; set; }

    public Guid SehirId { get; set; }

    public string Ad { get; set; } = null!;

    public bool Sil { get; set; }

    public virtual ICollection<Kurslar> Kurslars { get; set; } = new List<Kurslar>();

    public virtual ICollection<Profil> Profils { get; set; } = new List<Profil>();

    public virtual Sehir Sehir { get; set; } = null!;
}
