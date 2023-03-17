using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Api.ViewModels
{
    public class CreateTaskRequest
    {
        [Required]
        public string Title { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsComplete { get; set; }
        public bool IsImportant { get; set; }
    }
}