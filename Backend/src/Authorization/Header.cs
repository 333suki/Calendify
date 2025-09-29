using System.Text.Json.Serialization;

namespace Backend.Authorization;

public class Header {
    [JsonPropertyName("alg")] public string Alg { get; set; }
    [JsonPropertyName("typ")] public string Typ { get; set; }

    public Header(string alg, string typ) {
        Alg = alg;
        Typ = typ;
    }
}
