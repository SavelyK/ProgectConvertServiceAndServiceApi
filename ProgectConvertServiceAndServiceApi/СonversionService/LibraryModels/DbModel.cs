﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryModels
{
    public class DbModel
    {
        public enum StatusProces
        {
            FileUpload,
            Wait,
            InProgress,
            Completed
        }
        public Guid UserId { get; set; }
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime LoadTime { get; set; }
        public DateTime TaskTime { get; set; }
        public StatusProces Status { get; set; }
        public int Priority { get; set; }
        public long FileLength { get; set; }

    }
}
