using QLBH.DataBase.Repositories;
using UIT_QLTT.Database.Models;

namespace QLBH.Business.Services;

public interface IHoaDonService
{
    Task<List<Hoadon>> GetHoaDon();
}
public class HoaDonService : IHoaDonService
{
    private readonly IHoaDonRepository _hoaDonRepository;
    private readonly ICacheService _cache;
    public HoaDonService(IHoaDonRepository hoaDonRepository, ICacheService cacheService)
    {
        _hoaDonRepository = hoaDonRepository;
        _cache = cacheService;
    }

    public async Task<List<Hoadon>> GetHoaDon()
    {
        var keycache = _cache.CreateKey();
        var hoadons = _cache.Get<List<Hoadon>>(keycache);
        if (hoadons != null) return hoadons;
        hoadons = await _hoaDonRepository.GetAsync();
        _cache.Set(keycache, hoadons);
        return hoadons;
    }
}
