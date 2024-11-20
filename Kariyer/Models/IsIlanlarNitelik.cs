using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class IsIlanlarNitelik
{
    public Guid Id { get; set; }

    public Guid? NitelikId { get; set; }

    public Guid? IsIlanlarId { get; set; }

    public int? Sira { get; set; }

    public bool? Sil { get; set; }

    public virtual IsIlanlar? IsIlanlar { get; set; }

    public virtual Nitelik? Nitelik { get; set; }
}
