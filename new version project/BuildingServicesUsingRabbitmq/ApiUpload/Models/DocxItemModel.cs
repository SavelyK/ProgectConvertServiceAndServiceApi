using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiUpload.Models
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
