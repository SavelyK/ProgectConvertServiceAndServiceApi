using System;
using System.Text.Json.Serialization;

namespace ApiDownload.Models
{
    public class DocxItemModel
    {

        public Guid Id { get; set; }
 
        public string Path { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }

        public DocxItemModel(Guid id, string path, string status, string fileName)
        {
            Id = id;
            Path = path;
            Status = status;
            FileName = fileName;
        }
    }
}
