using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class SektorPozisyon
{
    public Guid Id { get; set; }

    public Guid? SektorId { get; set; }

    public Guid? PozisyonId { get; set; }

    public bool? Sil { get; set; }

    public virtual Pozisyon? Pozisyon { get; set; }

    public virtual Sektor? Sektor { get; set; }
}
