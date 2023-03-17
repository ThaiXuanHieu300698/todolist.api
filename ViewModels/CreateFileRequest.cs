namespace TodoList.Api.ViewModels
{
    public class CreateFileRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public string Path { get; set; }
        public int TaskId { get; set; }
    }
}