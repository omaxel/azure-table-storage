using Azure;
using Azure.Data.Tables;
using System;

namespace Todos.Models
{
    public class Todo : ITableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}