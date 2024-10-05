using System;
using System.Collections.Generic;

namespace UIT_QLTT.Database.Models;

public partial class Sanpham
{
    public string Masp { get; set; } = null!;

    public string? Tensp { get; set; }

    public string? Dvt { get; set; }

    public string? Nuocsx { get; set; }

    public decimal? Gia { get; set; }

    public virtual ICollection<Cthd> Cthds { get; set; } = new List<Cthd>();
}
