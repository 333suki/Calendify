using System.Text.Json.Serialization;

namespace Backend.Authorization;

public class Payload {
    [JsonPropertyName("sub")] public string Sub { get; set; }
    [JsonPropertyName("iat")] public long Iat { get; set; }
    [JsonPropertyName("exp")] public long Exp { get; set; }
    [JsonPropertyName("role")] public int Role { get; set; }

    public Payload(string sub, long iat, long exp, int role) {
        Sub = sub;
        Iat = iat;
        Exp = exp;
        Role = role;
    }
}
