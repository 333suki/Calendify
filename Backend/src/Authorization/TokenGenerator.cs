using System.Security.Cryptography;
using System.Text;
using Backend.Models;

namespace Backend.Authorization;

public static class TokenGenerator {
    public static readonly string Secret = "verysecretkey";

    public static string GenerateAccessToken(long expiryDuration, string subject, Role role) {
        long nowTime = DateTimeOffset.Now.ToUnixTimeSeconds();

        string header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
        string payload = $"{{\"sub\":\"{subject}\",\"iat\":{nowTime},\"exp\":{nowTime + expiryDuration},\"role\":{Convert.ToInt32(role)}}}";

        string headerEncoded = AuthUtils.Base64UrlEncode(Encoding.UTF8.GetBytes(header));
        string payloadEncoded = AuthUtils.Base64UrlEncode(Encoding.UTF8.GetBytes(payload));

        string toSign = $"{headerEncoded}.{payloadEncoded}";
        string signatureEncoded;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret))) {
            signatureEncoded = AuthUtils.Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes(toSign)));
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
