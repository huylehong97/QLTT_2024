using Microsoft.EntityFrameworkCore;
using UIT_QLTT.Database.Data;
using UIT_QLTT.Database.Models;

namespace QLBH.DataBase.Repositories;
public interface IKhachHang1Repository
{
    Task<List<Khachhang1>> GetAsync();
    Task<List<Khachhang1>> GetBySizeAsync(int size);
    Task<Khachhang1> Search(string search);
    IQueryable<Khachhang1> GetQuery();
}
public class KhachHang1Repository : IKhachHang1Repository
{
    private readonly QLBHContext _dbContext;
    public KhachHang1Repository(QLBHContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Khachhang1>> GetBySizeAsync(int size)
    {
        IQueryable<Khachhang1> queryable = _dbContext.Khachhang1s;
        if (size > 0)
        {
            queryable = queryable.Take(size);
        }
        return await queryable.AsNoTracking().ToListAsync();
    }

    public async Task<List<Khachhang1>> GetAsync()
    {
        IQueryable<Khachhang1> queryable = _dbContext.Khachhang1s;
        return await queryable.AsNoTracking().ToListAsync();
    }

    public void Add(Khachhang1 hoaDon)
    {
        _dbContext.Khachhang1s.Add(hoaDon);
    }

    public async Task<Khachhang1> Search(string search)
    {
        IQueryable<Khachhang1> queryable = _dbContext.Khachhang1s;
        return await queryable.AsNoTracking().FirstOrDefaultAsync(q => q.Makh == search);
    }

    public IQueryable<Khachhang1> GetQuery()
    {
        IQueryable<Khachhang1> queryable = _dbContext.Khachhang1s;

        return queryable;
    }
}
