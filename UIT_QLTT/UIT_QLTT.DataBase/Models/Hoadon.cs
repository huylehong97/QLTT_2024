using System;
using System.Collections.Generic;

namespace UIT_QLTT.Database.Models;

public partial class Hoadon
{
    public int Sohd { get; set; }

    public DateTime? Nghd { get; set; }

    public string? Makh { get; set; }

    public string? Manv { get; set; }

    public decimal? Trigia { get; set; }

    public virtual ICollection<Cthd> Cthds { get; set; } = new List<Cthd>();

    public virtual Khachhang? MakhNavigation { get; set; }

    public virtual Nhanvien? ManvNavigation { get; set; }
}
