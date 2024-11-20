using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class SosyalBaglanti
{
    public Guid Id { get; set; }

    public string? Facebook { get; set; }

    public string? Twitter { get; set; }

    public string? Linkedin { get; set; }

    public string? Github { get; set; }

    public bool? Sil { get; set; }

    public virtual ICollection<Profil> Profils { get; set; } = new List<Profil>();
}
