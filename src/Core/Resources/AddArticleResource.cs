using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Resources
{
    public class AddArticleResource
    {
        [Required]
        [MaxLength(2000)]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [Required]
        public string Body { get; set; }
        public int AuthorId { get; set; }
        public ICollection<int> Tags { get; set; }
        public AddArticleResource()
        {
            Tags = new Collection<int>();
        }
    }
}
