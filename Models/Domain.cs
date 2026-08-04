using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace TempMailBox.Models
{
    public class Domain
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("domain")]
        public string DomainName { get; set; } = string.Empty;

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("isPrivate")]
        public bool IsPrivate { get; set; }
    }

    public class DomainListResponse
    {
        [JsonPropertyName("hydra:member")]
        public List<Domain> Members { get; set; } = new();
    }
}
