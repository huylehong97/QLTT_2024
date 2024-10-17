namespace QLBH.Business.Models;

public partial class HoadonModel 
{
    public int? Sohd { get; set; }

    public DateTime? Nghd { get; set; }

    public string? Makh { get; set; }

    public string? Manv { get; set; }

    public decimal? Trigia { get; set; }

    public virtual ICollection<CthdModel> Cthds { get; set; } = new List<CthdModel>();

    public virtual KhachhangModel? MakhNavigation { get; set; }

    public virtual NhanvienModel? ManvNavigation { get; set; }
}
