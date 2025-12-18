using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Backend.Authorization;

public class AuthUtils {
    // public static bool VerifyToken(string token) {
    //     string[] tokenSplit = token.Split('.');
    //     if (tokenSplit.Length != 3) {
    //         return false;
    //     }
    //
    //     string headerEncoded = tokenSplit[0];
    //     string payloadEncoded = tokenSplit[1];
    //     string signature = tokenSplit[2];
    //
    //
    //     using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(TokenGenerator.Secret))) {
    //         if (AuthUtils.Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes($"{headerEncoded}.{payloadEncoded}"))) !=
    //             signature) {
    //             return false;
    //         }
    //     }
    //
    //     return true;
    // }

    public enum TokenParseResult {
        Ok,
        Invalid,
        InvalidFormat,
        HeaderNullOrEmpty,
        HeaderDeserializeError,
        PayloadNullOrEmpty,
        PayloadDeserializeError,
        SignatureNullOrEmpty,
        TokenExpired
    }

    /// <summary>
    /// Verifies and parses an Authentication token
    /// </summary>
    /// <param name="token">The token</param>
    /// <param name="result">The result of the function, if this function returns false, this variable will hold extra information.</param>
    /// <param name="header">The deserialized token header field, only available if function returns true.</param>
    /// <param name="payload">The deserialized token payload field, only available if function returns true.</param>
    /// <returns>True if token is valid, else false.</returns>
    public static bool ParseToken(string token, out TokenParseResult result, out Header? header, out Payload? payload) {
        string[] tokenSplit = token.Split('.');
        if (tokenSplit.Length != 3) {
            result = TokenParseResult.InvalidFormat;
            header = null;
            payload = null;
            return false;
        }
        
        string headerEncoded = tokenSplit[0];
        string payloadEncoded = tokenSplit[1];
        string signature = tokenSplit[2];
        if (String.IsNullOrEmpty(signature)) {
            result = TokenParseResult.SignatureNullOrEmpty;
            header = null;
            payload = null;
            return false;
        }
        
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(TokenGenerator.Secret))) {
            if (Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes($"{headerEncoded}.{payloadEncoded}"))) != signature) {
                result = TokenParseResult.Invalid;
                header = null;
                payload = null;
                return false;
            }
        }

        string headerJson = "";
        try {
            headerJson = Encoding.UTF8.GetString(Base64UrlDecode(tokenSplit[0]));
        }
        catch {
            result = TokenParseResult.Invalid;
            header = null;
            payload = null;
            return false;
        }
        if (String.IsNullOrEmpty(headerJson)) {
            result = TokenParseResult.HeaderNullOrEmpty;
            header = null;
            payload = null;
            return false;
        }

        string payloadJson = "";
        try {
            payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(tokenSplit[1]));
        }
        catch {
            result = TokenParseResult.Invalid;
            header = null;
            payload = null;
            return false;
        }
        if (String.IsNullOrEmpty(payloadJson)) {
            result = TokenParseResult.PayloadNullOrEmpty;
            header = null;
            payload = null;
            return false;
        }
        
        Header? deserializedHeader = JsonSerializer.Deserialize<Header>(headerJson);
        if (deserializedHeader is null) {
            result = TokenParseResult.HeaderDeserializeError;
            header = null;
            payload = null;
            return false;
        }
        
        if (deserializedHeader.Alg != "HS256")
        {
            result = TokenParseResult.Invalid;
            header = null;
            payload = null;
            return false;
        }
        
        // Console.WriteLine($"Alg: {header.Alg}");
        // Console.WriteLine($"Typ: {header.Typ}");
        
        Payload? deserializedPayload = JsonSerializer.Deserialize<Payload>(payloadJson);
        if (deserializedPayload is null) {
            result = TokenParseResult.PayloadDeserializeError;
            header = null;
            payload = null;
            return false;
        }
        
        // Console.WriteLine($"Sub: {payload.Sub}");
        // Console.WriteLine($"Iat: {payload.Iat} ({DateTimeOffset.FromUnixTimeSeconds(payload.Iat)})");
        // Console.WriteLine($"Exp: {payload.Exp} ({DateTimeOffset.FromUnixTimeSeconds(payload.Exp)})");
        // Console.WriteLine($"Now: {DateTimeOffset.Now.ToUnixTimeSeconds()} ({DateTimeOffset.UtcNow})");
        
        // Console.WriteLine($"Issued valid: {DateTimeOffset.FromUnixTimeSeconds(payload.Iat) < DateTimeOffset.UtcNow}");
        // Console.WriteLine($"Expired: {DateTimeOffset.FromUnixTimeSeconds(payload.Exp) < DateTimeOffset.UtcNow}");
        
        // Check if token is expired
        if (DateTimeOffset.FromUnixTimeSeconds(deserializedPayload.Exp) < DateTimeOffset.UtcNow) {
            result = TokenParseResult.TokenExpired;
            header = deserializedHeader;
            payload = deserializedPayload;
            return false;
        }

        result = TokenParseResult.Ok;
        header = deserializedHeader;
        payload = deserializedPayload;
        return true;
    }

    public static bool ValidAuthHeaderFormat(string authHeader) {
        if (authHeader.Split('.').Length != 3) {
            return false;
        }

        return true;
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
