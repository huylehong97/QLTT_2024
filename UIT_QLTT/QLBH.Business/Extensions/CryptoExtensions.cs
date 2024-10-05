using System.Security.Cryptography;
using System.Text;

namespace QLBH.Business.Extensions;

public static class CryptoExtensions
{
    public static string GetMD5Hash(this object obj)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.ASCII.GetBytes(obj.ToString());
            var hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
