namespace QLBH.Business.Models;

public partial class SanphamModel
{
    public string Masp { get; set; } = null!;

    public string? Tensp { get; set; }

    public string? Dvt { get; set; }

    public string? Nuocsx { get; set; }

    public decimal? Gia { get; set; }

    public virtual ICollection<CthdModel> Cthds { get; set; } = new List<CthdModel>();
}
