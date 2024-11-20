using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Cv
{
    public Guid Id { get; set; }

    public Guid? KullaniciId { get; set; }

    public string? Cvpdf { get; set; }

    public DateTime? OlusturmaTarihi { get; set; }

    public int? OzgecmisSay { get; set; }

    public bool? Sil { get; set; }

    public virtual Kullanici? Kullanici { get; set; }
}
