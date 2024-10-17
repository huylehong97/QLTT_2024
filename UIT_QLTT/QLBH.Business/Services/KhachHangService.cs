using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QLBH.Business.Models;
using QLBH.DataBase.Repositories;
using System.Collections.Generic;
using UIT_QLTT.Database.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QLBH.Business.Services;

public interface IKhachHangService
{
    Task<List<Khachhang1>> GetKhacHang();
    Task<List<Khachhang1HDModel>> GetKhachhang1HD(string maKh);
    Task<List<Khachhang1HDModel>> GetKhachhang1HDCache(string maKh);
    Task<Khachhang1> Search(string name);

    Task SyncDataToRedis();
    Task SyncDataToRedisV2();
    Task<List<Khachhang1>> GetKhachHangFromRedis();
    Task<List<Khachhang1HDModel>> GetKhachhang1HDFromRedis();
    Task<List<Khachhang1>> SearchFromRedis(string name);
}
public class KhachHangService : IKhachHangService
{
    private readonly IKhachHang1Repository _khachHang1Repository;
    private readonly IHoaDonRepository _hoaDonRepository;
    private readonly ICacheService _cache;
    private readonly IDbRedisService _db;
    private readonly int PageSize = 50000;
    private readonly int Time = 1000;
    public KhachHangService(ICacheService cacheService, IKhachHang1Repository khachHang1Repository, IDbRedisService db, IHoaDonRepository hoaDonRepository)
    {
        _cache = cacheService;
        _khachHang1Repository = khachHang1Repository;
        _db = db;
        _hoaDonRepository = hoaDonRepository;
    }

    public async Task<List<Khachhang1>> GetKhacHang()
    {

        return await _khachHang1Repository.GetBySizeAsync(PageSize);
    }

    public async Task<List<Khachhang1HDModel>> GetKhachhang1HD(string maKh)
    {
        var data = new List<Khachhang1HDModel>();
        for (int i = 0; i < Time; i++)
        {
            data = await JoinDataTest(maKh);
        }
        return data;
    }

    public async Task<List<Khachhang1HDModel>> GetKhachhang1HDCache(string maKh)
    {
        var keyCache = _cache.CreateKey(maKh);
        List<Khachhang1HDModel> datas = null;
        for (int i = 0; i < Time; i++)
        {
            datas = _cache.Get<List<Khachhang1HDModel>>(keyCache);
            if (datas == null)
            {
                datas = await JoinDataTest(maKh);
                _cache.Set(keyCache, datas);
            }
        }

        return datas;
    }

    public async Task<Khachhang1> Search(string name)
    {
        return await _khachHang1Repository.Search(name);
    }

    public async Task SyncDataToRedis()
    {
        var datas = await _khachHang1Repository.GetBySizeAsync(PageSize);
        if (!datas.IsNullOrEmpty())
        {
            await _db.AddListToRedisAsync(nameof(Khachhang1), datas);
        }
    }
    public async Task SyncDataToRedisV2()
    {
        var datas = await JoinDataTest();
        if (!datas.IsNullOrEmpty())
        {
            await _db.AddListToRedisAsync(nameof(Khachhang1HDModel), datas);
        }
    }

    public async Task<List<Khachhang1>> SearchFromRedis(string name)
    {

        return await _db.SearchCustomerByNameAsync<Khachhang1>(name);
    }
    public async Task<List<Khachhang1HDModel>> GetKhachhang1HDFromRedis()
    {
        return await _db.GetListFromRedisAsync<Khachhang1HDModel>(nameof(Khachhang1HDModel), PageSize);
    }

    public async Task<List<Khachhang1>> GetKhachHangFromRedis()
    {
        return await _db.GetListFromRedisAsync<Khachhang1>(nameof(Khachhang1), PageSize);
    }

    private async Task<List<Khachhang1HDModel>> JoinDataTest(string makh = "")
    {
        var queryKh = _khachHang1Repository.GetQuery().Take(PageSize);
        if (!string.IsNullOrEmpty(makh))
        {
            queryKh = queryKh.Where(e => e.Makh == makh);
        }
        var queryHd = _hoaDonRepository.GetQuery();

        return await queryKh.GroupJoin(
            queryHd,
            kh => kh.Makh,
            hd => hd.Makh,
            (kh, hds) => new { KhachHang = kh, HoaDons = hds }
        )
        .SelectMany(
            x => x.HoaDons.DefaultIfEmpty(),
            (x, hd) => new Khachhang1HDModel
            {
                Sohd = hd.Sohd,
                Nghd = hd.Nghd,
                Makh = x.KhachHang.Makh,
                Manv = hd.Manv,
                Trigia = hd.Trigia,
                Hoten = x.KhachHang.Hoten,
                Dchi = x.KhachHang.Dchi,
                Sodt = x.KhachHang.Sodt,
                Ngsinh = x.KhachHang.Ngsinh,
                Ngdk = x.KhachHang.Ngdk,
                Doanhso = x.KhachHang.Doanhso,
                Loaikh = x.KhachHang.Loaikh
            }
        ).ToListAsync();
    }
}
