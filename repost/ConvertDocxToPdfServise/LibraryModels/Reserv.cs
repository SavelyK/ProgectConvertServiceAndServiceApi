using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryModels
{
   public class Reserv
    {
        public int TaskId { get; set; }
        public string FilePath { get; set; }
        public DateTime TimeRegistrInDb { get; set; }
        public long FileLength { get; set; }
        public Reserv(int taskId, string filePath, DateTime timeRegistrInDb, long fileLength)
    {
        TaskId = taskId;
        FilePath = filePath;
        TimeRegistrInDb = timeRegistrInDb;
        FileLength = fileLength;
    }
    }
}
