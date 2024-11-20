using System;
using System.Collections.Generic;

namespace Kariyer.Models;

public partial class Cinsiyet
{
    public byte Id { get; set; }

    public string? Ad { get; set; }

    public bool? Sil { get; set; }

    public virtual ICollection<Ozgecmi> Ozgecmis { get; set; } = new List<Ozgecmi>();
}
