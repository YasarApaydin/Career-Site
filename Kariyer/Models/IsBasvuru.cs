using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class IsBasvuru
{
    public Guid Id { get; set; }

    public Guid? IsIlanlarId { get; set; }

    public Guid? IsVerenId { get; set; }

    public Guid? IsArayanId { get; set; }

    public bool? Sil { get; set; }

    public virtual IsArayan? IsArayan { get; set; }

    public virtual IsIlanlar? IsIlanlar { get; set; }

    public virtual IsVeren? IsVeren { get; set; }
}
