namespace TodoList.Api.Models
{
    public class Step
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }

    }
}