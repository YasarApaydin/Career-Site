using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class OzgecmisBeceri
{
    public Guid Id { get; set; }

    public Guid? OzgecmisId { get; set; }

    public string? Beceri { get; set; }

    public double? Yuzde { get; set; }

    public bool? Sil { get; set; }

    public virtual Ozgecmi? Ozgecmis { get; set; }
}
