using System.Collections.Generic;

namespace BlogLab.Models.Blog
{
    public class PageResults<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}