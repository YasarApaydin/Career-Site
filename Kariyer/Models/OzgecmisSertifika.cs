using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class OzgecmisSertifika
{
    public Guid Id { get; set; }

    public Guid? OzgecmisId { get; set; }

    public string? Sertifika { get; set; }

    public string? Kurum { get; set; }

    public int? AlinmaYili { get; set; }

    public bool? Sil { get; set; }

    public virtual Ozgecmi? Ozgecmis { get; set; }
}
