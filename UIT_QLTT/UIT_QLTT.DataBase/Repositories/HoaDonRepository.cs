using Microsoft.EntityFrameworkCore;
using UIT_QLTT.Database.Data;
using UIT_QLTT.Database.Models;

namespace QLBH.DataBase.Repositories;
public interface IHoaDonRepository
{
    Task<List<Hoadon>> GetAsync();
}
public class HoaDonRepository : IHoaDonRepository
{
    private readonly QLBHContext _dbContext;
    public HoaDonRepository(QLBHContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Hoadon>> GetAsync()
    {
        IQueryable<Hoadon> queryable = _dbContext.Hoadons;
        return await queryable.AsNoTracking().ToListAsync();
    }

    public void Add(Hoadon hoaDon)
    {
        _dbContext.Hoadons.Add(hoaDon);
    }
}
