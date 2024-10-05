using System;
using System.Collections.Generic;

namespace UIT_QLTT.Database.Models;

public partial class Cthd
{
    public int Sohd { get; set; }

    public string Masp { get; set; } = null!;

    public int? Sl { get; set; }

    public virtual Sanpham MaspNavigation { get; set; } = null!;

    public virtual Hoadon SohdNavigation { get; set; } = null!;
}
