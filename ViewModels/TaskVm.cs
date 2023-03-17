using System;
using System.Collections.Generic;

namespace TodoList.Api.ViewModels
{
    public class TaskVm
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsComplete { get; set; }
    }
}