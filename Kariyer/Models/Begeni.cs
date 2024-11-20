using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Begeni
{
    public Guid Id { get; set; }

    public Guid? KullaniciId { get; set; }

    public Guid? IlanId { get; set; }

    public DateTime? BegeniTarih { get; set; }

    public bool? Sil { get; set; }

    public virtual IsIlanlar? Ilan { get; set; }

    public virtual Kullanici? Kullanici { get; set; }
}
