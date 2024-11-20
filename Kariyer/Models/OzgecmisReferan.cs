using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class OzgecmisReferan
{
    public Guid Id { get; set; }

    public string? Referans { get; set; }

    public string? Pozisyon { get; set; }

    public string? IletisimBilgi { get; set; }

    public bool? Sil { get; set; }

    public Guid? OzgecmisId { get; set; }

    public virtual Ozgecmi? Ozgecmis { get; set; }
}
