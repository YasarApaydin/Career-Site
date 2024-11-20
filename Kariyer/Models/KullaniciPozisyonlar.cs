using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class KullaniciPozisyonlar
{
    public Guid Id { get; set; }

    public Guid? PozisyonId { get; set; }

    public Guid? KullaniciId { get; set; }

    public bool? Sil { get; set; }

    public virtual Kullanici? Kullanici { get; set; }

    public virtual Pozisyon? Pozisyon { get; set; }
}
