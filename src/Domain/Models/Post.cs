using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Post : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
