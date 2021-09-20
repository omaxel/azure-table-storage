namespace Todos.Models
{
    public class TodoDto
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
