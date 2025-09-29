using System.Text.Json.Serialization;

namespace Backend.Authorization;

public class Payload {
    [JsonPropertyName("sub")] public string Sub { get; set; }
    [JsonPropertyName("iat")] public long Iat { get; set; }
    [JsonPropertyName("exp")] public long Exp { get; set; }

    public Payload(string sub, long iat, long exp) {
        Sub = sub;
        Iat = iat;
        Exp = exp;
    }
}
