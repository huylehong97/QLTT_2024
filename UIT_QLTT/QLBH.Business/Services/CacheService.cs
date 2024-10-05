using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QLBH.Business.Common;
using QLBH.Business.Extensions;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace QLBH.Business.Services;
public interface ICacheService
{
    string CreateKey(params object[] varyBy);
    T Get<T>(string cacheKeyPrefix);
    bool Set(string cacheKeyPrefix, dynamic objectToCache, DateTime expiredDate);
    bool Set(string cacheKeyPrefix, dynamic objectToCache);
    bool Remove(string cacheKeyPrefix);
}
public class CacheService : ICacheService
{

    private IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _outCache;

    public CacheService(
        IConfiguration configuration,
        IMemoryCache memoryCache)
    {
        _configuration = configuration;
        _memoryCache = memoryCache;
        _outCache = new RedisCache(new RedisCacheOptions
        {
            Configuration = _configuration["Redis"],
            InstanceName = "_cache"
        });
    }

    public string CreateKey(params object[] varyBy)
    {
        var stackTrace = new StackTrace();
        MethodBase method = stackTrace.GetFrame(1).GetMethod();
        var callingMethod = method.Name;
        if (method.ReflectedType != null)
        {
            if (callingMethod == "MoveNext")
            {
                callingMethod = method.ReflectedType.FullName;
                MatchCollection matchCollection = Regex.Matches(callingMethod, @"\+<[^>]*>");
                if (matchCollection.Count > 0)
                {
                    var methodName = matchCollection[0].ToString();
                    var i = callingMethod.IndexOf("+", StringComparison.Ordinal);

                    callingMethod = callingMethod.Substring(0, i) + "." + methodName.Substring(2, methodName.Length - 3);
                }
            }
            else
            {
                callingMethod = method.ReflectedType.FullName + "." + callingMethod;
            }
        }

        var paramString = new StringBuilder();
        if (varyBy != null)
        {
            foreach (var param in varyBy)
            {
                if (param == null)
                {
                    paramString.Append("null,");
                }
                else
                {
                    var paramType = param.GetType();
                    if (paramType.IsValueType || paramType == typeof(string))
                    {
                        if (paramType == typeof(DateTime))
                        {
                            paramString.Append(((DateTime)param).ToString("yyyyMMddHHmmss"));
                        }
                        else
                        {
                            paramString.Append(param);
                        }
                    }
                    else
                    {
                        paramString.Append(param.GetMD5Hash() ?? param.ToString());
                    }

                    paramString.Append(",");
                }
            }
        }

        var finalParamString = paramString.ToString().TrimEnd(',');
        if (finalParamString.Length > 120)
        {
            finalParamString = finalParamString.GetMD5Hash();
        }

        return $"QLTT-{callingMethod}-{finalParamString}";
    }


    public T Get<T>(string cacheKeyPrefix)
    {
        try
        {
            string cacheKey = $"{cacheKeyPrefix}";

            var cachedObjectJson = _memoryCache.Get<string>(cacheKey);

            if (string.IsNullOrEmpty(cachedObjectJson))
            {
                cachedObjectJson = _outCache.GetString(cacheKey);
            }

            if (!string.IsNullOrEmpty(cachedObjectJson))
            {
                return JsonConvert.DeserializeObject<T>(cachedObjectJson);
            }
        }
        catch (Exception ex)
        {
        }
        return default(T);
    }

    public bool Remove(string cacheKeyPrefix)
    {
        if (!string.IsNullOrEmpty(cacheKeyPrefix))
        {
            string cacheKey = $"{cacheKeyPrefix}";
            _outCache.Remove(cacheKey);
            _memoryCache.Remove(cacheKey);
            return true;
        }
        return false;
    }

    public bool Set(string cacheKeyPrefix, dynamic objectToCache, DateTime expiredDate)
    {
        bool IsModified = false;
        try
        {
            if (objectToCache != null)
            {
                string cacheKey = $"{cacheKeyPrefix}";
                string serializedObjectToCache = JsonConvert.SerializeObject(objectToCache);
                _memoryCache.Remove(cacheKey);
                _outCache.Remove(cacheKey);
                _outCache.SetString(cacheKey, serializedObjectToCache,
                    new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpiration = expiredDate
                    });
                _memoryCache.Set(cacheKey, serializedObjectToCache, expiredDate);
                IsModified = true;
            }
        }
        catch (Exception ex)
        {
        }
        return IsModified;
    }

    public bool Set(string cacheKeyPrefix, dynamic objectToCache)
    {
        return Set(cacheKeyPrefix, objectToCache, ConfigConstants.CacheDefaultTime());
    }
}
