namespace QLBH.Business.Models;

public partial class NhanvienModel
{
    public string Manv { get; set; } = null!;

    public string? Hoten { get; set; }

    public string? Sodt { get; set; }

    public DateTime? Ngvl { get; set; }

    public virtual ICollection<HoadonModel> Hoadons { get; set; } = new List<HoadonModel>();
}
