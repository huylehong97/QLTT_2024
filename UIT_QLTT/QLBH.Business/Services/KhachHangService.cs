using QLBH.DataBase.Repositories;
using UIT_QLTT.Database.Models;

namespace QLBH.Business.Services;

public interface IKhachHangService
{
    Task<List<Khachhang1>> GetHoaDon();
}
public class KhachHangService : IKhachHangService
{
    private readonly IKhachHang1Repository _khachHang1Repository;
    private readonly ICacheService _cache;
    public KhachHangService(ICacheService cacheService, IKhachHang1Repository khachHang1Repository)
    {
        _cache = cacheService;
        _khachHang1Repository = khachHang1Repository;
    }

    public async Task<List<Khachhang1>> GetHoaDon()
    {
        var keycache = _cache.CreateKey();
        var hoadons = _cache.Get<List<Khachhang1>>(keycache);
        if (hoadons != null) return hoadons;
        hoadons = await _khachHang1Repository.GetAsync();
        _cache.Set(keycache, hoadons);
        return hoadons;
    }
}
