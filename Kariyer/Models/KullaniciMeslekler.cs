using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class KullaniciMeslekler
{
    public Guid Id { get; set; }

    public Guid MeslekId { get; set; }

    public Guid KullaniciId { get; set; }

    public bool? Sil { get; set; }

    public virtual Kullanici Kullanici { get; set; } = null!;

    public virtual Meslek Meslek { get; set; } = null!;

    public virtual ICollection<Profil> Profils { get; set; } = new List<Profil>();
}
