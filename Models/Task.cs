using System;
using System.Collections.Generic;

namespace TodoList.Api.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsComplete { get; set; }
        public bool IsImportant { get; set; }
        public List<Step> Steps { get; set; } = new List<Step>();
        public List<File> Files { get; set; } = new List<File>();
    }
}