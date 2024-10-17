using Microsoft.AspNetCore.Mvc;
using QLBH.Business.Services;

namespace QLBH.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class KhachHangController : ControllerBase
{
    private readonly IHoaDonService _hoaDonService;
    private readonly IKhachHangService _khachHangService;
    public KhachHangController(IHoaDonService hoaDonService, IKhachHangService khachHangService)
    {
        _hoaDonService = hoaDonService;
        _khachHangService = khachHangService;
    }

    [HttpGet("sync")]
    public async Task<IActionResult> SyncKhAsync()
    {
        await _khachHangService.SyncDataToRedis();
        return Ok();
    }

    [HttpGet("redis")]
    public async Task<IActionResult> RedisKhAsync()
    {
        return Ok(await _khachHangService.GetKhachHangFromRedis());
    }

    [HttpGet("redis-search")]
    public async Task<IActionResult> CreattRedisSearchAsync([FromQuery] string name)
    {
        return Ok(await _khachHangService.SearchFromRedis(name));
    }

    [HttpGet]
    public async Task<IActionResult> SQLKHAsync()
    {
        return Ok(await _khachHangService.GetKhacHang());
    }

    [HttpGet("search")]
    public async Task<IActionResult> SQLSearchKHAsync([FromQuery] string name)
    {
        return Ok(await _khachHangService.Search(name));
    }

    [HttpGet("data-join")]
    public async Task<IActionResult> KHJoinAsync(string makh)
    {
        return Ok(await _khachHangService.GetKhachhang1HD(makh));
    }

    [HttpGet("data-join/cache")]
    public async Task<IActionResult> KHJoinCacheAsync(string makh)
    {
        return Ok(await _khachHangService.GetKhachhang1HDCache(makh));
    }
}
