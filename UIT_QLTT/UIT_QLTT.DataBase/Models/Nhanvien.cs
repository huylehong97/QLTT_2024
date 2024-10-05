using System;
using System.Collections.Generic;

namespace UIT_QLTT.Database.Models;

public partial class Nhanvien
{
    public string Manv { get; set; } = null!;

    public string? Hoten { get; set; }

    public string? Sodt { get; set; }

    public DateTime? Ngvl { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();
}
