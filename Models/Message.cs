using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TempMailBox.Models
{
    public class MessageAddress
    {
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class Message
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("accountId")]
        public string AccountId { get; set; } = string.Empty;

        [JsonPropertyName("msgid")]
        public string MsgId { get; set; } = string.Empty;

        [JsonPropertyName("from")]
        public MessageAddress From { get; set; } = new();

        [JsonPropertyName("to")]
        public List<MessageAddress> To { get; set; } = new();

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("intro")]
        public string Intro { get; set; } = string.Empty;

        [JsonPropertyName("seen")]
        public bool Seen { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("hasAttachments")]
        public bool HasAttachments { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("downloadUrl")]
        public string DownloadUrl { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("html")]
        public List<string>? Html { get; set; }

        // Display helpers
        public string FromDisplay => string.IsNullOrEmpty(From?.Name) ? From?.Address ?? "" : From.Name;
        public string DateDisplay => CreatedAt.ToString("MM/dd HH:mm");
        public string SubjectDisplay => string.IsNullOrEmpty(Subject) ? "(无主题)" : Subject;
    }

    public class MessageListResponse
    {
        [JsonPropertyName("hydra:member")]
        public List<Message> Members { get; set; } = new();

        [JsonPropertyName("hydra:totalItems")]
        public int TotalItems { get; set; }
    }
}
