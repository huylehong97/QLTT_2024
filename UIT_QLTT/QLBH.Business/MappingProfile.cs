using AutoMapper;
using QLBH.Business.Models;
using UIT_QLTT.Database.Models;

namespace QLBH.Business;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Cthd, CthdModel>();
        CreateMap<Hoadon, HoadonModel>();
        CreateMap<Khachhang, KhachhangModel>();
        CreateMap<Khachhang1, KhachhangModel>();
        CreateMap<Nhanvien, NhanvienModel>();
        CreateMap<Sanpham, SanphamModel>();
    }
}
