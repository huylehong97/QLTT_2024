namespace QLBH.Business.Models;

public partial class CthdModel
{
    public int Sohd { get; set; }

    public string Masp { get; set; } = null!;

    public int? Sl { get; set; }

    public virtual SanphamModel MaspNavigation { get; set; } = null!;

    public virtual HoadonModel SohdNavigation { get; set; } = null!;
}
