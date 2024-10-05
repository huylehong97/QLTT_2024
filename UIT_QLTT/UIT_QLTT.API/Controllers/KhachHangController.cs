using Microsoft.AspNetCore.Mvc;
using QLBH.Business.Services;
using QLBH.DataBase.Repositories;

namespace QLBH.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class KhachHangController : ControllerBase
{
    private readonly IHoaDonService _hoaDonService;
    private readonly IKhachHangService _khachHangService;
    public KhachHangController(IHoaDonService hoaDonService,  IKhachHangService khachHangService)
    {
        _hoaDonService = hoaDonService;
        _khachHangService = khachHangService;
    }

    [HttpGet("hoa-don")]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _hoaDonService.GetHoaDon());
    }  
    
    [HttpGet]
    public async Task<IActionResult> GetKHAsync()
    {
        return Ok(await _khachHangService.GetHoaDon());
    }
}
