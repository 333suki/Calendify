using System.Security.Cryptography;
using System.Text;
using Backend.Models;

namespace Backend.Authorization;

public static class TokenGenerator {
    private static string secret = "verysecretkey";

    public static string GenerateAccessToken(long expiryDuration, string subject, Role role) {
        long nowTime = DateTimeOffset.Now.ToUnixTimeSeconds();

        string header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
        string payload = $"{{\"sub\":\"{subject}\",\"iat\":{nowTime},\"exp\":{nowTime + expiryDuration},\"role\":{Convert.ToInt32(role)}}}";

        string headerEncoded = Base64UrlEncode(Encoding.UTF8.GetBytes(header));
        string payloadEncoded = Base64UrlEncode(Encoding.UTF8.GetBytes(payload));

        string toSign = $"{headerEncoded}.{payloadEncoded}";
        string signatureEncoded;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(TokenGenerator.secret))) {
            signatureEncoded = Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes(toSign)));
        }

        return $"{headerEncoded}.{payloadEncoded}.{signatureEncoded}";
    }

    public static bool VerifyToken(string token) {
        string[] tokenSplit = token.Split('.');
        if (tokenSplit.Length != 3) {
            return false;
        }

        string headerEncoded = tokenSplit[0];
        string payloadEncoded = tokenSplit[1];
        string signature = tokenSplit[2];


        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(TokenGenerator.secret))) {
            if (Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes($"{headerEncoded}.{payloadEncoded}"))) !=
                signature) {
                return false;
            }
        }

        return true;
    }

    public static string GenerateRefreshToken() {
        Random random = new Random();
        const int length = 30;
        const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
        return new string(Enumerable.Range(1, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }

    public static string Base64UrlEncode(byte[] input) {
        return Convert.ToBase64String(input)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    public static byte[] Base64UrlDecode(string input) {
        string output = input.Replace('-', '+').Replace('_', '/');

        switch (output.Length % 4) {
            case 2: output += "=="; break;
            case 3: output += "="; break;
        }

        return Convert.FromBase64String(output);
    }
}
