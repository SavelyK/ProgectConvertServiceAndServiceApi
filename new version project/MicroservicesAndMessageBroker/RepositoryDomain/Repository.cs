﻿
using System;

namespace RepositoryDomain
{
    public class Repository
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime LoadTime { get; set; }
        public DateTime? TaskTime { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public long FileLength { get; set; }
        public int Port { get; set; }
        
    }
}