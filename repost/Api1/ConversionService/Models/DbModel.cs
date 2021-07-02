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
        public int Indicator { get; set; }
        public string NewPath { get; set; }
        public string NewFileName { get; set; }
        public int Priority { get; set; }
        public int TaskIndicator { get; set; }
    }
}

