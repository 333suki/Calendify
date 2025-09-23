using System.Security.Cryptography;
using System.Text;

namespace Backend;

public static class HashUtils {
    public static string Sha256Hash(string input) {
        using (var sha256 = SHA256.Create()) {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
