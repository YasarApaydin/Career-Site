using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Duyuru
{
    public Guid Id { get; set; }

    public string Ad { get; set; } = null!;

    public string Foto { get; set; } = null!;

    public string Aciklama { get; set; } = null!;

    public DateOnly BaslaTarih { get; set; }

    public DateOnly BitisTarih { get; set; }

    public bool Sil { get; set; }
}
