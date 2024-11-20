using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class OzgecmisIsDeneyimleri
{
    public Guid Id { get; set; }

    public Guid? OzgecmisId { get; set; }

    public string? KurumAd { get; set; }

    public string? Pozisyon { get; set; }

    public string? BasBityil { get; set; }

    public string? Acıiklama { get; set; }

    public bool? Sil { get; set; }

    public virtual Ozgecmi? Ozgecmis { get; set; }
}
