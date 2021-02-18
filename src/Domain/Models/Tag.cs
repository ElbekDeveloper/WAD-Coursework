using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Tag : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Article> Articles { get; set; }
    }
}
