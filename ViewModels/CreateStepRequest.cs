namespace TodoList.Api.ViewModels
{
    public class CreateStepRequest
    {
        public string Title { get; set; }
        public bool IsComplete { get; set; }
        public int TaskId { get; set; }
    }
}