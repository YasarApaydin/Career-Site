using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class OzgecmisEgitim
{
    public Guid Id { get; set; }

    public Guid? OzgecmisId { get; set; }

    public Guid? MezuniyetBigiId { get; set; }

    public string? OkulAd { get; set; }

    public string? MezuniyetYil { get; set; }

    public string? Bolum { get; set; }

    public bool? Sil { get; set; }

    public virtual Egitim? MezuniyetBigi { get; set; }

    public virtual Ozgecmi? Ozgecmis { get; set; }
}
