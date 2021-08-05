using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ConvertService.Models
{
    class DocxItemModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("loadTime")]
        public DateTime LoadTime { get; set; }

        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonPropertyName("fileLength")]
        public long FileLength { get; set; }
       
    }
}
