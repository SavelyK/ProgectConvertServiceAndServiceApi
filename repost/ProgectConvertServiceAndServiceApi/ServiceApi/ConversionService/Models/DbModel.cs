using System;


namespace ConversionService.Models
{
    public class DbModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime LoadTime { get; set; }
        public DateTime TaskTime { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public long FileLength { get; set; }
    }
}

