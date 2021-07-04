using System;

namespace LibraryModels
{
    public class DbModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime LoadTime { get; set; }
        public DateTime TaskTime { get; set; }
        public int Status { get; set; }
        public string NewPath { get; set; }
        public string NewFileName { get; set; }
        public int Priority { get; set; }
        public int TaskIndicator1 { get; set; }
        public int TaskIndicator2 { get; set; }
        public int TaskIndicator3 { get; set; }
        public int TaskIndicator4 { get; set; }
        public int TaskIndicator5 { get; set; }

    }
}
