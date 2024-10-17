using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NRediSearch;
using StackExchange.Redis;
using UIT_QLTT.Database.Models;

namespace QLBH.Business.Services;

public interface IDbRedisService
{
    Task CreateTableAsync(string tableName, string primaryKey, Dictionary<string, string> columns);
    Task<Dictionary<string, string>> GetDataFromTableAsync(string tableName, string primaryKey);
    Task<List<Dictionary<string, string>>> SearchInTableAsync(string tableName, string column, string searchValue);
    Task AddListToRedisAsync<T>(string keyPrefix, List<T> dataList);
    Task<List<T>> GetListFromRedisAsync<T>(string keyPrefix, int count);
    Task CreateIndexAsync();
    Task<List<T>> SearchCustomerByNameAsync<T>(string name);
}

public class DbRedisService : IDbRedisService
{
    private readonly IDatabase _db;
    private IConfiguration _configuration;

    public DbRedisService(IConfiguration configuration)
    {
        _configuration = configuration;
        var redis = ConnectionMultiplexer.Connect(_configuration["Redis"]);
        _db = redis.GetDatabase();
    }

    public async Task CreateTableAsync(string tableName, string primaryKey, Dictionary<string, string> columns)
    {
        // Sử dụng hash để mô phỏng một bảng, với tableName:primaryKey là khóa chính
        var redisKey = $"{tableName}:{primaryKey}";
        var batch = _db.CreateBatch();  // Tạo batch cho pipelining

        // Lưu tất cả các cột vào hash
        foreach (var column in columns)
        {
            batch.HashSetAsync(redisKey, column.Key, column.Value);
        }
        batch.Execute();  // Thực hiện batch lưu hàng loạt
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


    public async Task<List<Dictionary<string, string>>> SearchInTableAsync(string tableName, string column, string searchValue)
    {
        var resultList = new List<Dictionary<string, string>>();

        // Quét tất cả các khóa Redis bắt đầu bằng tên bảng với SCAN thay vì KEYS
        var server = _db.Multiplexer.GetServer("119.82.133.195:6379");
        var keys = server.KeysAsync(pattern: $"{tableName}:*");

        await foreach (var key in keys)
        {
            // Kiểm tra giá trị của cột trước khi lấy toàn bộ hash
            var columnValue = await _db.HashGetAsync(key, column);
            if (columnValue == searchValue)
            {
                // Lấy toàn bộ hash nếu giá trị khớp
                var hashEntries = await _db.HashGetAllAsync(key);
                var dict = hashEntries.ToDictionary(
                    x => x.Name.ToString(),
                    x => x.Value.ToString()
                );

                resultList.Add(dict);
            }
        }

        return resultList;

    }

    public async Task AddListToRedisAsync<T>(string keyPrefix, List<T> dataList)
    {
        if (dataList == null || dataList.Count == 0)
        {
            throw new ArgumentException("Danh sách truyền vào không được rỗng");
        }
        var batch = _db.CreateBatch();  // Tạo batch cho pipelining

        for (int i = 0; i < dataList.Count; i++)
        {
            var item = dataList[i];
            string key = $"{keyPrefix}:{i + 1}"; // Tạo khóa cho từng phần tử trong danh sách

            // Chuyển đổi đối tượng thành JSON
            string jsonData = JsonConvert.SerializeObject(item);

            // Lưu đối tượng vào Redis
            batch.StringSetAsync(key, jsonData);
        }
        batch.Execute();  // Thực hiện batch lưu hàng loạt

    }

    // Hàm lấy danh sách từ Redis
    public async Task<List<T>> GetListFromRedisAsync<T>(string keyPrefix, int count)
    {
        var resultList = new List<T>();
        int batchSize = 5000;  // Chia batch thành từng nhóm 100 lệnh
        for (int batchStart = 1; batchStart <= count; batchStart += batchSize)
        {
            var batch = _db.CreateBatch();
            var tasks = new List<Task<RedisValue>>();

            for (int i = batchStart; i < batchStart + batchSize && i <= count; i++)
            {
                string key = $"{keyPrefix}:{i}";
                tasks.Add(batch.StringGetAsync(key));
            }

            batch.Execute();
            var jsonResults = await Task.WhenAll(tasks);

            foreach (var jsonData in jsonResults)
            {
                if (jsonData.HasValue)
                {
                    T item = JsonConvert.DeserializeObject<T>(jsonData);
                    resultList.Add(item);
                }
            }
        }

        return resultList;
    }

    // Tạo chỉ mục cho các trường của đối tượng Khachhang1
    public async Task CreateIndexAsync()
    {
        var client = new Client("khachhang1_idx", _db);  // Tạo client cho chỉ mục

        var schema = new Schema()
            .AddTextField("MaKH", 1.0)   // Field Name (tìm kiếm văn bản)
            .AddNumericField("Id");      // Field Id (tìm kiếm số)

        // Tạo chỉ mục với schema
        await client.CreateIndexAsync(schema, new Client.ConfiguredIndexOptions());
    }

    public async Task<List<T>> SearchCustomerByNameAsync<T>(string name)
    {
        var client = new Client("khachhang1_idx", _db);

        // Tạo câu truy vấn tìm kiếm theo tên
        var query = new Query($"@Name:{name}");

        // Thực hiện tìm kiếm
        var results = await client.SearchAsync(query);

        var khachhangList = new List<T>();

        foreach (var doc in results.Documents)
        {
            // Ánh xạ các trường từ Document sang model Khachhang1
            string jsonData = doc["jsonData"].ToString();  // 'jsonData' là field chứa chuỗi JSON

            // Deserialize chuỗi JSON thành đối tượng Khachhang1
            var khachhang = JsonConvert.DeserializeObject<T>(jsonData);
            khachhangList.Add(khachhang);
        }
        return khachhangList;
    }


}
