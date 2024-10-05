using Microsoft.EntityFrameworkCore;
using QLBH.Business;
using QLBH.Business.Services;
using QLBH.DataBase.Repositories;
using UIT_QLTT.Database.Data;

var builder = WebApplication.CreateBuilder(args);

// Thêm Swagger vào dịch vụ
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(MappingProfile).Assembly);

// Đăng ký DbContext với chuỗi kết nối
builder.Services.AddDbContext<QLBHContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký Repository và Service
builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IKhachHang1Repository, KhachHang1Repository>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Sử dụng Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;  // Swagger ở URL gốc
    });
}

app.UseRouting();
app.MapControllers();

app.Run();
