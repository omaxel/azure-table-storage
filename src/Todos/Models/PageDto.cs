using System.Collections.Generic;

namespace Todos.Models
{
    public class PageDto<T>
    {
        public string ContinuationToken { get; set; }
        public IReadOnlyCollection<T> Values { get; set; }
    }
}
