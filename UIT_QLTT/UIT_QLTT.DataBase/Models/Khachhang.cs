using System;
using System.Collections.Generic;

namespace UIT_QLTT.Database.Models;

public partial class Khachhang
{
    public string Makh { get; set; } = null!;

    public string? Hoten { get; set; }

    public string? Dchi { get; set; }

    public string? Sodt { get; set; }

    public DateTime? Ngsinh { get; set; }

    public DateTime? Ngdk { get; set; }

    public decimal? Doanhso { get; set; }

    public string? Loaikh { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();
}
