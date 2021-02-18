using System;

namespace Domain.Models
{
    public class Article : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Author Author { get; set; }
    }
}
