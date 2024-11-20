using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Kurslar
{
    public Guid Id { get; set; }

    public Guid IsVerenId { get; set; }

    public Guid IlceId { get; set; }

    public string Aciklama { get; set; } = null!;

    public string Ad { get; set; } = null!;

    public string? Kurum { get; set; }

    public DateOnly Tarih { get; set; }

    public string? Sure { get; set; }

    public bool Sil { get; set; }

    public virtual Ilce Ilce { get; set; } = null!;

    public virtual IsVeren IsVeren { get; set; } = null!;
}
