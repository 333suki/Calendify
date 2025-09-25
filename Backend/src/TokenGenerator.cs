using System.Security.Cryptography;
using System.Text;

namespace Backend;

public static class TokenGenerator {
    public static string GenerateAccessToken(long expiryDuration, string subject) {
        string secret = "verysecretkey";
        long nowTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        
        string header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
        string payload = $"{{\"sub\":\"{subject}\",\"iat\":{nowTime},\"exp\":{nowTime + expiryDuration}}}";
        
        string headerEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(header)).Replace("+", "-").Replace("/", "_").TrimEnd('=');
        string payloadEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload)).Replace("+", "-").Replace("/", "_").TrimEnd('=');
        
        string toSign = $"{headerEncoded}.{payloadEncoded}";
        string signatureEncoded;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret))) {
            signatureEncoded = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(toSign))).Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }
        
        return $"{headerEncoded}.{payloadEncoded}.{signatureEncoded}";
    }

    public static string GenerateRefreshToken() {
        Random random = new Random();
        const int length = 30;
        const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
        return new string(Enumerable.Range(1, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }
}
