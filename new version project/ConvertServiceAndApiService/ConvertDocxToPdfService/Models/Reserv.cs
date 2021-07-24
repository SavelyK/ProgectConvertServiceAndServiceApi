using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertDocxToPdfService.Models
{
    public class Reserv
    {
        public Guid TaskId { get; set; }
        public string FilePath { get; set; }
        public DateTime TimeRegistrInDb { get; set; }
        public long FileLength { get; set; }
        public Reserv(Guid taskId, string filePath, DateTime timeRegistrInDb, long fileLength)
        {
            TaskId = taskId;
            FilePath = filePath;
            TimeRegistrInDb = timeRegistrInDb;
            FileLength = fileLength;
        }
    }
}
