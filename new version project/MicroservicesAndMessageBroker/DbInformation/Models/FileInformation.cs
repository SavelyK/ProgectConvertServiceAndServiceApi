using System;
using System.Collections.Generic;
using System.Text;

namespace DbInformation.Models
{
   public class FileInformation
    {
        public Guid Id { get; set; }

        public string Path { get; set; }
        public string Status { get; set; }
    }
}
