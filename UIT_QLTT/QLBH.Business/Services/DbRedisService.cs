using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace QLBH.Business.Services;

public class DbRedisService
{
    private readonly IDatabase _db;
    private IConfiguration _configuration;

    public DbRedisService(IConfiguration configuration)
    {
        _configuration = configuration;
        var redis = ConnectionMultiplexer.Connect(_configuration["Redis"]);
        _db = redis.GetDatabase();
    }

    // Hàm để khởi tạo dữ liệu trong "bảng"
    public async Task CreateTableAsync(string tableName, string primaryKey, Dictionary<string, string> columns)
    {
        // Sử dụng hash để mô phỏng một bảng, với tableName:primaryKey là khóa chính
        var redisKey = $"{tableName}:{primaryKey}";

        // Lưu tất cả các cột vào hash
        foreach (var column in columns)
        {
            await _db.HashSetAsync(redisKey, column.Key, column.Value);
        }
    }

    // Hàm để truy vấn dữ liệu theo primary key
    public async Task<Dictionary<string, string>> GetDataFromTableAsync(string tableName, string primaryKey)
    {
        var redisKey = $"{tableName}:{primaryKey}";
        var hashEntries = await _db.HashGetAllAsync(redisKey);

        // Chuyển đổi từ HashEntry[] sang Dictionary<string, string>
        return hashEntries.ToDictionary(
            x => x.Name.ToString(),
            x => x.Value.ToString()
        );
    }


    // Hàm tìm kiếm dữ liệu theo giá trị cột
    public async Task<List<Dictionary<string, string>>> SearchInTableAsync(string tableName, string column, string searchValue)
    {
        var resultList = new List<Dictionary<string, string>>();

        // Quét tất cả các khóa Redis bắt đầu bằng tên bảng
        var server = _db.Multiplexer.GetServer(_configuration["Redis"]);
        foreach (var key in server.Keys(pattern: $"{tableName}:*"))
        {
            // Lấy dữ liệu từ hash
            var hashEntries = await _db.HashGetAllAsync(key);
            var dict = hashEntries.ToDictionary(
                x => x.Name.ToString(),
                x => x.Value.ToString()
            );

            // Kiểm tra xem giá trị cột có khớp không
            if (dict.ContainsKey(column) && dict[column] == searchValue)
            {
                resultList.Add(dict);
            }
        }

        return resultList;
    }

}
