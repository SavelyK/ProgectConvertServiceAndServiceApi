using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryModels
{
   public class Reserv
    {
        public int TaskId { get; set; }
        public string FilePath { get; set; }
    public Reserv(int taskId, string filePath)
    {
        TaskId = taskId;
        FilePath = filePath;
    }
    }
}
