using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class OzgecmisYabanciDil
{
    public Guid ıd { get; set; }

    public Guid? OzgecmisId { get; set; }

    public string? Dil { get; set; }

    public double? Seviye { get; set; }

    public bool? Sil { get; set; }

    public virtual Ozgecmi? Ozgecmis { get; set; }
}
