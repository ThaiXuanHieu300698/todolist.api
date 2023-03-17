using System;
using System.Collections.Generic;

namespace TodoList.Api.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public string Path { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; } 
    }
}