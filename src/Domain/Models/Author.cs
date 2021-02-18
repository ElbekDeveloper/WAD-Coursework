using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Author : IEntity<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IList<Article> Articles { get; set; }

    }
}
